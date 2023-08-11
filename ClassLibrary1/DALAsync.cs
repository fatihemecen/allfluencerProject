using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DALAsync
    {
        private string connectionString;
        private SqlConnection conn;

        public DALAsync(string cnn)
        {
            this.connectionString = cnn;
            this.conn = new SqlConnection(cnn);
        }
        public async Task<DataTable> GetDataTable(string commandText)
        {
            return await ExecuteQueryInternalAsync(commandText, null, null).ConfigureAwait(false);
        }
        public async Task<DataTable> GetDataTable(SqlCommand cmd)
        {
            return await ExecuteQueryInternalAsync(cmd, null).ConfigureAwait(false);
        }
        public async Task<int> ExecuteNonQueryAsync(string commandText)
        {
            return await ExecuteAsync(commandText, null, null).ConfigureAwait(false);
        }
        public async Task<int> ExecuteNonQueryAsync(SqlCommand cmd)
        {
            return await ExecuteAsync(cmd, null).ConfigureAwait(false);
        }
        public async Task<object> ExecuteScalarAsync(string commandText)
        {
            return await ExecuteScalarAsync(commandText, null, null).ConfigureAwait(false);
        }
        public async Task<object> ExecuteScalarAsync(SqlCommand cmd)
        {
            return await ExecuteScalarAsync(cmd, null).ConfigureAwait(false);
        }
        public async Task<int> ExecuteIntScalarAsync(string commandText)
        {
            object retVal = await ExecuteScalarAsync(commandText, null, null).ConfigureAwait(false);
            if (retVal != null)
            {
                return Convert.ToInt32(retVal);
            }
            else
            {
                return -1;
            }
        }
        public async Task<int> ExecuteIntScalarAsync(SqlCommand cmd)
        {
            object retVal = await ExecuteScalarAsync(cmd, null).ConfigureAwait(false);
            if (retVal != null)
            {
                return Convert.ToInt32(retVal);
            }
            else
            {
                return -1;
            }
        }
        async Task<int> ExecuteAsync(string commandText, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(commandText);
            cmd.LoadParams(parameters);
            return await ExecuteAsync(cmd, transaction).ConfigureAwait(false);
        }
        async Task<int> ExecuteAsync(SqlCommand cmd, SqlTransaction transaction)
        {
            if (cmd.Connection == null)
            {
                cmd.Connection = new SqlConnection(this.connectionString);
            }
            cmd.CommandTimeout = 0;

            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }

            await cmd.Connection.OpenAsync().ConfigureAwait(false);
            try
            {
                return await cmd.ExecuteNonQueryAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return -1;
            }
            finally
            {
                cmd.Connection.Close();
            }

        }
        async Task<object> ExecuteScalarAsync(string commandText, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(commandText);
            cmd.LoadParams(parameters);
            cmd.CommandTimeout = 0;
            return await ExecuteScalarAsync(cmd, transaction).ConfigureAwait(false);
        }
        async Task<object> ExecuteScalarAsync(SqlCommand cmd, SqlTransaction transaction)
        {
            if (cmd.Connection == null)
            {
                cmd.Connection = new SqlConnection(this.connectionString);
            }
            cmd.CommandTimeout = 0;

            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }

            try
            {
                await cmd.Connection.OpenAsync().ConfigureAwait(false);
                return await cmd.ExecuteScalarAsync().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmd.Connection.Close();
            }
        }
        async Task<DataTable> ExecuteQueryInternalAsync(string commandText, SqlTransaction transaction, params SqlParameter[] parameters)
        {
            SqlCommand cmd = new SqlCommand(commandText);
            cmd.LoadParams(parameters);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 0;
            return await ExecuteQueryInternalAsync(cmd, transaction).ConfigureAwait(false);

        }
        async Task<DataTable> ExecuteQueryInternalAsync(SqlCommand cmd, SqlTransaction transaction)
        {
            if (cmd.Connection == null)
            {
                cmd.Connection = new SqlConnection(this.connectionString);
            }
            cmd.CommandTimeout = 0;

            if (transaction != null)
            {
                cmd.Transaction = transaction;
            }

            if (cmd.Connection.State == ConnectionState.Closed)
            {
                Console.WriteLine("Start Open: " + DateTime.Now);
                await cmd.Connection.OpenAsync();
                Console.WriteLine("Finish Open: " + DateTime.Now);
            }

            try
            {
                var datatable = await cmd.ExecuteAndCreateDataTableAsync().ConfigureAwait(false);
                return datatable;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
                cmd.Connection.Close();
            }

        }
        public static SqlCommand CommandForProcedure(string procedureName)
        {
            SqlCommand cmd = new SqlCommand(procedureName);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.CommandTimeout = 0;
            return cmd;
        }
    }

    public static class extensions
    {
        public async static Task<DataTable> ExecuteAndCreateDataTableAsync(string sql)
        {
            return await ExecuteAndCreateDataTableAsync(new SqlCommand(sql)).ConfigureAwait(false);
        }
        public async static Task<DataTable> ExecuteAndCreateDataTableAsync(this SqlCommand cmd)
        {
            using (var reader = await cmd.ExecuteReaderAsync().ConfigureAwait(false))
            {
                var dataTable = reader.CreateTableSchema();
                while (await reader.ReadAsync().ConfigureAwait(false))
                {
                    var dataRow = dataTable.NewRow();
                    for (int i = 0; i < dataTable.Columns.Count; i++)
                    {
                        dataRow[i] = reader[i];
                    }
                    dataTable.Rows.Add(dataRow);
                }
                return dataTable;
            }
        }
        public static void LoadParams(this SqlCommand cmd, params SqlParameter[] parameters)
        {
            if (parameters != null)
            {
                foreach (var parameter in parameters)
                {
                    if (parameter != null)
                    {
                        if (parameter.Value == null)
                        {
                            parameter.Value = DBNull.Value;
                        }

                        cmd.Parameters.Add(parameter);
                    }
                }
            }
        }
        static DataTable CreateTableSchema(this SqlDataReader reader)
        {
            DataTable schema = reader.GetSchemaTable();
            DataTable dataTable = new DataTable();
            if (schema != null)
            {
                foreach (DataRow drow in schema.Rows)
                {
                    string columnName = System.Convert.ToString(drow["ColumnName"]);
                    DataColumn column = new DataColumn(columnName, (Type)(drow["DataType"]));
                    dataTable.Columns.Add(column);
                }
            }
            return dataTable;
        }
    }
}
