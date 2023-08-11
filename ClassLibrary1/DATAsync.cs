using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;
using Data;
using static Util.Statics.Enums;
using BlazorApp.Shared;

namespace Data
{
    public class DATAsync
    {
        private DALAsync db;

        public DATAsync(string cnn)
        {
            db = new DALAsync(cnn);
        }

        #region UserAndLogin

        public async Task<string> CreateUser(string userName, string userFirstName, string userLastName, string userEmail, string userPassword, int userRole)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUser");
            cmd.Parameters.AddWithValue("@UserName", userName);
            cmd.Parameters.AddWithValue("@FirstName", userFirstName);
            cmd.Parameters.AddWithValue("@LastName", userLastName);
            cmd.Parameters.AddWithValue("@UserEmail", userEmail);
            cmd.Parameters.AddWithValue("@UserPassword", userPassword);
            cmd.Parameters.AddWithValue("@UserRole", userRole);
            

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return "";
            }

            string jwtToken = ""; //FunctionHelper.GenerateTokenJWT(userName, userFirstName, userLastName, userEmail, userRole);

            return jwtToken;
        }
        public async Task<bool> CheckUser(string userEmail,string userName)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CheckUser");
            cmd.Parameters.AddWithValue("@UserEmail", userEmail);
            cmd.Parameters.AddWithValue("@UserName", userName);


            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<string> LoginUser(string userEmail, string userPassword)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("LoginUser");
            cmd.Parameters.AddWithValue("@UserEmail", userEmail);
            cmd.Parameters.AddWithValue("@UserPassword", userPassword);


            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return "";
            }
            //var user = await db.ExecuteNonQueryAsync("SELECT UserName,FirstName,LastName,UserEmail,UserRole FROM [User] WHERE UserEmail = 'denemefatih@gmail.com' AND UserPassword = 'EvilSmurff1!' ").ConfigureAwait(false);
            coUser user2 = new coUser();
            /*user2.UserEmail = db.ExecuteNonQueryAsync("SELECT UserEmail FROM [USER] WHERE UserName = 'denemefatih@gmail.com'").Result.ToString();
            user2.UserName = db.ExecuteNonQueryAsync("SELECT UserName FROM [USER] WHERE UserName = 'denemefatih@gmail.com'").Result.ToString();
            user2.FirstName = db.ExecuteNonQueryAsync("SELECT FirstName FROM [USER] WHERE UserName = 'denemefatih@gmail.com'").Result.ToString();
            user2.LastName = db.ExecuteNonQueryAsync("SELECT LastName FROM [USER] WHERE UserName = 'denemefatih@gmail.com'").Result.ToString();
            user2.UserRole = db.ExecuteNonQueryAsync("SELECT UserEmail FROM [USER] WHERE UserName = 'denemefatih@gmail.com'").Result;
            */
            user2.UserEmail = "denemefatih@gmail.com";
            user2.UserName = "denemefatih";
            user2.FirstName = "Fatih";
            user2.LastName = "Emecen";
            user2.UserRole = 1;

            string jwtToken = ""; //FunctionHelper.GenerateTokenJWT(user2);
            return jwtToken;
        }
        public async Task<DataTable> GetUserList()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserList");
            coUser user = new coUser();
            user.UserEmail = db.ExecuteNonQueryAsync(cmd).Result.ToString();
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }





        public async Task<DataTable> GetOrganizationByCode(string code)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetOrganizationByCode");
            cmd.Parameters.AddWithValue("@Code", code);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetOrganizationByID(int organizationID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetOrganizationByID");
            cmd.Parameters.AddWithValue("@OrganizationID", organizationID);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetUserByEmailAndPassword(string useremail, string password, string organizationcode)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserByEmailAndPassword");
            cmd.Parameters.AddWithValue("@UserEmail", useremail);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@OrganizationCode", organizationcode);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<int> ChangeUserPassword(int userid, string oldpassword, string newpassword)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("ChangeUserPassword");
            cmd.Parameters.AddWithValue("@UserID", userid);
            addStringParameter("@OldPassword", oldpassword, cmd);
            addStringParameter("@NewPassword", newpassword, cmd);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {

            }

            return 0;
        }
        public async Task<DataTable> GetUserByEposta(string useremail, string orgcode)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserByEposta");
            cmd.Parameters.AddWithValue("@UserEmail", useremail);
            cmd.Parameters.AddWithValue("@OrganizationCode", orgcode);
            //addStringParameter("@UserName", useremail, cmd);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetUserByUserID(int UserID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserByUserID");
            cmd.Parameters.AddWithValue("@UserID", UserID);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetUserByText(string searchText, int skipIndex, int takeCount, int userType, int invexType)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("SearchUserByText");
            cmd.Parameters.AddWithValue("@SearchText", searchText);
            cmd.Parameters.AddWithValue("@SkipIndex", skipIndex);
            cmd.Parameters.AddWithValue("@TakeCount", takeCount);
            cmd.Parameters.AddWithValue("@UserType", userType);
            cmd.Parameters.AddWithValue("@InvexType", invexType);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetExByOpID(int userType, string userID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetExByOpID");
            cmd.Parameters.AddWithValue("@UserType", userType);
            cmd.Parameters.AddWithValue("@UserID", userID);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> AddExpert(string operatorID, string userID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("AddExpert");
            cmd.Parameters.AddWithValue("@OperatorID", operatorID);
            cmd.Parameters.AddWithValue("@UserID", userID);
            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteExpert(string userID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteExpert");
            cmd.Parameters.AddWithValue("@UserID", userID);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteUserByGuid(string userID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteUserByGuid");
            cmd.Parameters.AddWithValue("@UserID", userID);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateUserDeviceInfo(string deviceip, string devicename, string invextoken, int userid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateUserDeviceInfo");
            addStringParameter("@UserDeviceIp", deviceip, cmd);
            addStringParameter("@UserDeviceName", devicename, cmd);
            addStringParameter("@UserInvexToken", invextoken, cmd);
            cmd.Parameters.AddWithValue("@UserID", userid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion UserAndLogin
        #region Report
        public async Task<DataTable> GetReportByCodeAndUserID(string code, int userid, int userrole, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportByCodeAndUserID_New");
            cmd.Parameters.AddWithValue("@Code", code);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserRole", userrole);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportFilesByReportID(int reportid, int typeid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportFilesByReportID");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@TypeID", typeid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetUserReportsByUserID(int userid, int skipindex, int takecount, int organizationid,
            int usertype, int randevu, int takbis, int late)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserReportsByUserID_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@SkipIndex", skipindex);
            cmd.Parameters.AddWithValue("@TakeCount", takecount);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@Randevu", randevu);
            cmd.Parameters.AddWithValue("@Takbis", takbis);
            cmd.Parameters.AddWithValue("@Late", late);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetUserReports(int userid, int skipindex, int takecount)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserReports");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@SkipIndex", skipindex);
            cmd.Parameters.AddWithValue("@TakeCount", takecount);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportByReportID(int userid, int usertype, int reportid, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportByReportID_New");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetLastTenReport(int userType, string userID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetLastTenReport");
            cmd.Parameters.AddWithValue("@UserType", userType);
            cmd.Parameters.AddWithValue("@UserID", userID);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> CreateReportFileNew(int userid, int reportid, int filetypeid, string fileguid,
            DateTime datetime, DateTime createddate, string createddatest, int housingpartindex, int partcountindex, int organizationid,
            int subtype, int iskapak, int useinreport, string invextype, int sourcetype, int sysid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportFileNew_New");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@FileTypeID", filetypeid);
            cmd.Parameters.AddWithValue("@FileGuid", fileguid);
            cmd.Parameters.AddWithValue("@DateTime", datetime);
            cmd.Parameters.AddWithValue("@CreatedDate", createddate);
            cmd.Parameters.AddWithValue("@CreatedDateSt", createddatest);
            cmd.Parameters.AddWithValue("@HousingPartIndex", housingpartindex);
            cmd.Parameters.AddWithValue("@PartCountIndex", partcountindex);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@SubType", subtype);
            cmd.Parameters.AddWithValue("@IsKapak", iskapak);
            cmd.Parameters.AddWithValue("@UseInReport", useinreport);
            addStringParameter("@InvexType", invextype, cmd);
            cmd.Parameters.AddWithValue("@SourceType", sourcetype);
            cmd.Parameters.AddWithValue("@SysID", sysid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteReportFile(int reportfileid, int userid, DateTime datetime, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteReportFile_New");
            cmd.Parameters.AddWithValue("@ReportFileID", reportfileid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTime", datetime);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteReportFileBySysID(int reportfileid, int userid, DateTime datetime, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteReportFileBySysID");
            cmd.Parameters.AddWithValue("@ReportFileSysID", reportfileid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTime", datetime);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateReport(int userid, int reportid, DateTime datetime, string location, int organizationid, int photoproportion)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateReport_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@DateTime", datetime);
            addStringParameter("@Location", location, cmd);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@DateTick", datetime.Ticks);
            cmd.Parameters.AddWithValue("@PhotoProportion", photoproportion);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        /*public async Task<DataTable> GetReportFormData(int userid, int banktemplateid, int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportFormData");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }*/
        public async Task<int> GetReportFormDataVersionNo(int reportid, int organizationid)
        {

            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportFormDataVersionNo_New");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {

            }

            return 0;
        }
        public async Task<DataTable> GetTemplateData(int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateData");
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportFormData_New(int banktemplateid, int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportFormData_New");
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportDataBySysInfo(int reportid, string syscode, string kod, string tapukod)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportDataBySysInfo");
            cmd.Parameters.AddWithValue("@BankTemplateID", 0);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            addStringParameter("@SysCode", syscode, cmd);
            addStringParameter("@Kod", kod, cmd);
            addStringParameter("@TapuKod", tapukod, cmd);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<(int res, int uzmanid, int operasyonuserid)> UpdateReportInvexState(string sirket_id, int basvuru_id, int etkin, int islem, int silindi, int ReportState,
            string actioninfo, string actionDescription, DateTime dateTime, string basvuru_rapor, string birimi)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateReportInvexState");
            addStringParameter("@sirket_id", sirket_id, cmd);
            cmd.Parameters.AddWithValue("@basvuru_id", basvuru_id);
            //cmd.Parameters.AddWithValue("@uzman", uzman);
            cmd.Parameters.AddWithValue("@etkin", etkin);
            cmd.Parameters.AddWithValue("@islem", islem);
            cmd.Parameters.AddWithValue("@silindi", silindi);
            cmd.Parameters.AddWithValue("@ReportState", ReportState);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            addStringParameter("@ReportInvexActionInfo", actioninfo, cmd);
            addStringParameter("@ReportInvexActionDescription", actionDescription, cmd);
            addStringParameter("@basvuru_rapor", basvuru_rapor, cmd);
            addStringParameter("@birimi", birimi, cmd);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);
            addOutPutParametr("@UzmanID", SqlDbType.Int, cmd);
            addOutPutParametr("@OperasyonUserID", SqlDbType.Int, cmd);

            int res = 0;
            int uzmanid = 0;
            int operasyonuserid = 0;

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    res = Convert.ToInt32(obj);
                    uzmanid = getOutputParamIntValue(cmd, "@UzmanID");
                    operasyonuserid = getOutputParamIntValue(cmd, "@OperasyonUserID");
                }
            }
            catch
            {

            }

            return (res, uzmanid, operasyonuserid);
        }
        public async Task<bool> DeleteSyncCompletedData()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteSyncCompletedData");

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetInvexReportFiles(int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetInvexReportFiles");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> UpdateReportVersionNo(int reportID, DateTime dateTime, int userID)
        {
            SqlCommand cmd = new SqlCommand("UpdateReportVersionNo");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportID", reportID);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@UserID", userID);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetAllReportFilesByReportID(int reportid, int resourcetype)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetAllReportFilesByReportID");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@ResourceType", resourcetype);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> ChangeReportYazimState(int reportid, int newstate, int userid, DateTime dateTime)
        {
            SqlCommand cmd = new SqlCommand("ChangeReportYazimState");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@NewState", newstate);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateReportRandevu(int reportid, int userid, long randevudatetick, DateTime dateTime)
        {
            SqlCommand cmd = new SqlCommand("UpdateReportRandevu");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@RandevuDateTick", randevudatetick);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<(bool, int)> UpdateReportTakbis(int kod, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateReportTakbis");
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            addOutPutParametr("@AssignedUserID", SqlDbType.Int, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                int assigneduserid = getOutputParamIntValue(cmd, "@AssignedUserID");
                return (true, assigneduserid);
            }

            return (false, 0);
        }
        public async Task<bool> CreateUpdateInvexReportFoto(int kod, int basvuru_kod, int kullanici, int typeid, string guid, DateTime dateTime,
            DateTime tarih, string tarihst, int subtype, int iskapak, int useinreport, string invextype, int sourcetype, int size)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUpdateInvexReportFoto");
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@kullanici", kullanici);
            cmd.Parameters.AddWithValue("@typeid", typeid);
            cmd.Parameters.AddWithValue("@guid", guid);
            cmd.Parameters.AddWithValue("@dateTime", dateTime);
            cmd.Parameters.AddWithValue("@tarih", tarih);
            cmd.Parameters.AddWithValue("@tarihst", tarihst);
            cmd.Parameters.AddWithValue("@subtype", subtype);
            cmd.Parameters.AddWithValue("@iskapak", iskapak);
            cmd.Parameters.AddWithValue("@useinreport", useinreport);
            addStringParameter("@invextype", invextype, cmd);
            cmd.Parameters.AddWithValue("@sourcetype", sourcetype);
            cmd.Parameters.AddWithValue("@size", size);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateReportFileInUseKapakState(int reportfileid, int usein, int iskapak, int reportid, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateReportFileInUseKapakState");
            cmd.Parameters.AddWithValue("@ReportFileID", reportfileid);
            cmd.Parameters.AddWithValue("@UseIn", usein);
            cmd.Parameters.AddWithValue("@IsKapak", iskapak);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateReportAdaParselByReportID(int basvuru_kod, string ada, string parsel, DateTime dateTime)
        {
            SqlCommand cmd = new SqlCommand("UpdateReportAdaParselByReportID");
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@Ada", ada);
            cmd.Parameters.AddWithValue("@Parsel", parsel);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetReportFillState(DateTime prevDate, DateTime nextDate)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportFillState");
            cmd.Parameters.AddWithValue("@PrevDate", prevDate);
            cmd.Parameters.AddWithValue("@NextDate", nextDate);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportFillStateDetails(int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportFillStateDetails");
            cmd.Parameters.AddWithValue("@ReportID", reportid);


            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetUserReports_New(int userid, int usertype, int organizationid,
            string reportcode, int reportsysid, int bankid,
            string province, string county, string neighborhood,
            int state, long datestart, long dateend, int skipindex, int takecount)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserReports_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            addStringParameter("@ReportCode", reportcode, cmd);
            cmd.Parameters.AddWithValue("@ReportSysID", reportsysid);
            cmd.Parameters.AddWithValue("@BankID", bankid);
            addStringParameter("@Province", province, cmd);
            addStringParameter("@County", county, cmd);
            addStringParameter("@Neighborhood", neighborhood, cmd);
            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@DateStart", datestart);
            cmd.Parameters.AddWithValue("@DateEnd", dateend);
            cmd.Parameters.AddWithValue("@SkipIndex", skipindex);
            cmd.Parameters.AddWithValue("@TakeCount", takecount);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        #endregion Report
        #region ReportSubPartValue
        public async Task<int> CreateReportSubPartUserValue_New(int reportid, int subpartid, int userid, DateTime dateTime,
            string title, int reportsubpartuservalueid, int organizationid, int tapuid, int formproportion)
        {

            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportSubPartUserValue_New");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@SubPartID", subpartid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            addStringParameter("@Title", title, cmd);
            cmd.Parameters.AddWithValue("@ReportSubPartUserValueID", reportsubpartuservalueid);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@TapuID", tapuid);
            cmd.Parameters.AddWithValue("@FormProportion", formproportion);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {

            }

            return 0;
        }
        public async Task<int> CreateReportSubPartUserFieldValue(int reportsubartuservalueid, int sectionfieldid,
            string reportsubpartuserfieldvalue, DateTime dateTime, int reportsubpartuserfieldvalueid, string sysid)
        {

            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportSubPartUserFieldValue");
            cmd.Parameters.AddWithValue("@ReportSubPartUserValueID", reportsubartuservalueid);
            cmd.Parameters.AddWithValue("@SectionFieldID", sectionfieldid);
            addStringParameter("@ReportSubPartUserFieldValue", reportsubpartuserfieldvalue, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@ReportSubPartUserFieldValueID", reportsubpartuserfieldvalueid);
            addStringParameter("@SysID", sysid, cmd);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {

            }

            return 0;
        }
        public async Task<bool> CopyReportSubPartUserValue(int reportSubPartUserValueID, DateTime dateTime, bool copyValues, int userID, int reportID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CopyReportSubPartUserValue");
            cmd.Parameters.AddWithValue("@ReportSubPartUserValueID", reportSubPartUserValueID);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@CopyValues", copyValues);
            cmd.Parameters.AddWithValue("@ReportID", reportID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteReportSubPartUserValue(int reportSubPartUserValueID, int userID, DateTime dateTime, int reportID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteReportSubPartUserValue");
            cmd.Parameters.AddWithValue("@ReportSubPartUserValueID", reportSubPartUserValueID);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@ReportID", reportID);
            cmd.Parameters.AddWithValue("@UserID", userID);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion ReportSubPartValue
        #region Dashboard
        public async Task<string> GetDashboardCounts(int userid, int usertype, long datestarttick, long dateendtick, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetDashboardCounts_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@DateStartTick", datestarttick);
            cmd.Parameters.AddWithValue("@DateEndTick", dateendtick);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null)
                {
                    return obj.ToString();
                }
            }
            catch
            {

            }

            return "";
        }
        public async Task<DataTable> GetDashboardReportNotes(int userid, int userrole, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetDashboardReportNotes_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserRole", userrole);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetDashboardReports(int userid, int usertype, int organizationid, int isall, int randevu, int takbis)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetDashboardReports_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@IsAll", isall);
            cmd.Parameters.AddWithValue("@Randevu", randevu);
            cmd.Parameters.AddWithValue("@Takbis", takbis);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetDashboardReports(int userid, int usertype, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetDashboardReports");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserType", usertype);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetAnnouncement(int userid, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetAnnouncement_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        #endregion Dashboard
        #region Adres
        public async Task<DataTable> GetProvince()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetProvince");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetCountyByProvinceID(int provinceid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetCountyByProvinceID");
            cmd.Parameters.AddWithValue("@ProvinceID", provinceid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetNeighborhoodByCountyID(int countyid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetNeighborhoodByCountyID");
            cmd.Parameters.AddWithValue("@CountyID", countyid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetVillageByCountyID(int countyid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetVillageByCountyID");
            cmd.Parameters.AddWithValue("@CountyID", countyid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        #endregion Adres
        #region UserReportCustomParameter
        public async Task<DataTable> GetUserReportCustomParameter(int userid, int sectionfieldid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetUserReportCustomParameter");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@SectionFieldID", sectionfieldid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> CreateUserReportCustomParameter(int userid, int sectionfieldid, string userreportcustomparametertext)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUserReportCustomParameter");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@SectionFieldID", sectionfieldid);
            cmd.Parameters.AddWithValue("@UserReportCustomParameterText", userreportcustomparametertext);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteUserReportCustomParameter(int userid, int userreportcustomparameterid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteUserReportCustomParameter");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserReportCustomParameterID", userreportcustomparameterid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion UserReportCustomParameter
        #region ReportNote
        public async Task<DataTable> GetAllReportNotes(int userid, int userrole, int skipindex, int takecount, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetAllReportNotes_New");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserRole", userrole);
            cmd.Parameters.AddWithValue("@SkipIndex", skipindex);
            cmd.Parameters.AddWithValue("@TakeCount", takecount);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportNotes(int reportid, int userid, int userrole)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportNotes");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@UserRole", userrole);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        /*
        public async Task<bool> CreateReportNote(int userid, int reportid, int type, string text,
            int ownertype, int subjectid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportNote");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@ReportNoteType", type);
            addStringParameter("@ReportNoteText", text, cmd);
            cmd.Parameters.AddWithValue("@ReportNoteOwnerType", ownertype);
            cmd.Parameters.AddWithValue("@ReportNoteSubjectID", subjectid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }*/
        public async Task<bool> CreateReportNoteNew(int userid, int reportid, int type, string text,
            int ownertype, int subjectid, int sd, int su, int sdu, string invextype, long datetick, string username)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportNoteNew");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@ReportNoteType", type);
            addStringParameter("@ReportNoteText", text, cmd);
            cmd.Parameters.AddWithValue("@ReportNoteOwnerType", ownertype);
            cmd.Parameters.AddWithValue("@ReportNoteSubjectID", subjectid);

            cmd.Parameters.AddWithValue("@SD", sd);
            cmd.Parameters.AddWithValue("@SU", su);
            cmd.Parameters.AddWithValue("@SDU", sdu);
            addStringParameter("@InvexType", invextype, cmd);
            cmd.Parameters.AddWithValue("@DateTick", datetick);
            addStringParameter("@UserName", username, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        /*
        public async Task<bool> CreateReportNoteByService(int userid, int reportid, string type, string text,
         int subjectid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportNoteByService");
            cmd.Parameters.AddWithValue("@kullanici", userid);
            cmd.Parameters.AddWithValue("@basvuru_kod", reportid);
            cmd.Parameters.AddWithValue("@tipi", type);
            addStringParameter("@ReportNoteText", text, cmd);
            cmd.Parameters.AddWithValue("@ReportNoteSubjectID", subjectid);
            

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }*/
        public async Task<bool> CreateReportNoteByServiceNew(int userid, int reportid, string type, string text,
         int subjectid, int sd, int su, int sdu, string invextype, long datetick, string username)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateReportNoteByServiceNew");
            cmd.Parameters.AddWithValue("@kullanici", userid);
            cmd.Parameters.AddWithValue("@tipi", type);
            addStringParameter("@ReportNoteText", text, cmd);
            cmd.Parameters.AddWithValue("@ReportNoteSubjectID", subjectid);
            cmd.Parameters.AddWithValue("@basvuru_kod", reportid);
            cmd.Parameters.AddWithValue("@SD", sd);
            cmd.Parameters.AddWithValue("@SU", su);
            cmd.Parameters.AddWithValue("@SDU", sdu);
            addStringParameter("@InvexType", invextype, cmd);
            cmd.Parameters.AddWithValue("@DateTick", datetick);
            addStringParameter("@UserName", username, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateNoteRead(int userid, int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateNoteRead");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetReportNoteSubjectList()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportNoteSubjectList");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }

        #endregion ReportNote
        #region Masraf
        public async Task<bool> CreateMasraf(int basvuru_kod, int UserID, int ResourceType,
            float degerleme_bedeli, float degerleme_bedeli_kdv, float degerleme_bedeli_toplam,
            float resmi_harc_tapu, float resmi_harc_tapu_kdv, float resmi_harc_tapu_toplam,
            float resmi_harc_belediye, float resmi_harc_belediye_kdv, float resmi_harc_belediye_toplam,
            float ulasim_bedeli, float ulasim_bedeli_kdv, float ulasim_bedeli_toplam,
            float konaklama_bedeli, float konaklama_bedeli_kdv, float konaklama_bedeli_toplam,
            float diger_harcamalar, float diger_harcamalar_kdv, float diger_harcamalar_toplam,
            int InvexID, string aciklama)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateMasraf");
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@ResourceType", ResourceType);
            cmd.Parameters.AddWithValue("@degerleme_bedeli", degerleme_bedeli);
            cmd.Parameters.AddWithValue("@degerleme_bedeli_kdv", degerleme_bedeli_kdv);
            cmd.Parameters.AddWithValue("@degerleme_bedeli_toplam", degerleme_bedeli_toplam);
            cmd.Parameters.AddWithValue("@resmi_harc_tapu", resmi_harc_tapu);
            cmd.Parameters.AddWithValue("@resmi_harc_tapu_kdv", resmi_harc_tapu_kdv);
            cmd.Parameters.AddWithValue("@resmi_harc_tapu_toplam", resmi_harc_tapu_toplam);
            cmd.Parameters.AddWithValue("@resmi_harc_belediye", resmi_harc_belediye);
            cmd.Parameters.AddWithValue("@resmi_harc_belediye_kdv", resmi_harc_belediye_kdv);
            cmd.Parameters.AddWithValue("@resmi_harc_belediye_toplam", resmi_harc_belediye_toplam);
            cmd.Parameters.AddWithValue("@ulasim_bedeli", ulasim_bedeli);
            cmd.Parameters.AddWithValue("@ulasim_bedeli_kdv", ulasim_bedeli_kdv);
            cmd.Parameters.AddWithValue("@ulasim_bedeli_toplam", ulasim_bedeli_toplam);
            cmd.Parameters.AddWithValue("@konaklama_bedeli", konaklama_bedeli);
            cmd.Parameters.AddWithValue("@konaklama_bedeli_kdv", konaklama_bedeli_kdv);
            cmd.Parameters.AddWithValue("@konaklama_bedeli_toplam", konaklama_bedeli_toplam);
            cmd.Parameters.AddWithValue("@diger_harcamalar", diger_harcamalar);
            cmd.Parameters.AddWithValue("@diger_harcamalar_kdv", diger_harcamalar_kdv);
            cmd.Parameters.AddWithValue("@diger_harcamalar_toplam", diger_harcamalar_toplam);
            cmd.Parameters.AddWithValue("@InvexID", InvexID);
            addStringParameter("@Aciklama", aciklama, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetMasrafList(int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetMasrafByReportID");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> CreateMasrafByMobile(int basvuru_kod, int reportid, int UserID, int ResourceType,
           float resmi_harc_tapu, float resmi_harc_tapu_kdv, float resmi_harc_tapu_toplam,
           float resmi_harc_belediye, float resmi_harc_belediye_kdv, float resmi_harc_belediye_toplam,
           float ulasim_bedeli, float ulasim_bedeli_kdv, float ulasim_bedeli_toplam,
           float konaklama_bedeli, float konaklama_bedeli_kdv, float konaklama_bedeli_toplam,
           float diger_harcamalar, float diger_harcamalar_kdv, float diger_harcamalar_toplam,
           string aciklama)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateMasrafByMobile");
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@UserID", UserID);
            cmd.Parameters.AddWithValue("@ResourceType", ResourceType);
            cmd.Parameters.AddWithValue("@resmi_harc_tapu", resmi_harc_tapu);
            cmd.Parameters.AddWithValue("@resmi_harc_tapu_kdv", resmi_harc_tapu_kdv);
            cmd.Parameters.AddWithValue("@resmi_harc_tapu_toplam", resmi_harc_tapu_toplam);
            cmd.Parameters.AddWithValue("@resmi_harc_belediye", resmi_harc_belediye);
            cmd.Parameters.AddWithValue("@resmi_harc_belediye_kdv", resmi_harc_belediye_kdv);
            cmd.Parameters.AddWithValue("@resmi_harc_belediye_toplam", resmi_harc_belediye_toplam);
            cmd.Parameters.AddWithValue("@ulasim_bedeli", ulasim_bedeli);
            cmd.Parameters.AddWithValue("@ulasim_bedeli_kdv", ulasim_bedeli_kdv);
            cmd.Parameters.AddWithValue("@ulasim_bedeli_toplam", ulasim_bedeli_toplam);
            cmd.Parameters.AddWithValue("@konaklama_bedeli", konaklama_bedeli);
            cmd.Parameters.AddWithValue("@konaklama_bedeli_kdv", konaklama_bedeli_kdv);
            cmd.Parameters.AddWithValue("@konaklama_bedeli_toplam", konaklama_bedeli_toplam);
            cmd.Parameters.AddWithValue("@diger_harcamalar", diger_harcamalar);
            cmd.Parameters.AddWithValue("@diger_harcamalar_kdv", diger_harcamalar_kdv);
            cmd.Parameters.AddWithValue("@diger_harcamalar_toplam", diger_harcamalar_toplam);
            addStringParameter("@Aciklama", aciklama, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion Masraf
        #region Other
        public async Task<int> GetTasinmazKimlikNoByReportID(int reportid)
        {

            SqlCommand cmd = DALAsync.CommandForProcedure("GetTasinmazKimlikNoByReportID");
            cmd.Parameters.AddWithValue("@ReportID", reportid);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {

            }

            return 0;
        }
        public async Task<DataTable> GetReportTakbisList(int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportTakbisList");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetDigitalMunicipality()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetDigitalMunicipality");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetTempReport()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTempReport");
            return await db.GetDataTable(cmd);
        }
        public async Task<int> InsertTestData(string jsonData, int dataType, string methodname, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("InsertTestData");
            //addStringParameter("@JsonData", jsonData, cmd);

            SqlParameter paramJson = new SqlParameter();
            paramJson.ParameterName = "@JsonData";
            paramJson.Value = jsonData;
            paramJson.Size = -1;
            paramJson.SqlDbType = SqlDbType.NText;
            cmd.Parameters.Add(paramJson);
            cmd.Parameters.AddWithValue("@DataType", dataType);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@MethodName", methodname);

            object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return Convert.ToInt32(obj);
            }

            return 0;
        }
        public async Task<bool> UpdateSyncDataState(int syncDataID, int state, int reportid, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateSyncDataState");
            cmd.Parameters.AddWithValue("@SyncDataID", syncDataID);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@State", state);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetSyncInCompletedData()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncInCompletedData");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetSyncDataByID(int syncoid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncDataByID");
            cmd.Parameters.AddWithValue("@SyncOID", syncoid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetSyncReportDataAll()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncReportDataAll");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        #endregion Other
        #region InvexSync
        public async Task<DataTable> GetOrganizationList()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetOrganizationList");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> CreateUpdateOrganization(string kisaad, string unvan, string logo, int orgid, DateTime dateTime, bool hasMobile)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUpdateOrganization");
            addStringParameter("@KisaAd", kisaad, cmd);
            addStringParameter("@Unvan", unvan, cmd);
            addStringParameter("@Logo", logo, cmd);
            cmd.Parameters.AddWithValue("@OrgID", orgid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@HasMobile", hasMobile);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateSyncLog(string synclogname, bool hasAnyError, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateSyncLog");
            addStringParameter("@SyncLogName", synclogname, cmd);
            cmd.Parameters.AddWithValue("@SyncLogHasAnyError", hasAnyError);
            cmd.Parameters.AddWithValue("@SyncLogDate", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateUpdateBank(int kod, string type, string adi, string kisaadi, string kodu, string logo, int teslimsure,
            int teslimsureuzman, int organizationid, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUpdateBank");
            cmd.Parameters.AddWithValue("@kod", kod);
            addStringParameter("@type", type, cmd);
            addStringParameter("@adi", adi, cmd);
            addStringParameter("@kisaadi", kisaadi, cmd);
            addStringParameter("@kodu", kodu, cmd);
            addStringParameter("@logo", logo, cmd);
            cmd.Parameters.AddWithValue("@teslimsure", teslimsure);
            cmd.Parameters.AddWithValue("@teslimsureuzman", teslimsureuzman);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateUpdateUser(int kod, string adi, string soyadi, string eposta, string tipi, int organizationid, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUpdateUser");
            cmd.Parameters.AddWithValue("@kod", kod);
            addStringParameter("@adi", adi, cmd);
            addStringParameter("@soyadi", soyadi, cmd);
            addStringParameter("@eposta", eposta, cmd);
            addStringParameter("@tipi", tipi, cmd);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);


            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<int> CreateUpdateReportByService(int kod, string sirket_rapor_no, int uzman, string il, string ilce, string mahalle, string yergostericiad,
            string musteri_unvani, string yergostericitel, string sirket_id, int banka, int etkin, int islem, int silindi, string actioninfo,
            string actiondescription, DateTime dateTime, string basvuru_rapor, string birimi, DateTime sysDate, DateTime uzmanSonTarih,
            int acil, int vip, int takbis)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUpdateReportByService");
            cmd.Parameters.AddWithValue("@kod", kod);
            addStringParameter("@sirket_rapor_no", sirket_rapor_no, cmd);
            cmd.Parameters.AddWithValue("@uzman", uzman);
            addStringParameter("@il", il, cmd);
            addStringParameter("@ilce", ilce, cmd);
            addStringParameter("@mahalle", mahalle, cmd);
            addStringParameter("@yergostericiad", yergostericiad, cmd);
            addStringParameter("@musteri_unvani", musteri_unvani, cmd);
            addStringParameter("@yergostericitel", yergostericitel, cmd);
            addStringParameter("@sirket_id", sirket_id, cmd);
            cmd.Parameters.AddWithValue("@banka", banka);
            cmd.Parameters.AddWithValue("@etkin", etkin);
            cmd.Parameters.AddWithValue("@islem", islem);
            cmd.Parameters.AddWithValue("@silindi", silindi);
            addStringParameter("@actioninfo", actioninfo, cmd);
            addStringParameter("@actiondescription", actiondescription, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            addStringParameter("@basvuru_rapor", basvuru_rapor, cmd);
            addStringParameter("@birimi", birimi, cmd);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);
            cmd.Parameters.AddWithValue("@SysDate", sysDate);
            cmd.Parameters.AddWithValue("@SysDateTick", sysDate.Ticks);
            cmd.Parameters.AddWithValue("@UzmanSonTarih", uzmanSonTarih);
            cmd.Parameters.AddWithValue("@UzmanSonTarihTick", uzmanSonTarih.Ticks);
            cmd.Parameters.AddWithValue("@Acil", acil);
            cmd.Parameters.AddWithValue("@Vip", vip);
            cmd.Parameters.AddWithValue("@Takbis", takbis);

            object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return Convert.ToInt32(obj);
            }

            return 0;
        }
        public async Task<int> CreateInvexSyncReportSubPartUserValue(int reportid, string subpartcode, int uzman, int invexkod,
            int tapuid, DateTime dateTime, int reportsysid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateInvexSyncReportSubPartUserValue");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            addStringParameter("@SubPartCode", subpartcode, cmd);
            cmd.Parameters.AddWithValue("@uzman", uzman);
            cmd.Parameters.AddWithValue("@InvexKod", invexkod);
            cmd.Parameters.AddWithValue("@TapuID", tapuid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@ReportSysID", reportsysid);

            object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return Convert.ToInt32(obj);
            }

            return 0;
        }
        public async Task<bool> CreateInvexSyncReportSubPartUserFieldValue(int reportsubpartuservalueid, string fieldinvexcode, string subpartcode, string fieldinvexvalue, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateInvexSyncReportSubPartUserFieldValue");
            cmd.Parameters.AddWithValue("@ReportSubPartUserValueID", reportsubpartuservalueid);
            addStringParameter("@fieldinvexcode", fieldinvexcode, cmd);
            addStringParameter("@SubPartCode", subpartcode, cmd);
            addStringParameter("@fieldinvexvalue", fieldinvexvalue, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateInvexSyncReport(int reportid, int state, int userid, int type, string basvuru, string birim, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateInvexSyncReport");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@Type", type);
            addStringParameter("@Basvuru", basvuru, cmd);
            addStringParameter("@Birim", birim, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetSyncReportList()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncReportList");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetSyncReportYazimList()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncReportYazimList");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetSyncReportByID(int syncreportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncReportByID");
            cmd.Parameters.AddWithValue("@SyncReportID", syncreportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> UpdateInvexSyncReportState(int reportid, int state, string note, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateInvexSyncReportState");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@State", state);
            addStringParameter("@Note", note, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateUser(string firstName, string lastName, string password, int userType, string email, DateTime dateTime, string userID)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateUser");
            addStringParameter("@UserFirstName", firstName, cmd);
            addStringParameter("@UserLastName", lastName, cmd);
            addStringParameter("@UserPassword", password, cmd);
            addStringParameter("@UserEmail", email, cmd);
            addStringParameter("@UserID", userID, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@UserType", userType);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateInvexSyncReportYazimState(int reportid, int state, string note, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateInvexSyncReportYazimState");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@State", state);
            addStringParameter("@Note", note, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> ChangeReportStateFromMobile(int userid, int reportid, string basvuru_rapor, string birimi, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("ChangeReportStateFromMobile");
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            addStringParameter("@basvuru_rapor", basvuru_rapor, cmd);
            addStringParameter("@birimi", birimi, cmd);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateUpdateInvexReportFile(int basvuru_kod, int kod, int kullanici, int silindi, int silindi_kullanici,
            string tipi, string dosyatipi, string adi, string orjadi, string tarih, DateTime dateTime, int size)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateUpdateInvexReportFile");
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@kullanici", kullanici);
            cmd.Parameters.AddWithValue("@silindi", silindi);
            cmd.Parameters.AddWithValue("@silindi_kullanici", silindi_kullanici);
            addStringParameter("@tipi", tipi, cmd);
            addStringParameter("@dosyatipi", dosyatipi, cmd);
            addStringParameter("@adi", adi, cmd);
            addStringParameter("@orjadi", orjadi, cmd);
            addStringParameter("@tarih", tarih, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);
            cmd.Parameters.AddWithValue("@size", size);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;

        }
        public async Task<bool> UpdateInvexReportFoto(int kod, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateInvexReportFoto");
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteInvexReportFoto(int kod, int basvuru_kod, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteInvexReportFoto");
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateInvexReportFile(int kod, string tipi, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateInvexReportFile");
            cmd.Parameters.AddWithValue("@kod", kod);
            addStringParameter("@tipi", tipi, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;

        }
        public async Task<bool> DeleteInvexReportFile(int kod, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteInvexReportFile");
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;

        }
        public async Task<int> GetReportIDByBasvuruKod(int kod)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportIDByBasvuruKod");
            cmd.Parameters.AddWithValue("@kod", kod);

            object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                return Convert.ToInt32(obj);
            }

            return 0;

        }
        /*@uzman INT,
	@basvuru_kod INT,
	@banka INT,
	@DateTime DATETIME,
	@DateTick BIGINT,
	@UserID INT OUTPUT*/
        public async Task<(int reportid, int userid, string reportcode, bool res)> CreateOrUpdateBasvuruRevizyon(string sirket_id, int kod, int basvuru_kod, DateTime revizyon_tarihi,
            DateTime dateTime, string revizyon_gerekcesi, string revizyon_gerekcesi_aciklama, string revizyon_sonucu, string revizyon_sonucu_aciklama,
            string not_sonuc_ekle, string sonuc, string sonuc_aciklama_banka, string sonuc_aciklama_sirket, string revizyon_notu)
        {
            bool res = false;
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateOrUpdateBasvuruRevizyon");
            addStringParameter("@sirket_id", sirket_id, cmd);
            cmd.Parameters.AddWithValue("@sirket_id", sirket_id);
            cmd.Parameters.AddWithValue("@kod", kod);
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@revizyon_tarihi", revizyon_tarihi);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);
            addStringParameter("@revizyon_gerekcesi", revizyon_gerekcesi, cmd);
            addStringParameter("@revizyon_gerekcesi_aciklama", revizyon_gerekcesi_aciklama, cmd);
            addStringParameter("@revizyon_sonucu", revizyon_sonucu, cmd);
            addStringParameter("@revizyon_sonucu_aciklama", revizyon_sonucu_aciklama, cmd);
            addStringParameter("@not_sonuc_ekle", not_sonuc_ekle, cmd);
            addStringParameter("@sonuc", sonuc, cmd);
            addStringParameter("@sonuc_aciklama_banka", sonuc_aciklama_banka, cmd);
            addStringParameter("@sonuc_aciklama_sirket", sonuc_aciklama_sirket, cmd);
            addStringParameter("@revizyon_notu", revizyon_notu, cmd);
            addOutPutParametr("@UserID", SqlDbType.Int, cmd);
            addOutPutParametr("@ReportID", SqlDbType.Int, cmd);
            addOutPutParametr("@ReportCode", SqlDbType.NVarChar, cmd);


            object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                res = true;
            }

            int userid = getOutputParamIntValue(cmd, "@UserID");
            int reportid = getOutputParamIntValue(cmd, "@ReportID");
            string reportcode = getOutputParamValue(cmd, "@AssignedUserID");

            /*if (cmd.Parameters["@UserID"].Value != null && cmd.Parameters["@UserID"].Value.ToString().Length > 0)
            {
                userid = Convert.ToInt32(cmd.Parameters["@UserID"].Value);
            }

            if (cmd.Parameters["@ReportID"].Value != null && cmd.Parameters["@ReportID"].Value.ToString().Length > 0)
            {
                reportid = Convert.ToInt32(cmd.Parameters["@ReportID"].Value);
            }

            if (cmd.Parameters["@ReportCode"].Value != null)
            {
                reportcode = cmd.Parameters["@ReportCode"].Value.ToString();
            }*/

            return (reportid, userid, reportcode, res);

        }
        public async Task<(int userid, string reportcode, bool res)> UpdateBirimInvexSync(int uzman, int basvuru_kod, int banka,
            DateTime dateTime)
        {
            bool res = false;
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateBirimInvexSync");

            cmd.Parameters.AddWithValue("@uzman", uzman);
            cmd.Parameters.AddWithValue("@basvuru_kod", basvuru_kod);
            cmd.Parameters.AddWithValue("@banka", banka);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@DateTick", dateTime.Ticks);
            addOutPutParametr("@UserID", SqlDbType.Int, cmd);
            addOutPutParametr("@ReportCode", SqlDbType.NVarChar, cmd);

            /*object obj = await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false);
            if (obj != null && Convert.ToInt32(obj) > 0)
            {
                res = true;
            }*/

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                res = true;
            }

            int userid = getOutputParamIntValue(cmd, "@UserID");
            string reportcode = getOutputParamValue(cmd, "@ReportCode");

            return (userid, reportcode, res);
        }
        public async Task<DataTable> GetTemplateSubPartFieldCodes(string subpartcodes)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateSubPartFieldCodes");
            cmd.Parameters.AddWithValue("@SubPartSysCodes", subpartcodes);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetReportSubPartSyncData(int reportid, string subpartcodes)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetReportSubPartSyncData");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@SubPartSysCodes", subpartcodes);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }

        public async Task<bool> UpdateInvexSubpartCode(int kod, int subpartuservalueid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateInvexSubpartCode");
            cmd.Parameters.AddWithValue("@Kod", kod);
            cmd.Parameters.AddWithValue("@ReportSubPartUserValueID", subpartuservalueid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetSyncReportFilesByReportID(int reportid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSyncReportFilesByReportID");
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> UpdateReportFileSyncState(int reportfileid, int syncstate, int sysid, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateReportFileSyncState");
            cmd.Parameters.AddWithValue("@ReportFileID", reportfileid);
            cmd.Parameters.AddWithValue("@SyncState", syncstate);
            cmd.Parameters.AddWithValue("@SysID", sysid);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }

        #endregion InvexSync
        #region PushLog
        public async Task<bool> InsertPushLog(int notifytpe, string text, string tagguid, int userid, string objguid, DateTime dateTime, int reportsysid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("InsertPushLog");
            cmd.Parameters.AddWithValue("@NotifyType", notifytpe);
            addStringParameter("@Text", text, cmd);
            addStringParameter("@TagGuid", tagguid, cmd);
            cmd.Parameters.AddWithValue("@UserID", userid);
            addStringParameter("@ObjGuid", objguid, cmd);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);
            cmd.Parameters.AddWithValue("@ReportSysID", reportsysid);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdatePushLogState(int logid, int state, DateTime dateTime)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdatePushLogState");
            cmd.Parameters.AddWithValue("@LogID", logid);
            cmd.Parameters.AddWithValue("@State", state);
            cmd.Parameters.AddWithValue("@DateTime", dateTime);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetPushLogs()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetPushLogs");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetPushLogByID(int pushlogid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetPushLogByID");
            cmd.Parameters.AddWithValue("@PushLogID", pushlogid);

            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        #endregion PushLog
        #region System
        public async Task<bool> CreateErrorLogs(string parameters, DateTime date, string? sessionkey, string controllername,
            string actionname, string error, string innerexception, int userid, int reportid, int organizationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateErrorLog_New");
            addStringParameter("@Parameters", parameters, cmd);
            cmd.Parameters.AddWithValue("@DateTime", date);
            addStringParameter("@SessionKey", sessionkey, cmd);
            addStringParameter("@ControllerName", controllername, cmd);
            addStringParameter("@ActionName", actionname, cmd);
            addStringParameter("@ErrorLog", error, cmd);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@ReportID", reportid);
            cmd.Parameters.AddWithValue("@OrganizationID", organizationid);
            addStringParameter("@InnerExcetion", innerexception, cmd);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<int> GetAppVersion(int platformid)
        {

            SqlCommand cmd = DALAsync.CommandForProcedure("GetAppVersion");
            cmd.Parameters.AddWithValue("@PlatFormID", platformid);

            try
            {
                object obj = await db.ExecuteScalarAsync(cmd).ConfigureAwait(false);
                if (obj != null && Convert.ToInt32(obj) > 0)
                {
                    return Convert.ToInt32(obj);
                }
            }
            catch
            {

            }

            return 0;
        }
        public void addStringParameter(string key, string? value, SqlCommand cmd)
        {
            if (value == null)
            {
                cmd.Parameters.AddWithValue(key, DBNull.Value);
            }
            else
            {
                SqlParameter parameter = new SqlParameter(key, SqlDbType.NVarChar);
                parameter.Value = value;
                parameter.Size = -1;
                cmd.Parameters.Add(parameter);
                //cmd.Parameters.AddWithValue(key, value);
            }
        }
        public void addOutPutParametr(string key, SqlDbType type, SqlCommand cmd)
        {
            cmd.Parameters.Add(key, type);
            cmd.Parameters[key].Direction = ParameterDirection.Output;
            if (type == SqlDbType.NVarChar)
            {
                cmd.Parameters[key].Size = -1;
            }
        }
        int getOutputParamIntValue(SqlCommand cmd, string param)
        {
            try
            {
                if (cmd.Parameters[param].Value != null && cmd.Parameters[param].Value.ToString().Length > 0)
                {
                    int val = Convert.ToInt32(cmd.Parameters[param].Value);
                    return val;
                }
            }
            catch
            {

            }

            return 0;
        }
        string getOutputParamValue(SqlCommand cmd, string param)
        {
            try
            {
                if (cmd.Parameters[param].Value != null)
                {
                    string val = cmd.Parameters[param].Value.ToString();
                    return val;
                }
            }
            catch
            {

            }

            return "";
        }
        #endregion System

        #region SablonCreate
        #region Templates
        public async Task<DataTable> GetTemplates_Web()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplates_Web");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetTemplateList_Web()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateList_Web");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> CreateBankTemplate_Web(int bankid, int templateid, int userid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateBankTemplate_Web");
            cmd.Parameters.AddWithValue("@BankID", bankid);
            cmd.Parameters.AddWithValue("@TemplateID", templateid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> DeleteBankTemplateByID_Web(int banktemplateid, int userid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteBankTemplateByID_Web");
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@UserID", userid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<DataTable> GetOrganizationBanks(int organiationid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetOrganizationBanks");
            cmd.Parameters.AddWithValue("@OrganizationID", organiationid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }

        #endregion Templates
        #region TemplateParts
        public async Task<DataTable> GetTemplateParts_Web(int banktemplateid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateParts_Web");
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetTemplatePartByID_Web(int templatepartid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplatePartByID_Web");
            cmd.Parameters.AddWithValue("@TemplatePartID", templatepartid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> DeleteTemplatePart_Web(int templatepartid, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteTemplatePart_Web");
            cmd.Parameters.AddWithValue("@TemplatePartID", templatepartid);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateTemplatePart_Web(string templatepartname, int templatepartindex, int banktemplateid, string templatepartsyscode, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateTemplatePart_Web");
            cmd.Parameters.AddWithValue("@TemplatePartName", templatepartname);
            cmd.Parameters.AddWithValue("@TemplatePartIndex", templatepartindex);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@TemplatePartSysCode", templatepartsyscode);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateTemplatePart_Web(string templatepartname, int templatepartindex, int templatepartid, string templatepartsyscode, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateTemplatePart_Web");
            cmd.Parameters.AddWithValue("@TemplatePartName", templatepartname);
            cmd.Parameters.AddWithValue("@TemplatePartIndex", templatepartindex);
            cmd.Parameters.AddWithValue("@TemplatePartID", templatepartid);
            cmd.Parameters.AddWithValue("@TemplatePartSysCode", templatepartsyscode);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }

        #endregion TemplateParts
        #region TemplateSubParts
        public async Task<DataTable> GetTemplateSubParts_Web(int templatepartid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateSubParts_Web");
            cmd.Parameters.AddWithValue("@TemplatePartID", templatepartid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetTemplateSubPartByID_Web(int templatesubpartid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateSubPartByID_Web");
            cmd.Parameters.AddWithValue("@TemplateSubPartID", templatesubpartid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> DeleteTemplateSubPart_Web(int templatesubpartid, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteTemplateSubPart_Web");
            cmd.Parameters.AddWithValue("@TemplateSubPartID", templatesubpartid);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateTemplateSubPart_Web(string name, int index, int partid, bool hasmultiple, string code, string altlik, bool istapubased, string syscode, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateTemplateSubPart_Web");
            cmd.Parameters.AddWithValue("@TemplateSubPartName", name);
            cmd.Parameters.AddWithValue("@TemplateSubPartIndex", index);
            cmd.Parameters.AddWithValue("@TemplatePartID", partid);
            cmd.Parameters.AddWithValue("@TemplatePartHasMultiple", hasmultiple);
            addStringParameter("@TemplateSubPartCode", code, cmd);
            addStringParameter("@TemplateSubPartAltlik", altlik, cmd);
            cmd.Parameters.AddWithValue("@TemplateSubPartIsTapuBased", istapubased);
            addStringParameter("@TemplateSubPartSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateTemplateSubPart_Web(int subpartid, string name, int index, bool hasmultiple, string code, string altlik, bool istapubased, string syscode, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateTemplateSubPart_Web");
            cmd.Parameters.AddWithValue("@TemplateSubPartID", subpartid);
            cmd.Parameters.AddWithValue("@TemplateSubPartName", name);
            cmd.Parameters.AddWithValue("@TemplateSubPartIndex", index);
            cmd.Parameters.AddWithValue("@TemplatePartHasMultiple", hasmultiple);
            addStringParameter("@TemplateSubPartCode", code, cmd);
            addStringParameter("@TemplateSubPartAltlik", altlik, cmd);
            cmd.Parameters.AddWithValue("@TemplateSubPartIsTapuBased", istapubased);
            addStringParameter("@TemplateSubPartSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }

        #endregion TemplateSubParts
        #region TemplateSections
        public async Task<DataTable> GetTemplateSections_Web(int templatesubpartid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateSection_Web");
            cmd.Parameters.AddWithValue("@TemplateSubPartID", templatesubpartid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetTemplateSectionByID_Web(int templatesectionid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetTemplateSectionByID_Web");
            cmd.Parameters.AddWithValue("@TemplateSectionID", templatesectionid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> DeleteTemplateSection_Web(int templatesectionid, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteTemplateSection_Web");
            cmd.Parameters.AddWithValue("@TemplateSectionID", templatesectionid);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateTemplateSection_Web(string name, int index, int subpartid, bool hastitle, string visibilityids, string bankvibilityids, string syscode, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateTemplateSection_Web");
            cmd.Parameters.AddWithValue("@TemplateSectionName", name);
            cmd.Parameters.AddWithValue("@TemplateSectionIndex", index);
            cmd.Parameters.AddWithValue("@TemplateSubPartID", subpartid);
            cmd.Parameters.AddWithValue("@TemplatePartHasTitle", hastitle);
            addStringParameter("@TemplateSectionVisibilityIds", visibilityids, cmd);
            addStringParameter("@TemplateSectionBankVisibilityIds", bankvibilityids, cmd);
            addStringParameter("@TemplateSectionSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateTemplateSection_Web(int sectionid, string name, int index, bool hastitle, string visibilityids, string bankvibilityids, string syscode, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateTemplateSection_Web");
            cmd.Parameters.AddWithValue("@TemplateSectionID", sectionid);
            cmd.Parameters.AddWithValue("@TemplateSectionName", name);
            cmd.Parameters.AddWithValue("@TemplateSectionIndex", index);
            cmd.Parameters.AddWithValue("@TemplatePartHasTitle", hastitle);
            addStringParameter("@TemplateSectionVisibilityIds", visibilityids, cmd);
            addStringParameter("@TemplateSectionBankVisibilityIds", bankvibilityids, cmd);
            addStringParameter("@TemplateSectionSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion TemplateSections
        #region SectionFields
        public async Task<DataTable> GetSectionFields_Web(int templatesectionid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSectionFields_Web");
            cmd.Parameters.AddWithValue("@TemplateSectionID", templatesectionid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetSectionFieldByID_Web(int sectionfieldid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetSectionFieldByID_Web");
            cmd.Parameters.AddWithValue("@SectionFieldID", sectionfieldid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> DeleteSectionField_Web(int sectionfieldid, int banktemplateid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteSectionField_Web");
            cmd.Parameters.AddWithValue("@SectionFieldID", sectionfieldid);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateSectionField_Web(int templatesectionid, int fieldid, int index, bool isrequired,
                                            string minvalue, string maxvalue, string visibilityids, string bankvisibilityids,
                                            int banktemplateid, long datetick, int isbankfield)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateSectionField_Web");
            cmd.Parameters.AddWithValue("@TemplateSectionID", templatesectionid);
            cmd.Parameters.AddWithValue("@FieldID", fieldid);
            cmd.Parameters.AddWithValue("@SectionFieldIndex", index);
            cmd.Parameters.AddWithValue("@SectionFieldIsRequired", isrequired);
            addStringParameter("@SectionFieldMinValue", minvalue, cmd);
            addStringParameter("@SectionFieldMaxValue", maxvalue, cmd);
            addStringParameter("@SectionFieldVisibilityIds", visibilityids, cmd);
            addStringParameter("@SectionFieldBankVisibilityIds", bankvisibilityids, cmd);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);
            cmd.Parameters.AddWithValue("@IsBankField", isbankfield);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateSectionField_Web(int sectionfieldid, int index, bool isrequired,
                                            string minvalue, string maxvalue, string visibilityids, string bankvisibilityids,
                                            int banktemplateid, long datetick, int isbankfield)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateSectionField_Web");
            cmd.Parameters.AddWithValue("@SectionFieldID", sectionfieldid);
            cmd.Parameters.AddWithValue("@SectionFieldIndex", index);
            cmd.Parameters.AddWithValue("@SectionFieldIsRequired", isrequired);
            addStringParameter("@SectionFieldMinValue", minvalue, cmd);
            addStringParameter("@SectionFieldMaxValue", maxvalue, cmd);
            addStringParameter("@SectionFieldVisibilityIds", visibilityids, cmd);
            addStringParameter("@SectionFieldBankVisibilityIds", bankvisibilityids, cmd);
            cmd.Parameters.AddWithValue("@BankTemplateID", banktemplateid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);
            cmd.Parameters.AddWithValue("@IsBankField", isbankfield);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion SectionFields
        #region Fields
        public async Task<DataTable> GetFieldTypes_Web()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetFieldTypes_Web");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetFields_Web()
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetFields_Web");
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetFieldByID_Web(int fieldid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetFieldByID_Web");
            cmd.Parameters.AddWithValue("@FieldID", fieldid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> DeleteField_Web(int fieldid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteField_Web");
            cmd.Parameters.AddWithValue("@FieldID", fieldid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateField_Web(string name, string code, int type, string subinfo, string altlikformat, string syscode, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateField_Web");
            addStringParameter("@FieldName", name, cmd);
            addStringParameter("@FieldCode", code, cmd);
            cmd.Parameters.AddWithValue("@FieldType", type);
            addStringParameter("@FieldSubInfo", subinfo, cmd);
            addStringParameter("@FieldAltlikFormat", altlikformat, cmd);
            addStringParameter("@FieldSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateField_Web(int fieldid, string name, string code, int type, string subinfo, string altlikformat, string syscode, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateField_Web");
            cmd.Parameters.AddWithValue("@FieldID", fieldid);
            addStringParameter("@FieldName", name, cmd);
            addStringParameter("@FieldCode", code, cmd);
            cmd.Parameters.AddWithValue("@FieldType", type);
            addStringParameter("@FieldSubInfo", subinfo, cmd);
            addStringParameter("@FieldAltlikFormat", altlikformat, cmd);
            addStringParameter("@FieldSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion Fields
        #region FieldParameters
        public async Task<DataTable> GetFieldParameters_Web(int fieldid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetFieldParameters_Web");
            cmd.Parameters.AddWithValue("@FieldID", fieldid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<DataTable> GetFieldParameterByID_Web(int fieldparameterid)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("GetFieldParameterByID_Web");
            cmd.Parameters.AddWithValue("@FieldParameterID", fieldparameterid);
            return await db.GetDataTable(cmd).ConfigureAwait(false);
        }
        public async Task<bool> DeleteFieldParameter_Web(int fieldparameterid, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("DeleteFieldParameter_Web");
            cmd.Parameters.AddWithValue("@FieldParameterID", fieldparameterid);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> CreateFieldParameter_Web(int fieldid, string text, string value, bool isdefault, int index, string syscode, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("CreateFieldParameter_Web");
            cmd.Parameters.AddWithValue("@FieldID", fieldid);
            addStringParameter("@FieldParameterText", text, cmd);
            addStringParameter("@FieldParameterValue", value, cmd);
            cmd.Parameters.AddWithValue("@FieldParameterIsDefault", isdefault);
            cmd.Parameters.AddWithValue("@FieldParameterIndex", index);
            addStringParameter("@FieldParameterSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        public async Task<bool> UpdateFieldParameter_Web(int fieldparameterid, string text, string value, bool isdefault, int index, string syscode, long datetick)
        {
            SqlCommand cmd = DALAsync.CommandForProcedure("UpdateFieldParameter_Web");
            cmd.Parameters.AddWithValue("@FieldParameterID", fieldparameterid);
            addStringParameter("@FieldParameterText", text, cmd);
            addStringParameter("@FieldParameterValue", value, cmd);
            cmd.Parameters.AddWithValue("@FieldParameterIsDefault", isdefault);
            cmd.Parameters.AddWithValue("@FieldParameterIndex", index);
            addStringParameter("@FieldParameterSysCode", syscode, cmd);
            cmd.Parameters.AddWithValue("@DateTick", datetick);

            if (await db.ExecuteNonQueryAsync(cmd).ConfigureAwait(false) > 0)
            {
                return true;
            }

            return false;
        }
        #endregion FieldParameters
        #endregion SablonCreate
    }
}
