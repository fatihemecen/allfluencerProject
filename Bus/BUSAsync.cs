using BlazorApp.Shared;
using Data;
using Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Util;
using static System.Net.Mime.MediaTypeNames;

namespace Bus
{
    public class BUSAsync
    {


        public DATAsync db;
        public TB tb;
        public BUSAsync(DATAsync database)
        {
            db = database;
            tb = new TB();
        }


        public async Task<List<coUser>> GetUserList()
        {
            DataTable dt = await db.GetUserList().ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coUser> us = coBaseObject.CreateObjectListFromDataTable<coUser>(dt);
                return us;
            }
            else
            {
                return null;
            }
        }

        public async Task<string> CreateUser(string userName,string userFirstName, string userLastName, string userEmail, string userPassword, int userRole)
        {

            return await db.CreateUser(userName,userFirstName, userLastName,userEmail, userPassword, userRole).ConfigureAwait(false);

        }
        public async Task<bool> CheckUser(string userEmail,string userName)
        {

            return await db.CheckUser(userEmail, userName);

        }
        public async Task<string> LoginUser(string userEmail, string userPassword)
        {

            return await db.LoginUser(userEmail, userPassword).ConfigureAwait(false);

        }



        /*#region UserAndLogin

        public async Task<coOrganization> GetOrganizationByID(int organizationID)
        {
            DataTable dt = await db.GetOrganizationByID(organizationID).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                coOrganization org = coBaseObject.CreateObjectFromDataTable<coOrganization>(dt);
                if (org != null)
                {
                    org.OrganizationName = Base32.EncodeAsBase32String(org.OrganizationName);
                }

                return org;
            }
            else
            {
                return null;
            }
        }
        public async Task<coUser> GetUserByEmailAndPassword(string useremail, string password, string organizationcode)
        {
            DataTable dt = await db.GetUserByEmailAndPassword(useremail, password, organizationcode).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                coUser us = coBaseObject.CreateObjectFromDataTable<coUser>(dt);
                if (us != null)
                {
                    us.UserFirstName = Base32.EncodeAsBase32String(us.UserFirstName);
                    us.UserLastName = Base32.EncodeAsBase32String(us.UserLastName);
                    us.UserEmail = Base32.EncodeAsBase32String(us.UserEmail);
                }

                return us;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coUser>> GetUserByUserID(int UserID)
        {
            DataTable dt = await db.GetUserByUserID(UserID).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coUser> us = coBaseObject.CreateObjectListFromDataTable<coUser>(dt);
                return us;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coUser>> GetUserByText(string text, int skipIndex, int takeCount, int userType, int invexType)
        {
            DataTable dt = await db.GetUserByText(text, skipIndex, takeCount, userType, invexType).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coUser> us = coBaseObject.CreateObjectListFromDataTable<coUser>(dt);
                return us;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coExpert>> GetExByOpID(int userType, string userID)
        {
            DataTable dt = await db.GetExByOpID(userType, userID).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coExpert> us = coBaseObject.CreateObjectListFromDataTable<coExpert>(dt);
                return us;
            }
            else
            {
                return null;
            }
        }
        public async Task<int> ChangeUserPassword(int userid, string oldpassword, string newpassword)
        {
            return await db.ChangeUserPassword(userid, oldpassword, newpassword).ConfigureAwait(false);
        }
        public async Task<coUser> GetUserByEposta(string useremail, string orgcode)
        {
            DataTable dt = await db.GetUserByEposta(useremail, orgcode).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                coUser us = coBaseObject.CreateObjectFromDataTable<coUser>(dt);
                if (us != null)
                {
                    us.UserFirstName = Base32.EncodeAsBase32String(us.UserFirstName);
                    us.UserLastName = Base32.EncodeAsBase32String(us.UserLastName);
                    us.UserEmail = Base32.EncodeAsBase32String(us.UserEmail);
                }

                return us;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateUser(string userFirstName, string userLastName, string userPassword, int userType, string userEmail, DateTime dateTime, string userGuid, int organizationID)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.CreateUser(userFirstName, userLastName, userPassword, userType, userEmail, dateTime, userGuid,
                organizationID).ConfigureAwait(false);

        }
        public async Task<bool> AddExpert(string operatorID, string userID)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.AddExpert(operatorID, userID).ConfigureAwait(false);

        }
        public async Task<bool> DeleteExpert(string userID)
        {
            return await db.DeleteExpert(userID).ConfigureAwait(false);
        }
        public async Task<bool> DeleteUserByGuid(string userID)
        {
            return await db.DeleteUserByGuid(userID).ConfigureAwait(false);
        }
        public async Task<bool> UpdateUserDeviceInfo(string deviceip, string devicename, string invextoken, int userid)
        {
            return await db.UpdateUserDeviceInfo(deviceip, devicename, invextoken, userid).ConfigureAwait(false);
        }
        #endregion UserAndLogin
        #region Report
        public async Task<List<coReportFillState>> GetReportFillState(DateTime prevDate, DateTime nextDate)
        {
            DataTable dt = await db.GetReportFillState(prevDate, nextDate).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coReportFillState> list = coBaseObject.CreateObjectListFromDataTable<coReportFillState>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReportFillStateDetails>> GetReportFillStateDetails(int reportid)
        {
            DataTable dt = await db.GetReportFillStateDetails(reportid).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coReportFillStateDetails> list = coBaseObject.CreateObjectListFromDataTable<coReportFillStateDetails>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReport>> GetReportByCodeAndUserID(string code, int userid, int userrole, int organizationid)
        {
            DataTable dt = await db.GetReportByCodeAndUserID(code, userid, userrole, organizationid).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coReport> list = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReportFile>> GetReportFilesByReportID(int reportid, int typeid)
        {
            DataTable dt = await db.GetReportFilesByReportID(reportid, typeid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReportFile> list = coBaseObject.CreateObjectListFromDataTable<coReportFile>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReport>> GetUserReportsByUserID(int userid, int skipindex, int takecount, int organizationid,
            int usertype, int randevu, int takbis, int late)
        {
            DataTable dt = await db.GetUserReportsByUserID(userid, skipindex, takecount, organizationid, usertype, randevu, takbis, late).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReport> list = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReport>> GetUserReports(int userid, int skipindex, int takecount)
        {
            DataTable dt = await db.GetUserReports(userid, skipindex, takecount).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReport> list = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coReport> GetReportByReportID(int userid, int usertype, int reportid, int organizationid)
        {
            DataTable dt = await db.GetReportByReportID(userid, usertype, reportid, organizationid).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                coReport rp = coBaseObject.CreateObjectFromDataTable<coReport>(dt);

                return rp;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReport>> GetLastTenReport(int userType, string userID)
        {
            DataTable dt = await db.GetLastTenReport(userType, userID).ConfigureAwait(false);
            if (dt != null && dt.Rows.Count > 0)
            {
                List<coReport> rp = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);

                return rp;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateReportFileNew(int userid, int reportid, int filetypeid, string fileguid,
                DateTime createddate, string createddatest, int housingpartindex, int partcountindex, int organizationid,
                int subtype, int iskapak, int useinreport, string invextype, int sourcetype, int sysid)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.CreateReportFileNew(userid, reportid, filetypeid, fileguid, datetime, createddate, createddatest,
                housingpartindex, partcountindex, organizationid, subtype, iskapak, useinreport, invextype, sourcetype, sysid).ConfigureAwait(false);

        }
        public async Task<bool> DeleteReportFile(int reportfileid, int userid, int organizationid)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.DeleteReportFile(reportfileid, userid, datetime, organizationid);
        }
        public async Task<bool> DeleteReportFileBySysID(int reportfileid, int userid, int organizationid)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.DeleteReportFileBySysID(reportfileid, userid, datetime, organizationid);
        }
        public async Task<bool> UpdateReport(int userid, int reportid, string location, int organizationid, int photoproportion)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.UpdateReport(userid, reportid, datetime, location, organizationid, photoproportion).ConfigureAwait(false);
        }
        public async Task<bool> UpdateUser(string firstName, string lastName, string password, int userType, string email, DateTime dateTime, string userID)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.UpdateUser(firstName, lastName, password, userType, email, datetime, userID).ConfigureAwait(false);
        }
        /*public async Task<coTemplate> GetReportFormData(int userid, int banktemplateid, int reportid)
        {
            DataTable dt = await db.GetReportFormData(userid, banktemplateid, reportid).ConfigureAwait(false);

            if (dt != null && dt.Rows.Count > 0)
            {
                coTemplate template = coBaseObject.CreateObjectFromDataTable<coTemplate>(dt);
                template.TemplateParts = new List<coTemplatePart>();

                List<coTemplatePart> templateParts = coBaseObject.CreateObjectListFromDataTable<coTemplatePart>(dt);
                List<coTemplateSubPart> templateSubParts = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPart>(dt);
                List<coTemplateSection> templateSections = coBaseObject.CreateObjectListFromDataTable<coTemplateSection>(dt);
                List<coField> fields = coBaseObject.CreateObjectListFromDataTable<coField>(dt);
                List<coFieldParameter> fieldParameters = coBaseObject.CreateObjectListFromDataTable<coFieldParameter>(dt);

                foreach (coTemplatePart templatePart in templateParts)
                {
                    if (!template.TemplateParts.Any(x => x.TemplatePartID == templatePart.TemplatePartID))
                    {
                        templatePart.TemplateSubParts = new List<coTemplateSubPart>();
                        foreach (coTemplateSubPart templateSubPart in templateSubParts)
                        {
                            if (templateSubPart.TemplatePartID == templatePart.TemplatePartID && !templatePart.TemplateSubParts.Any(x => x.TemplateSubPartID == templateSubPart.TemplateSubPartID && x.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID))
                            {
                                templateSubPart.TemplateSections = new List<coTemplateSection>();

                                foreach (coTemplateSection templateSection in templateSections)
                                {
                                    if (templateSection.TemplateSubPartID == templateSubPart.TemplateSubPartID && templateSection.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSubPart.TemplateSections.Any(x => x.TemplateSectionID == templateSection.TemplateSectionID))
                                    {
                                        templateSection.Fields = new List<coField>();

                                        foreach (coField field in fields)
                                        {
                                            if (field.TemplateSectionID == templateSection.TemplateSectionID && field.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSection.Fields.Any(x => x.SectionFieldID == field.SectionFieldID))
                                            {
                                                field.FieldParameters = new List<coFieldParameter>();

                                                foreach (coFieldParameter fieldParameter in fieldParameters)
                                                {
                                                    if (fieldParameter.FieldParameterID != 0 && fieldParameter.FieldID == field.FieldID && !field.FieldParameters.Any(x => x.FieldParameterID == fieldParameter.FieldParameterID))
                                                    {
                                                        field.FieldParameters.Add(fieldParameter);
                                                    }
                                                }

                                                templateSection.Fields.Add(field);
                                            }
                                        }

                                        templateSubPart.TemplateSections.Add(templateSection);
                                    }
                                }

                                templatePart.TemplateSubParts.Add(templateSubPart);
                            }
                        }

                        template.TemplateParts.Add(templatePart);
                    }
                }

                return template;
            }
            else
            {
                return null;
            }
        }*/
        /*
        public async Task<int> GetReportFormDataVersionNo(int reportid, int organizationid)
        {
            return await db.GetReportFormDataVersionNo(reportid, organizationid);
        }
        public async Task<coTemplate> GetTemplateData(int banktemplateid, long datetick)
        {
            DataTable dt = await db.GetTemplateData(banktemplateid, datetick).ConfigureAwait(false);

            if (dt != null && dt.Rows.Count > 0)
            {
                coTemplate template = coBaseObject.CreateObjectFromDataTable<coTemplate>(dt);
                template.TemplateParts = new List<coTemplatePart>();

                List<coTemplatePart> templateParts = coBaseObject.CreateObjectListFromDataTable<coTemplatePart>(dt);
                List<coTemplateSubPart> templateSubParts = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPart>(dt);
                List<coTemplateSection> templateSections = coBaseObject.CreateObjectListFromDataTable<coTemplateSection>(dt);
                List<coField> fields = coBaseObject.CreateObjectListFromDataTable<coField>(dt);
                List<coFieldParameter> fieldParameters = coBaseObject.CreateObjectListFromDataTable<coFieldParameter>(dt);

                foreach (coTemplatePart templatePart in templateParts)
                {
                    if (!template.TemplateParts.Any(x => x.TemplatePartID == templatePart.TemplatePartID))
                    {
                        templatePart.TemplateSubParts = new List<coTemplateSubPart>();
                        foreach (coTemplateSubPart templateSubPart in templateSubParts)
                        {
                            if (templateSubPart.TemplatePartID == templatePart.TemplatePartID && !templatePart.TemplateSubParts.Any(x => x.TemplateSubPartID == templateSubPart.TemplateSubPartID && x.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID))
                            {
                                templateSubPart.TemplateSections = new List<coTemplateSection>();

                                foreach (coTemplateSection templateSection in templateSections)
                                {
                                    if (templateSection.TemplateSubPartID == templateSubPart.TemplateSubPartID && templateSection.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSubPart.TemplateSections.Any(x => x.TemplateSectionID == templateSection.TemplateSectionID))
                                    {
                                        templateSection.Fields = new List<coField>();

                                        foreach (coField field in fields)
                                        {
                                            if (field.TemplateSectionID == templateSection.TemplateSectionID && field.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSection.Fields.Any(x => x.SectionFieldID == field.SectionFieldID))
                                            {
                                                field.FieldParameters = new List<coFieldParameter>();

                                                foreach (coFieldParameter fieldParameter in fieldParameters)
                                                {
                                                    if (fieldParameter.FieldParameterID != 0 && fieldParameter.FieldID == field.FieldID && !field.FieldParameters.Any(x => x.FieldParameterID == fieldParameter.FieldParameterID))
                                                    {
                                                        field.FieldParameters.Add(fieldParameter);
                                                    }
                                                }

                                                templateSection.Fields.Add(field);
                                            }
                                        }

                                        templateSubPart.TemplateSections.Add(templateSection);
                                    }
                                }

                                templatePart.TemplateSubParts.Add(templateSubPart);
                            }
                        }

                        template.TemplateParts.Add(templatePart);
                    }
                }

                return template;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTemplateSDO> GetReportFormData_New(int banktemplateid, int reportid)
        {
            DataTable dt = await db.GetReportFormData_New(banktemplateid, reportid).ConfigureAwait(false);

            if (dt != null && dt.Rows.Count > 0)
            {
                coTemplateSDO template = coBaseObject.CreateObjectFromDataTable<coTemplateSDO>(dt);
                template.TemplateParts = new List<coTemplatePartSDO>();

                List<coTemplatePartSDO> templateParts = coBaseObject.CreateObjectListFromDataTable<coTemplatePartSDO>(dt);
                List<coTemplateSubPartSDO> templateSubParts = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPartSDO>(dt);
                List<coTemplateSectionSDO> templateSections = coBaseObject.CreateObjectListFromDataTable<coTemplateSectionSDO>(dt);
                List<coFieldSDO> fields = coBaseObject.CreateObjectListFromDataTable<coFieldSDO>(dt);

                foreach (coTemplatePartSDO templatePart in templateParts)
                {
                    if (!template.TemplateParts.Any(x => x.TemplatePartID == templatePart.TemplatePartID))
                    {
                        templatePart.TemplateSubParts = new List<coTemplateSubPartSDO>();
                        foreach (coTemplateSubPartSDO templateSubPart in templateSubParts)
                        {
                            if (templateSubPart.TemplatePartID == templatePart.TemplatePartID && !templatePart.TemplateSubParts.Any(x => x.TemplateSubPartID == templateSubPart.TemplateSubPartID && x.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID))
                            {
                                templateSubPart.TemplateSections = new List<coTemplateSectionSDO>();

                                foreach (coTemplateSectionSDO templateSection in templateSections)
                                {
                                    if (templateSection.TemplateSubPartID == templateSubPart.TemplateSubPartID && templateSection.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSubPart.TemplateSections.Any(x => x.TemplateSectionID == templateSection.TemplateSectionID))
                                    {
                                        templateSection.Fields = new List<coFieldSDO>();

                                        foreach (coFieldSDO field in fields)
                                        {
                                            if (field.TemplateSectionID == templateSection.TemplateSectionID && field.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSection.Fields.Any(x => x.SectionFieldID == field.SectionFieldID))
                                            {
                                                templateSection.Fields.Add(field);
                                            }
                                        }

                                        templateSubPart.TemplateSections.Add(templateSection);
                                    }
                                }

                                templatePart.TemplateSubParts.Add(templateSubPart);
                            }
                        }

                        template.TemplateParts.Add(templatePart);
                    }
                }

                return template;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTemplate> GetReportFormDataWithOriginalObjects(int banktemplateid, int reportid)
        {
            DataTable dt = await db.GetReportFormData_New(banktemplateid, reportid).ConfigureAwait(false);

            if (dt != null && dt.Rows.Count > 0)
            {
                coTemplate template = coBaseObject.CreateObjectFromDataTable<coTemplate>(dt);
                template.TemplateParts = new List<coTemplatePart>();

                List<coTemplatePart> templateParts = coBaseObject.CreateObjectListFromDataTable<coTemplatePart>(dt);
                List<coTemplateSubPart> templateSubParts = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPart>(dt);
                List<coTemplateSection> templateSections = coBaseObject.CreateObjectListFromDataTable<coTemplateSection>(dt);
                List<coField> fields = coBaseObject.CreateObjectListFromDataTable<coField>(dt);

                foreach (coTemplatePart templatePart in templateParts)
                {
                    if (!template.TemplateParts.Any(x => x.TemplatePartID == templatePart.TemplatePartID))
                    {
                        templatePart.TemplateSubParts = new List<coTemplateSubPart>();
                        foreach (coTemplateSubPart templateSubPart in templateSubParts)
                        {
                            if (templateSubPart.TemplatePartID == templatePart.TemplatePartID && !templatePart.TemplateSubParts.Any(x => x.TemplateSubPartID == templateSubPart.TemplateSubPartID && x.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID))
                            {
                                templateSubPart.TemplateSections = new List<coTemplateSection>();

                                foreach (coTemplateSection templateSection in templateSections)
                                {
                                    if (templateSection.TemplateSubPartID == templateSubPart.TemplateSubPartID && templateSection.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSubPart.TemplateSections.Any(x => x.TemplateSectionID == templateSection.TemplateSectionID))
                                    {
                                        templateSection.Fields = new List<coField>();

                                        foreach (coField field in fields)
                                        {
                                            if (field.TemplateSectionID == templateSection.TemplateSectionID && field.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSection.Fields.Any(x => x.SectionFieldID == field.SectionFieldID))
                                            {
                                                templateSection.Fields.Add(field);
                                            }
                                        }

                                        templateSubPart.TemplateSections.Add(templateSection);
                                    }
                                }

                                templatePart.TemplateSubParts.Add(templateSubPart);
                            }
                        }

                        template.TemplateParts.Add(templatePart);
                    }
                }

                return template;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTemplate> GetReportDataBySysInfo(int reportid, string syscode, string kod, string tapukod)
        {
            DataTable dt = await db.GetReportDataBySysInfo(reportid, syscode, kod, tapukod).ConfigureAwait(false);

            if (dt != null && dt.Rows.Count > 0)
            {
                coTemplate template = coBaseObject.CreateObjectFromDataTable<coTemplate>(dt);
                template.TemplateParts = new List<coTemplatePart>();

                List<coTemplatePart> templateParts = coBaseObject.CreateObjectListFromDataTable<coTemplatePart>(dt);
                List<coTemplateSubPart> templateSubParts = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPart>(dt);
                List<coTemplateSection> templateSections = coBaseObject.CreateObjectListFromDataTable<coTemplateSection>(dt);
                List<coField> fields = coBaseObject.CreateObjectListFromDataTable<coField>(dt);

                foreach (coTemplatePart templatePart in templateParts)
                {
                    if (!template.TemplateParts.Any(x => x.TemplatePartID == templatePart.TemplatePartID))
                    {
                        templatePart.TemplateSubParts = new List<coTemplateSubPart>();
                        foreach (coTemplateSubPart templateSubPart in templateSubParts)
                        {
                            if (templateSubPart.TemplatePartID == templatePart.TemplatePartID && !templatePart.TemplateSubParts.Any(x => x.TemplateSubPartID == templateSubPart.TemplateSubPartID && x.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID))
                            {
                                templateSubPart.TemplateSections = new List<coTemplateSection>();

                                foreach (coTemplateSection templateSection in templateSections)
                                {
                                    if (templateSection.TemplateSubPartID == templateSubPart.TemplateSubPartID && templateSection.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSubPart.TemplateSections.Any(x => x.TemplateSectionID == templateSection.TemplateSectionID))
                                    {
                                        templateSection.Fields = new List<coField>();

                                        foreach (coField field in fields)
                                        {
                                            if (field.TemplateSectionID == templateSection.TemplateSectionID && field.ReportSubPartUserValueID == templateSubPart.ReportSubPartUserValueID && !templateSection.Fields.Any(x => x.SectionFieldID == field.SectionFieldID))
                                            {
                                                templateSection.Fields.Add(field);
                                            }
                                        }

                                        templateSubPart.TemplateSections.Add(templateSection);
                                    }
                                }

                                templatePart.TemplateSubParts.Add(templateSubPart);
                            }
                        }

                        template.TemplateParts.Add(templatePart);
                    }
                }

                return template;
            }
            else
            {
                return null;
            }
        }
        public async Task<(int res, int uzmanid, int operasyonuserid)> UpdateReportInvexState(string sirket_id, int basvuru_id, int etkin, int islem, int silindi, int ReportState,
            string actioninfo, string actionDescription, string basvuru_rapor, string birimi)
        {
            return await db.UpdateReportInvexState(sirket_id, basvuru_id, etkin, islem, silindi, ReportState,
                actioninfo, actionDescription, DateTime.UtcNow, basvuru_rapor, birimi).ConfigureAwait(false);
        }
        public async Task<bool> DeleteSyncCompletedData()
        {
            return await db.DeleteSyncCompletedData().ConfigureAwait(false);
        }
        public async Task<List<coInvexReportFile>> GetInvexReportFiles(int reportid)
        {
            DataTable dt = await db.GetInvexReportFiles(reportid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coInvexReportFile> list = coBaseObject.CreateObjectListFromDataTable<coInvexReportFile>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> UpdateReportVersionNo(int reportID, int userID)
        {
            return await db.UpdateReportVersionNo(reportID, DateTime.UtcNow, userID).ConfigureAwait(false);
        }
        public async Task<List<coReportFile>> GetAllReportFilesByReportID(int reportid, int resourcetype)
        {
            DataTable dt = await db.GetAllReportFilesByReportID(reportid, resourcetype).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReportFile> list = coBaseObject.CreateObjectListFromDataTable<coReportFile>(dt);

                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> ChangeReportYazimState(int reportid, int newstate, int userid)
        {
            return await db.ChangeReportYazimState(reportid, newstate, userid, DateTime.Now).ConfigureAwait(false);
        }
        public async Task<bool> UpdateReportRandevu(int reportid, int userid, long randevudatetick)
        {
            return await db.UpdateReportRandevu(reportid, userid, randevudatetick, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<(bool, int)> UpdateReportTakbis(int kod)
        {
            return await db.UpdateReportTakbis(kod, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> CreateUpdateInvexReportFoto(int kod, int basvuru_kod, int kullanici, int typeid, string guid,
            DateTime tarih, string tarihst, int subtype, int iskapak, int useinreport, string invextype, int sourcetype, int size)
        {
            return await db.CreateUpdateInvexReportFoto(kod, basvuru_kod, kullanici, typeid, guid, DateTime.UtcNow, tarih, tarihst,
                subtype, iskapak, useinreport, invextype, sourcetype, size).ConfigureAwait(false);
        }
        public async Task<bool> UpdateReportFileInUseKapakState(int reportfileid, int usein, int iskapak, int reportid)
        {
            return await db.UpdateReportFileInUseKapakState(reportfileid, usein, iskapak, reportid, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> UpdateReportAdaParselByReportID(int basvuru_kod, string ada, string parsel)
        {
            return await db.UpdateReportAdaParselByReportID(basvuru_kod, ada, parsel, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<List<coReport>> GetUserReports_New(int userid, int usertype, int organizationid,
            string reportcode, int reportsysid, int bankid,
            string province, string county, string neighborhood,
            int state, long datestart, long dateend, int skipindex, int takecount)
        {
            DataTable dt = await db.GetUserReports_New(userid, usertype, organizationid, reportcode, reportsysid, bankid,
                province, county, neighborhood, state, datestart, dateend, skipindex, takecount).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReport> list = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion Report
        #region ReportSubPartValue
        public async Task<int> CreateReportSubPartUserValue_New(int reportid, int subpartid, int userid, string title, int reportsubpartuservalueid,
            int organizationid, int tapuid, int formproportion)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.CreateReportSubPartUserValue_New(reportid, subpartid, userid, datetime, title,
                reportsubpartuservalueid, organizationid, tapuid, formproportion).ConfigureAwait(false);
        }
        public async Task<int> CreateReportSubPartUserFieldValue(int reportsubartuservalueid, int sectionfieldid, string reportsubpartuserfieldvalue,
            int reportsubpartuserfieldvalueid, string sysid)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.CreateReportSubPartUserFieldValue(reportsubartuservalueid, sectionfieldid, reportsubpartuserfieldvalue,
                datetime, reportsubpartuserfieldvalueid, sysid).ConfigureAwait(false);
        }
        public async Task<bool> CopyReportSubPartUserValue(int reportSubPartUserValueID, bool copyValues, int userID, int reportID)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.CopyReportSubPartUserValue(reportSubPartUserValueID, datetime, copyValues, userID, reportID).ConfigureAwait(false);
        }
        public async Task<bool> DeleteReportSubPartUserValue(int reportSubPartUserValueID, int userID, int reportID)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.DeleteReportSubPartUserValue(reportSubPartUserValueID, userID, datetime, reportID).ConfigureAwait(false);
        }
        #endregion ReportSubPartValue
        #region Dashboard
        public async Task<string> GetDashboardCounts(int userid, int usertype, long datestarttick, long dateendtick, int organizationid)
        {
            return await db.GetDashboardCounts(userid, usertype, datestarttick, dateendtick, organizationid).ConfigureAwait(false);
        }
        public async Task<List<coReportNote>> GetDashboardReportNotes(int userid, int userrole, int organizationid)
        {
            DataTable dt = await db.GetDashboardReportNotes(userid, userrole, organizationid);
            if (dt != null)
            {
                List<coReportNote> list = coBaseObject.CreateObjectListFromDataTable<coReportNote>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReport>> GetDashboardReports(int userid, int usertype, int organizationid, int isall, int randevu, int takbis)
        {
            DataTable dt = await db.GetDashboardReports(userid, usertype, organizationid, isall, randevu, takbis).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReport> list = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReport>> GetDashboardReports(int userid, int usertype, int organizationid)
        {
            DataTable dt = await db.GetDashboardReports(userid, usertype, organizationid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReport> list = coBaseObject.CreateObjectListFromDataTable<coReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coAnnouncement>> GetAnnouncement(int userid, int organizationid)
        {
            DataTable dt = await db.GetAnnouncement(userid, organizationid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coAnnouncement> list = coBaseObject.CreateObjectListFromDataTable<coAnnouncement>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion Dashboard
        #region Adres
        public async Task<List<coProvince>> GetProvince()
        {
            DataTable dt = await db.GetProvince();
            if (dt != null)
            {
                List<coProvince> list = coBaseObject.CreateObjectListFromDataTable<coProvince>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coCounty>> GetCountyByProvinceID(int provinceid)
        {
            DataTable dt = await db.GetCountyByProvinceID(provinceid);
            if (dt != null)
            {
                List<coCounty> list = coBaseObject.CreateObjectListFromDataTable<coCounty>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coNeighborhood>> GetNeighborhoodByCountyID(int countyid)
        {
            DataTable dt = await db.GetNeighborhoodByCountyID(countyid);
            if (dt != null)
            {
                List<coNeighborhood> list = coBaseObject.CreateObjectListFromDataTable<coNeighborhood>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coVillage>> GetVillageByCountyID(int countyid)
        {
            DataTable dt = await db.GetVillageByCountyID(countyid);
            if (dt != null)
            {
                List<coVillage> list = coBaseObject.CreateObjectListFromDataTable<coVillage>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion Adres
        #region UserReportCustomParameter
        public async Task<List<coUserReportCustomParameter>> GetUserReportCustomParameter(int userid, int sectionfieldid)
        {
            DataTable dt = await db.GetUserReportCustomParameter(userid, sectionfieldid);
            if (dt != null)
            {
                List<coUserReportCustomParameter> list = coBaseObject.CreateObjectListFromDataTable<coUserReportCustomParameter>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateUserReportCustomParameter(int userid, int sectionfieldid, string userreportcustomparametertext)
        {
            return await db.CreateUserReportCustomParameter(userid, sectionfieldid, userreportcustomparametertext);
        }
        public async Task<bool> DeleteUserReportCustomParameter(int userid, int userreportcustomparameterid)
        {
            return await db.DeleteUserReportCustomParameter(userid, userreportcustomparameterid);
        }
        #endregion UserReportCustomParameter
        #region ReportNote
        public async Task<List<coReportNote>> GetAllReportNotes(int userid, int userrole, int skipindex, int takecount, int organizationid)
        {
            DataTable dt = await db.GetAllReportNotes(userid, userrole, skipindex, takecount, organizationid);
            if (dt != null)
            {
                List<coReportNote> list = coBaseObject.CreateObjectListFromDataTable<coReportNote>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coReportNote>> GetReportNotes(int reportid, int userid, int userrole)
        {
            DataTable dt = await db.GetReportNotes(reportid, userid, userrole);
            if (dt != null)
            {
                List<coReportNote> list = coBaseObject.CreateObjectListFromDataTable<coReportNote>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        /*public async Task<bool> CreateReportNote(int userid, int reportid, int type, string text,
            int ownertype, int subjectid)
        {
            return await db.CreateReportNote(userid, reportid, type, text, ownertype, subjectid).ConfigureAwait(false);
        }*/
        /*
        public async Task<bool> CreateReportNoteNew(int userid, int reportid, int type, string text,
            int ownertype, int subjectid, int sd, int su, int sdu, string invextype, long datetick, string username)
        {
            return await db.CreateReportNoteNew(userid, reportid, type, text, ownertype, subjectid,
                sd, su, sdu, invextype, datetick, username).ConfigureAwait(false);
        }
        /*public async Task<bool> CreateReportNoteByService(int userid, int reportid, string type, string text,
         int subjectid)
        {
            return await db.CreateReportNoteByService(userid, reportid, type, text, subjectid).ConfigureAwait(false);
        }*/
        /*
        public async Task<bool> CreateReportNoteByServiceNew(int userid, int reportid, string type, string text,
         int subjectid, int sd, int su, int sdu, string invextype, long datetick, string username)
        {
            return await db.CreateReportNoteByServiceNew(userid, reportid, type, text, subjectid,
                sd, su, sdu, invextype, datetick, username).ConfigureAwait(false);
        }
        public async Task<bool> CreateNoteRead(int userid, int reportid)
        {
            return await db.CreateNoteRead(userid, reportid).ConfigureAwait(false);
        }
        public async Task<List<coReportNoteSubject>> GetReportNoteSubjectList()
        {
            DataTable dt = await db.GetReportNoteSubjectList();
            if (dt != null)
            {
                List<coReportNoteSubject> list = coBaseObject.CreateObjectListFromDataTable<coReportNoteSubject>(dt);
                return list;
            }
            else
            {
                return null;
            }
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
            return await db.CreateMasraf(basvuru_kod, UserID, ResourceType,
                degerleme_bedeli, degerleme_bedeli_kdv, degerleme_bedeli_toplam,
                resmi_harc_tapu, resmi_harc_tapu_kdv, resmi_harc_tapu_toplam,
                resmi_harc_belediye, resmi_harc_belediye_kdv, resmi_harc_belediye_toplam,
                ulasim_bedeli, ulasim_bedeli_kdv, ulasim_bedeli_toplam,
                konaklama_bedeli, konaklama_bedeli_kdv, konaklama_bedeli_toplam,
                diger_harcamalar, diger_harcamalar_kdv, diger_harcamalar_toplam,
                InvexID, aciklama).ConfigureAwait(false);
        }
        public async Task<List<coMasraf>> GetMasrafList(int reportid)
        {
            DataTable dt = await db.GetMasrafList(reportid);
            if (dt != null)
            {
                List<coMasraf> list = coBaseObject.CreateObjectListFromDataTable<coMasraf>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateMasrafByMobile(int basvuru_kod, int reportid, int UserID, int ResourceType,
           float resmi_harc_tapu, float resmi_harc_tapu_kdv, float resmi_harc_tapu_toplam,
           float resmi_harc_belediye, float resmi_harc_belediye_kdv, float resmi_harc_belediye_toplam,
           float ulasim_bedeli, float ulasim_bedeli_kdv, float ulasim_bedeli_toplam,
           float konaklama_bedeli, float konaklama_bedeli_kdv, float konaklama_bedeli_toplam,
           float diger_harcamalar, float diger_harcamalar_kdv, float diger_harcamalar_toplam,
           string aciklama)
        {
            return await db.CreateMasrafByMobile(basvuru_kod, reportid, UserID, ResourceType,
                resmi_harc_tapu, resmi_harc_tapu_kdv, resmi_harc_tapu_toplam,
                resmi_harc_belediye, resmi_harc_belediye_kdv, resmi_harc_belediye_toplam,
                ulasim_bedeli, ulasim_bedeli_kdv, ulasim_bedeli_toplam,
                konaklama_bedeli, konaklama_bedeli_kdv, konaklama_bedeli_toplam,
                diger_harcamalar, diger_harcamalar_kdv, diger_harcamalar_toplam,
                aciklama).ConfigureAwait(false);
        }
        #endregion Masraf
        #region Other
        public async Task<int> GetTasinmazKimlikNoByReportID(int reportid)
        {
            return await db.GetTasinmazKimlikNoByReportID(reportid);
        }
        public async Task<List<coTakbis>> GetReportTakbisList(int reportid)
        {
            DataTable dt = await db.GetReportTakbisList(reportid);
            if (dt != null)
            {
                List<coTakbis> list = coBaseObject.CreateObjectListFromDataTable<coTakbis>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coDigitalMunicipality>> GetDigitalMunicipality()
        {
            DataTable dt = await db.GetDigitalMunicipality();
            if (dt != null)
            {
                List<coDigitalMunicipality> list = coBaseObject.CreateObjectListFromDataTable<coDigitalMunicipality>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<DataTable> GetTempReport()
        {
            return await db.GetTempReport();
        }
        public async Task<int> InsertTestData(string jsonData, int dataType, string methodname)
        {
            return await db.InsertTestData(jsonData, dataType, methodname, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> UpdateSyncDataState(int syncDataID, int state, int reportid)
        {
            return await db.UpdateSyncDataState(syncDataID, state, reportid, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<List<coTestData>> GetSyncInCompletedData()
        {
            DataTable dt = await db.GetSyncInCompletedData();
            if (dt != null)
            {
                List<coTestData> list = coBaseObject.CreateObjectListFromDataTable<coTestData>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coTestData>> GetSyncReportDataAll()
        {
            DataTable dt = await db.GetSyncReportDataAll();
            if (dt != null)
            {
                List<coTestData> list = coBaseObject.CreateObjectListFromDataTable<coTestData>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTestData> GetSyncDataByID(int syncoid)
        {
            DataTable dt = await db.GetSyncDataByID(syncoid);
            if (dt != null)
            {
                List<coTestData> list = coBaseObject.CreateObjectListFromDataTable<coTestData>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        #endregion Other
        #region System
        public async Task<int> GetAppVersion(int platformid)
        {
            return await db.GetAppVersion(platformid).ConfigureAwait(false);
        }
        #endregion System
        #region InvexSync
        public async Task<List<coOrganization>> GetOrganizationList()
        {
            DataTable dt = await db.GetOrganizationList();
            if (dt != null)
            {
                List<coOrganization> list = coBaseObject.CreateObjectListFromDataTable<coOrganization>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateUpdateOrganization(string kisaad, string unvan, string logo, int orgid, bool hasMobile)
        {
            return await db.CreateUpdateOrganization(kisaad, unvan, logo, orgid, DateTime.UtcNow, hasMobile).ConfigureAwait(false);
        }
        public async Task<bool> CreateSyncLog(string synclogname, bool hasAnyError)
        {
            return await db.CreateSyncLog(synclogname, hasAnyError, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> CreateUpdateBank(int kod, string type, string adi, string kisaadi, string kodu, string logo, int teslimsure,
            int teslimsureuzman, int organizationid)
        {
            return await db.CreateUpdateBank(kod, type, adi, kisaadi, kodu, logo, teslimsure, teslimsureuzman, organizationid, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> CreateUpdateUser(int kod, string adi, string soyadi, string eposta, string tipi, int organizationid)
        {
            return await db.CreateUpdateUser(kod, adi, soyadi, eposta, tipi, organizationid, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<int> CreateUpdateReportByService(int kod, string sirket_rapor_no, int uzman, string il, string ilce, string mahalle, string yergostericiad,
            string musteri_unvani, string yergostericitel, string sirket_id, int banka, int etkin, int islem, int silindi, string actioninfo, string actiondescription,
            string basvuru_rapor, string birimi, DateTime sysDate, DateTime uzmanSonTarih, int acil, int vip, int takbis)
        {
            return await db.CreateUpdateReportByService(kod, sirket_rapor_no, uzman, il, ilce, mahalle, yergostericiad, musteri_unvani, yergostericitel, sirket_id,
                banka, etkin, islem, silindi, actioninfo, actiondescription, DateTime.UtcNow, basvuru_rapor, birimi, sysDate, uzmanSonTarih,
                acil, vip, takbis).ConfigureAwait(false);
        }
        public async Task<int> CreateInvexSyncReportSubPartUserValue(int reportid, string subpartcode, int uzman, int invexkod, int tapuid, int reportsysid)
        {
            return await db.CreateInvexSyncReportSubPartUserValue(reportid, subpartcode, uzman, invexkod, tapuid, DateTime.UtcNow, reportsysid).ConfigureAwait(false);
        }
        public async Task<bool> CreateInvexSyncReportSubPartUserFieldValue(int reportsubpartuservalueid, string fieldinvexcode, string subpartcode, string fieldinvexvalue)
        {
            return await db.CreateInvexSyncReportSubPartUserFieldValue(reportsubpartuservalueid, fieldinvexcode, subpartcode, fieldinvexvalue, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> CreateInvexSyncReport(int reportid, int state, int userid, int type, string basvuru, string birim)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.CreateInvexSyncReport(reportid, state, userid, type, basvuru, birim, datetime).ConfigureAwait(false);
        }
        public async Task<bool> UpdateInvexSyncReportState(int reportid, int state, string note)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.UpdateInvexSyncReportState(reportid, state, note, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> UpdateInvexSyncReportYazimState(int reportid, int state, string note)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.UpdateInvexSyncReportYazimState(reportid, state, note, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<List<coInvexSyncReport>> GetSyncReportList()
        {
            DataTable dt = await db.GetSyncReportList().ConfigureAwait(false);
            if (dt != null)
            {
                List<coInvexSyncReport> list = coBaseObject.CreateObjectListFromDataTable<coInvexSyncReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coInvexSyncReport>> GetSyncReportYazimList()
        {
            DataTable dt = await db.GetSyncReportYazimList().ConfigureAwait(false);
            if (dt != null)
            {
                List<coInvexSyncReport> list = coBaseObject.CreateObjectListFromDataTable<coInvexSyncReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coInvexSyncReport>> GetSyncReportByID(int syncreportid)
        {
            DataTable dt = await db.GetSyncReportByID(syncreportid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coInvexSyncReport> list = coBaseObject.CreateObjectListFromDataTable<coInvexSyncReport>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coInvexSyncFieldSubPartCodes>> GetTemplateSubPartFieldCodes(string subpartcodes)
        {
            DataTable dt = await db.GetTemplateSubPartFieldCodes(subpartcodes).ConfigureAwait(false);
            if (dt != null)
            {
                List<coInvexSyncFieldSubPartCodes> list = coBaseObject.CreateObjectListFromDataTable<coInvexSyncFieldSubPartCodes>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> ChangeReportStateFromMobile(int userid, int reportid, string basvuru_rapor, string birimi)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.ChangeReportStateFromMobile(userid, reportid, basvuru_rapor, birimi, datetime).ConfigureAwait(false);
        }
        public async Task<bool> CreateUpdateInvexReportFile(int basvuru_kod, int kod, int kullanici, int silindi, int silindi_kullanici,
            string tipi, string dosyatipi, string adi, string orjadi, string tarih, int size)
        {
            return await db.CreateUpdateInvexReportFile(basvuru_kod, kod, kullanici, silindi, silindi_kullanici,
                tipi, dosyatipi, adi, orjadi, tarih, DateTime.UtcNow, size).ConfigureAwait(false);
        }
        public async Task<bool> UpdateInvexReportFoto(int kod)
        {
            return await db.UpdateInvexReportFoto(kod, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> DeleteInvexReportFoto(int kod, int basvuru_kod)
        {
            return await db.DeleteInvexReportFoto(kod, basvuru_kod, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<bool> UpdateInvexReportFile(int kod, string tipi)
        {
            return await db.UpdateInvexReportFile(kod, tipi, DateTime.UtcNow);
        }
        public async Task<bool> DeleteInvexReportFile(int kod)
        {
            return await db.DeleteInvexReportFile(kod, DateTime.UtcNow);
        }
        public async Task<int> GetReportIDByBasvuruKod(int kod)
        {
            return await db.GetReportIDByBasvuruKod(kod).ConfigureAwait(false);
        }
        public async Task<(int reportid, int userid, string reportcode, bool res)> CreateOrUpdateBasvuruRevizyon(string sirket_id, int kod, int basvuru_kod, DateTime revizyon_tarihi,
            string revizyon_gerekcesi, string revizyon_gerekcesi_aciklama, string revizyon_sonucu, string revizyon_sonucu_aciklama,
            string not_sonuc_ekle, string sonuc, string sonuc_aciklama_banka, string sonuc_aciklama_sirket, string revizyon_notu)
        {
            return await db.CreateOrUpdateBasvuruRevizyon(sirket_id, kod, basvuru_kod, revizyon_tarihi, DateTime.Now, revizyon_gerekcesi, revizyon_gerekcesi_aciklama,
                revizyon_sonucu, revizyon_sonucu_aciklama, not_sonuc_ekle, sonuc, sonuc_aciklama_banka, sonuc_aciklama_sirket, revizyon_notu).ConfigureAwait(false);
        }
        public async Task<(int userid, string reportcode, bool res)> UpdateBirimInvexSync(int uzman, int basvuru_kod, int banka)
        {
            return await db.UpdateBirimInvexSync(uzman, basvuru_kod, banka, DateTime.UtcNow).ConfigureAwait(false);
        }
        public async Task<List<coInvexSyncFieldSubPartCodes>> GetReportSubPartSyncData(int reportid, string subpartcodes)
        {
            DataTable dt = await db.GetReportSubPartSyncData(reportid, subpartcodes).ConfigureAwait(false);
            if (dt != null)
            {
                List<coInvexSyncFieldSubPartCodes> list = coBaseObject.CreateObjectListFromDataTable<coInvexSyncFieldSubPartCodes>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> UpdateInvexSubpartCode(int kod, int subpartuservalueid)
        {
            return await db.UpdateInvexSubpartCode(kod, subpartuservalueid);
        }
        public async Task<List<coReportFile>> GetSyncReportFilesByReportID(int reportid)
        {
            DataTable dt = await db.GetSyncReportFilesByReportID(reportid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coReportFile> list = coBaseObject.CreateObjectListFromDataTable<coReportFile>(dt);

                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> UpdateReportFileSyncState(int reportfileid, int syncstate, int sysid)
        {
            return await db.UpdateReportFileSyncState(reportfileid, syncstate, sysid, DateTime.UtcNow).ConfigureAwait(false);
        }
        #endregion InvexSync
        #region PushLog
        public async Task<bool> InsertPushLog(int notifytpe, string text, string tagguid, int userid, string objguid, int reportsysid)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.InsertPushLog(notifytpe, text, tagguid, userid, objguid, datetime, reportsysid).ConfigureAwait(false);
        }
        public async Task<bool> UpdatePushLogState(int logid, int state)
        {
            DateTime datetime = DateTime.UtcNow;
            return await db.UpdatePushLogState(logid, state, datetime).ConfigureAwait(false);
        }
        public async Task<List<coPushLog>> GetPushLogs()
        {
            DataTable dt = await db.GetPushLogs().ConfigureAwait(false);
            if (dt != null)
            {
                List<coPushLog> list = coBaseObject.CreateObjectListFromDataTable<coPushLog>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coPushLog> GetPushLogByID(int pushlogid)
        {
            DataTable dt = await db.GetPushLogByID(pushlogid).ConfigureAwait(false);

            if (dt != null && dt.Rows.Count > 0)
            {
                List<coPushLog> list = coBaseObject.CreateObjectListFromDataTable<coPushLog>(dt);
                return list[0];
            }
            else
            {
                return null;
            }
        }
        #endregion PushLog
        #region SablonCreate
        #region Templates
        public async Task<List<coTemplate>> GetTemplates_Web()
        {
            DataTable dt = await db.GetTemplates_Web().ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplate> list = coBaseObject.CreateObjectListFromDataTable<coTemplate>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coTemplate>> GetTemplateList_Web()
        {
            DataTable dt = await db.GetTemplateList_Web().ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplate> list = coBaseObject.CreateObjectListFromDataTable<coTemplate>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<bool> CreateBankTemplate_Web(int bankid, int templateid, int userid)
        {
            return await db.CreateBankTemplate_Web(bankid, templateid, userid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> DeleteBankTemplateByID_Web(int banktemplateid, int userid)
        {
            return await db.DeleteBankTemplateByID_Web(banktemplateid, userid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<List<coBank>> GetOrganizationBanks(int organiationid)
        {
            DataTable dt = await db.GetOrganizationBanks(organiationid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coBank> list = coBaseObject.CreateObjectListFromDataTable<coBank>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        #endregion Templates
        #region TemplateParts
        public async Task<List<coTemplatePart>> GetTemplateParts_Web(int banktemplateid)
        {
            DataTable dt = await db.GetTemplateParts_Web(banktemplateid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplatePart> list = coBaseObject.CreateObjectListFromDataTable<coTemplatePart>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTemplatePart> GetTemplatePartByID_Web(int templatepartid)
        {
            DataTable dt = await db.GetTemplatePartByID_Web(templatepartid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplatePart> list = coBaseObject.CreateObjectListFromDataTable<coTemplatePart>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        public async Task<bool> DeleteTemplatePart_Web(int templatepartid, int banktemplateid)
        {
            return await db.DeleteTemplatePart_Web(templatepartid, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> CreateTemplatePart_Web(string templatepartname, int templatepartindex, int banktemplateid, string templatepartsyscode)
        {
            return await db.CreateTemplatePart_Web(templatepartname, templatepartindex, banktemplateid, templatepartsyscode, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> UpdateTemplatePart_Web(string templatepartname, int templatepartindex, int templatepartid, string templatepartsyscode, int banktemplateid)
        {
            return await db.UpdateTemplatePart_Web(templatepartname, templatepartindex, templatepartid, templatepartsyscode, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        #endregion TemplateParts
        #region TemplateSubParts
        public async Task<List<coTemplateSubPart>> GetTemplateSubParts_Web(int templatepartid)
        {
            DataTable dt = await db.GetTemplateSubParts_Web(templatepartid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplateSubPart> list = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPart>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTemplateSubPart> GetTemplateSubPartByID_Web(int templatesubpartid)
        {
            DataTable dt = await db.GetTemplateSubPartByID_Web(templatesubpartid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplateSubPart> list = coBaseObject.CreateObjectListFromDataTable<coTemplateSubPart>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        public async Task<bool> DeleteTemplateSubPart_Web(int templatesubpartid, int banktemplateid)
        {
            return await db.DeleteTemplateSubPart_Web(templatesubpartid, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> CreateTemplateSubPart_Web(string name, int index, int partid, bool hasmultiple, string code, string altlik, bool istapubased, string syscode, int banktemplateid)
        {
            return await db.CreateTemplateSubPart_Web(name, index, partid, hasmultiple, code, altlik, istapubased, syscode, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> UpdateTemplateSubPart_Web(int subpartid, string name, int index, bool hasmultiple, string code, string altlik, bool istapubased, string syscode, int banktemplateid)
        {
            return await db.UpdateTemplateSubPart_Web(subpartid, name, index, hasmultiple, code, altlik, istapubased, syscode, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        #endregion TemplateSubParts
        #region TemplateSections
        public async Task<List<coTemplateSection>> GetTemplateSections_Web(int templatesubpartid)
        {
            DataTable dt = await db.GetTemplateSections_Web(templatesubpartid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplateSection> list = coBaseObject.CreateObjectListFromDataTable<coTemplateSection>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coTemplateSection> GetTemplateSectionByID_Web(int templatesectionid)
        {
            DataTable dt = await db.GetTemplateSectionByID_Web(templatesectionid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coTemplateSection> list = coBaseObject.CreateObjectListFromDataTable<coTemplateSection>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        public async Task<bool> DeleteTemplateSection_Web(int templatesectionid, int banktemplateid)
        {
            return await db.DeleteTemplateSection_Web(templatesectionid, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> CreateTemplateSection_Web(string name, int index, int subpartid, bool hastitle, string visibilityids, string bankvibilityids, string syscode, int banktemplateid)
        {
            return await db.CreateTemplateSection_Web(name, index, subpartid, hastitle, visibilityids, bankvibilityids, syscode, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> UpdateTemplateSection_Web(int sectionid, string name, int index, bool hastitle, string visibilityids, string bankvibilityids, string syscode, int banktemplateid)
        {
            return await db.UpdateTemplateSection_Web(sectionid, name, index, hastitle, visibilityids, bankvibilityids, syscode, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        #endregion TemplateSections
        #region SectionFields
        public async Task<List<coField>> GetSectionFields_Web(int templatesectionid)
        {
            DataTable dt = await db.GetSectionFields_Web(templatesectionid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coField> list = coBaseObject.CreateObjectListFromDataTable<coField>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coField> GetSectionFieldByID_Web(int sectionfieldid)
        {
            DataTable dt = await db.GetSectionFieldByID_Web(sectionfieldid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coField> list = coBaseObject.CreateObjectListFromDataTable<coField>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        public async Task<bool> DeleteSectionField_Web(int sectionfieldid, int banktemplateid)
        {
            return await db.DeleteSectionField_Web(sectionfieldid, banktemplateid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> CreateSectionField_Web(int templatesectionid, int fieldid, int index, bool isrequired,
                                            string minvalue, string maxvalue, string visibilityids, string bankvisibilityids,
                                            int banktemplateid, int isbankfield)
        {
            return await db.CreateSectionField_Web(templatesectionid, fieldid, index, isrequired, minvalue, maxvalue,
                visibilityids, bankvisibilityids, banktemplateid, DateTime.UtcNow.Ticks, isbankfield).ConfigureAwait(false);
        }
        public async Task<bool> UpdateSectionField_Web(int sectionfieldid, int index, bool isrequired,
                                            string minvalue, string maxvalue, string visibilityids, string bankvisibilityids,
                                            int banktemplateid, int isbankfield)
        {
            return await db.UpdateSectionField_Web(sectionfieldid, index, isrequired, minvalue, maxvalue,
                visibilityids, bankvisibilityids, banktemplateid, DateTime.UtcNow.Ticks, isbankfield).ConfigureAwait(false);
        }
        #endregion SectionFields
        #region Fields
        public async Task<List<coFieldType>> GetFieldTypes_Web()
        {
            DataTable dt = await db.GetFieldTypes_Web().ConfigureAwait(false);
            if (dt != null)
            {
                List<coFieldType> list = coBaseObject.CreateObjectListFromDataTable<coFieldType>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<List<coField>> GetFields_Web()
        {
            DataTable dt = await db.GetFields_Web().ConfigureAwait(false);
            if (dt != null)
            {
                List<coField> list = coBaseObject.CreateObjectListFromDataTable<coField>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coField> GetFieldByID_Web(int fieldid)
        {
            DataTable dt = await db.GetFieldByID_Web(fieldid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coField> list = coBaseObject.CreateObjectListFromDataTable<coField>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        public async Task<bool> DeleteField_Web(int fieldid)
        {
            return await db.DeleteField_Web(fieldid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> CreateField_Web(string name, string code, int type, string subinfo, string altlikformat, string syscode)
        {
            return await db.CreateField_Web(name, code, type, subinfo, altlikformat, syscode, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> UpdateField_Web(int fieldid, string name, string code, int type, string subinfo, string altlikformat, string syscode)
        {
            return await db.UpdateField_Web(fieldid, name, code, type, subinfo, altlikformat, syscode, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        #endregion Fields
        #region FieldParameters
        public async Task<List<coFieldParameter>> GetFieldParameters_Web(int fieldid)
        {
            DataTable dt = await db.GetFieldParameters_Web(fieldid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coFieldParameter> list = coBaseObject.CreateObjectListFromDataTable<coFieldParameter>(dt);
                return list;
            }
            else
            {
                return null;
            }
        }
        public async Task<coFieldParameter> GetFieldParameterByID_Web(int fieldparameterid)
        {
            DataTable dt = await db.GetFieldParameterByID_Web(fieldparameterid).ConfigureAwait(false);
            if (dt != null)
            {
                List<coFieldParameter> list = coBaseObject.CreateObjectListFromDataTable<coFieldParameter>(dt);
                if (list.Count > 0)
                {
                    return list[0];
                }
            }

            return null;
        }
        public async Task<bool> DeleteFieldParameter_Web(int fieldparameterid)
        {
            return await db.DeleteFieldParameter_Web(fieldparameterid, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> CreateFieldParameter_Web(int fieldid, string text, string value, bool isdefault, int index, string syscode)
        {
            return await db.CreateFieldParameter_Web(fieldid, text, value, isdefault, index, syscode, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        public async Task<bool> UpdateFieldParameter_Web(int fieldparameterid, string text, string value, bool isdefault, int index, string syscode)
        {
            return await db.UpdateFieldParameter_Web(fieldparameterid, text, value, isdefault, index, syscode, DateTime.UtcNow.Ticks).ConfigureAwait(false);
        }
        #endregion FieldParameters
        #endregion SablonCreate
    }*/
    }
}
