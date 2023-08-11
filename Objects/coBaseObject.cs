using System;
using System.Collections.Generic;
using System.Data;
using System.Reflection;
using System.Text;

namespace Objects
{
    [Serializable()]
    public class coBaseObject
    {
        public static List<T> CreateObjectListFromDataTable<T>(DataTable dt) where T : new()
        {
            List<T> retVal = new List<T>();

            if (dt != null)
            {
                foreach (DataRow dr in dt.Rows)
                {
                    T obj = CreateItemFromRow<T>(dr);
                    retVal.Add(obj);
                }
            }

            return retVal;
        }

        public static T CreateObjectFromDataTable<T>(DataTable dt) where T : new()
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                T obj = CreateItemFromRow<T>(dt.Rows[0]);
                return obj;
            }

            return new T();
        }

        protected static T CreateItemFromRow<T>(DataRow row) where T : new()
        {
            T item = new T();

            SetItemFromRow(item, row);

            return item;
        }

        protected static void SetItemFromRow<T>(T item, DataRow row) where T : new()
        {
            foreach (DataColumn c in row.Table.Columns)
            {
                PropertyInfo p = item.GetType().GetProperty(c.ColumnName);

                if (p != null && row[c] != DBNull.Value)
                {
                    try
                    {
                        p.SetValue(item, row[c], null);
                    }
                    catch (Exception ex)
                    {
                        if (ex != null)
                        {

                        }
                    }


                }
            }
        }
    }
}
