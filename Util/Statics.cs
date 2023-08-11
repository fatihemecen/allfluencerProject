using System;
using System.Collections.Generic;
using System.Text;

namespace Util
{
    public class Statics
    {
        public static class ApplicationStrings
        {
            public static string SessionSharedKey = "bXc2us41";
            public static string SessionSaltText = "o320564ekkM7d0";
            public static string KeySeperator = "___[]___";
            public static string AzureAppName = "NH-SDS";
            public static string AzureApnKey = "Endpoint=sb://NHNM-SDS.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=34IZq6uGvfSfR0iwnvhGSTtQwXmB2UYgS2fRzz9VVfA=";
        }

        public static class ErrorCodes
        {
            public static string FailedCall = "Hatalı çağrı";
            public static string WrongUserPass = "Yanlış kullanıcı adı veya şifre girdiniz";
            public static string InvalidToken = "Geçersiz Giriş Anahtarı";
            public static string ErrorOnOperation = "İşlem yapılırken bir hata oluştu";
            public static string Error = "Hata oluştu";
            public static string SuccessUpd = "Kayıt başarılı bir şekilde güncellendi";
            public static string SuccessSave = "Kayıt başarılı bir şekilde kaydedildi";
            public static string SuccessDel = "Kayıt başarılı bir şekilde silindi";
            public static string InvalidVersion = "Uygulamanızın versiyonu güncel değil. Lütfen güncelleme yapın";
            public static string MustLogon = "Sisteme giriş yapmalısınız";
            public static string SelectImage = "Resim seçmediniz";
            public static string FillAllFields = "Lütfen tüm alanları doldurun";
            public static string FillPublicationInfo = "Yayın bilgilerinin tamamını doldurun";
            public static string ReSelectImage = "Lütfen yeniden resim yükleyin";
            public static string TakbisFileNotFound = "Takbis datası bulunamadı.";
            public static string TakbisFileImported = "Takbis dosyası başarıyla işlendi.";
            public static string ErrorNotAuthorized = "Bu işlemi gerçekleştirmek için yetkiniz bulunmamaktadır.";
        }

        public static class QueryStrings
        {
            public static string UserID = "userid";
            public static string Password = "password";
            public static string SessionID = "sessionid";
            public static string Key = "key";
            public static string UserName = "username";
            public static string IsMobil = "ismobil";
            public static string IsNew = "isnew";
            public static string Type = "type";
            public static string OrganizationGuid = "orgguid";
            public static string OrgName = "orgname";
            public static string SessionKey = "sessionkey";
            public static string UserGuid = "userguid";
            public static string FileGuid = "fileguid";
            public static string PasswordResetKey = "passwordresetkey";
        }
        public static class SystemKeys
        {
            public static string skJwt = "jwt";
            public static string skTokenHeader = "tokenheader";
            public static string skKey = "key";
            public static string skSessionKey = "sessionkey";
            public static string skServiceCallDateTick = "servicecalldatetick";
            public static string skAuthorizationStatus = "AuthorizationStatus";
            public static string skTempParam = "tempparam";
            public static string skTemplatemaxtick = "templatemaxtick";
            public static string skReporttemplate = "reporttemplate";
        }
        public static class SystemValues
        {
            public static string Key = "64B7F487-6227-4BFA-A9C2-33SE22064DD1";
            public static int PageSize = 10;
            public static string blobUrl = "https://argefiles.blob.core.windows.net/files/";
        }
        public static class MethodNames
        {
            public static string srvCheckPreToken = "CheckPreToken";
            public static string srvCreatePreToken = "CreatePreToken";
            public static string srvRegister = "Register";
            public static string srvVerifyCode = "VerifyCode";
            public static string srvSendVerifyCode = "SendVerifyCode";
            public static string srvLoginUser = "LoginUser";
            public static string srvGetSureList = "GetSureList";
            public static string srvGetCeviriList = "GetCeviriList";
            public static string srvGetSureMetaByNo = "GetSureMetaByNo";
            public static string srvGetAddressAll = "GetAddressAll";
            public static string srvGetUserList = "GetUserList";
            public static string srvCreateUser = "CreateUser";

        }
        public static class ApplicationTexts
        {
            public static string AppText = "Allfluencer";
            public static string AppTextUpper = "ALLFLUENCER";
            public static string AppMotto = "\"O, muttakîler için bir yol göstericidir.\"";
            public static string AppSearch = "Bir şeyler ara...";
        }
        public static class ApplicationPaths
        {
            public static string aplProfile = "/profile";
            public static string aplLogin = "/login";
            public static string aplRegister = "/register";
        }

        public static class Enums
        {

            public enum UserCheckResult
            {
                ErrorOccured = 0,
                NotExist = 1,
                UserNameSame = 2,
                EmailSame = 3
            }

            public enum UserRegisterResult
            {
                ErrorOccured = 0,
                Success = 1,
                UserNameSame = 2,
                EmailSame = 3,
                DataHasProblem = 4
            }
            public enum DeviceTypes
            {
                Tablet = 1,
                Telephone = 2
            }

            public enum PlatformTypes
            {
                Apple = 1,
                Android = 2,
                All = 3
            }

            public enum FileTypes
            {
                Image = 1,
                Text = 2,
                Sound = 3
            }

            public enum AddRemove
            {
                Add = 0,
                Remove = 1
            }

            public enum UserRoles
            {
                Admin = 1,
                Uzman = 2,
                Reporter = 3,
                Assigner = 4,
                Operation = 5,
                Bank = 6
            }

            public enum NotificationTypes
            {
                ReportAssignedUser = 1,
                ReportMessageReceived = 2,
                ReportDenetmenIade = 3,
                ReportTalepIptal = 4,
                ReportIptalGeriAl = 5,
                ReportTalepSil = 6,
                ReportTalepSilGeriAl = 7,
                ReportTalepDondur = 8,
                ReportTalepCoz = 9,
                ReportTalepBasvuruGuncellendi = 10,
                ReportTalepKonumGuncellendi = 11,
                ReportRevizyonEklendi = 12,
                ReportSentToYazim = 13,
                ReportBackToExper = 14,
                ReportTakbisAlindi = 15,
                ReportRandevuAlindi = 16,
                ReportUzmanIade = 17,
            }
            public enum BasvuruProcessTypes
            {
                UzmanaAtandi = 1,
                Basvuru = 2,
                Konum = 3,
                Basvuru_Banka = 4,
                Konum_Banka = 5
            }
            public enum SyncTypes
            {
                UzmanaAtandi = 2,
                AsamaDegistir = 3,
                BirimDegistir = 4,
                UzmanRed = 5,
                DenetmenIade = 6,
                TalepIptal = 7,
                IptalGeriAl = 8,
                TalepSil = 9,
                TalepSilGeriAl = 10,
                TalepDondur = 11,
                TalepCoz = 12,
                BasvuruKaydet = 13,
                KonumKaydet = 14,
                TapuOzellikKaydet = 15,
                AnaTasinmazKaydet = 16,
                TapuDegerlemeKaydet = 17,
                TapuSicilKaydet = 18,
                YetkiYazisiKaydet = 19,
                YetkiYazisiSil = 20,
                BasvuruRevizyonKaydet = 21,
                BasvuruRevizyonNotKaydet = 22,
                BelgelerYuklendi = 23,
                TalepBelgeSil = 24,
                TalepBelgebilgiGuncelle = 25,
                FotoYukle = 26,
                TalepFotoSil = 27,
                TalepFotobilgiGuncelle = 28,
                MasrafKaydet = 29,
                IleriGeri = 30,
                UzmanEklendi = 31,
                UzmanDurumDegisti = 32,
                TakbisTapuGuncellendi = 33,
                EmsalGuncelleKaydet = 34,
                TalepNotEkle = 35,
                BasvuruRevizyonGuncelle = 36,
                MasrafBelgelerYukle = 37,
                TapuDegerlemeGenelKaydet = 38
            }
            public enum TokenCheckStatus
            {
                NotExist = 1,
                CreatedFromLocal = 2,
                AlreadyCreated = 3
            }
            public enum ReportStates
            {
                Beklemede = 0,
                Uzmanda = 1,
                Denetmende = 2,
                Denetmen2de = 3,
                Sekreteryada = 4,
                MusteriOnOnay = 5,
                Tamamlanmis = 6
            }
            public enum FieldType
            {
                FieldTypeDefault = 0,
                FieldTypeCombobox = 1,
                FieldTypeText = 2,
                FieldTypeDatetime = 3,
                FieldTypeInteger = 4,
                FieldTypeFloat = 5,
                FieldTypeRadioButton = 6,
                FieldTypeCheckbox = 7,
                FieldTypeListbox = 8,
                FieldTypeMultilineTextbox = 9,
                FieldTypeDropdown = 10,
                FieldTypeShortText = 11,
                FieldTypeSubReport = 12,
                FieldTypeCoordinate = 13,
                FieldTypeProvince = 14,
                FieldTypeCounty = 15,
                FieldTypeNeighborhood = 16,
                FieldTypeVillage = 17,
                FieldTypeMultilineTemplate = 18,
                FieldTypeCoordinateWithWeb = 19,
                FieldTypeAdresCheck = 20,
                FieldTypeLabel = 21,
                FieldTypeMalik = 22
            }
        }

        public static class SessionStrings
        {
            public static string DataBaseConnection = "DataBaseConnection";
            public static string Organization = "Organization";
            public static string User = "User";
            public static string UserList = "UserList";
            public static string SelectedUser = "selecteduser";
            public static string SelectedFile = "selectedfile";
            public static string SelectedFileList = "selectedfilelist";
            public static string TopLevel = "toplevel";
            public static string TempPhoto = "tempphoto_";
            public static string SelectedSession = "selectedsession";
            public static string BusinessObject = "businessobject";
            public static string OrgName = "OrgName";
            public static string PageData = "PageData_";
            public static string Subjects = "Subjects";
            public static string TakbisList = "takbislist";
            public static string TakbisGeneralInfo = "takbisgeneralinfo_";
            public static string TakbisFile = "takbisfile_";
            public static string TakbisSerhBeyanInfo = "takbisserhbeyan_";
            public static string TakbisMulkiyetBilgileri = "takbismulkiyetbilgileri_";
            public static string TakbisMulkiyetSerhBeyanBilgileri = "takbismulkiyetserhbeyanbilgileri_";
            public static string TakbisEklentiBilgileri = "takbiseklentibilgileri_";
            public static string Template = "template";
            public static string Fields = "fields";
            public static string FieldTypes = "fieldtypes";
        }
        public static class Routing
        {
            public static class OrgLogin
            {
                public const string value = "login/{orgname}";
                public const string name = "OrgLogin";
                public const string path = "~/pages/login.aspx";
            }

            public static class UserList
            {
                public const string value = "userlist";
                public const string name = "UserList";
                public const string path = "~/pages/userlist.aspx";
            }

            public static class UserProfile
            {
                public const string value = "user/{userguid}";
                public const string name = "UserProfile";
                public const string path = "~/pages/userdetail.aspx";
            }

            public static class PasswordReset
            {
                public const string value = "passwordreset";
                public const string name = "PasswordReset";
                public const string path = "~/pages/passwordreset.aspx";
            }

            public static class FileList
            {
                public const string value = "filelist";
                public const string name = "FileList";
                public const string path = "~/pages/filelist.aspx";
            }

            public static class FileDetail
            {
                public const string value = "filedetail/{fileguid}";
                public const string name = "FileDetail";
                public const string path = "~/pages/filedetail.aspx";
            }
            public static class FileTakbis
            {
                public const string value = "filetakbis/{fileguid}";
                public const string name = "FileTakbis";
                public const string path = "~/pages/filetakbis.aspx";
            }
            public static class FileCreate
            {
                public const string value = "filedetail/{fileguid}";
                public const string name = "FileDetail";
                public const string path = "~/pages/filedetail.aspx";
            }
            public static class ReportData
            {
                public const string value = "reportdata/{fileguid}";
                public const string name = "ReportData";
                public const string path = "~/pages/reportdata.aspx";
            }
            public static class Templates
            {
                public const string value = "templates";
                public const string name = "templates";
                public const string path = "~/pages/templates.aspx";
            }
            public static class Fields
            {
                public const string value = "fields";
                public const string name = "fields";
                public const string path = "~/pages/fields.aspx";
            }
        }
    }
}
