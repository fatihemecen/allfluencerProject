using Azure;
using Bus;
using Data;
using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using RestSharp;
using System;
using System.Collections.Generic;
using System.IO.Compression;
using System.Net;
using System.Text.Json.Nodes;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Util;
using static Azure.Core.HttpHeader;

namespace SharedLib
{
    public class ServiceUtility
    {
        //static string conn = "Data Source=tcp:srvsg.database.windows.net;Initial Catalog=dbsg;User ID=srvsgusadmn@srvsg.com;Password=5121665sgusadmn.;MultipleActiveResultSets=True;Application Name=EntityFramework;Connection Timeout=0";
        //static string conn = "Data Source=WIN-MD9VSPI5MI2\\SQLEXPRESS;Initial Catalog=dbsg;User ID=MobilUser;Password=13553100Aa@.;MultipleActiveResultSets=True;Application Name=EntityFramework;Connection Timeout=0";
        //static string conn = "Data Source=WIN-VTIKO66EBNC;Initial Catalog=dbsg;User ID=mobiluser;Password=124124Asx.;MultipleActiveResultSets=True;Application Name=EntityFramework;Connection Timeout=0";
        static string conn = "Server=tcp:srvallfluencer.database.windows.net,1433;Initial Catalog=allfluencer;Persist Security Info=False;User ID=allfluenceradmin;Password=Evilsmurff1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
          public static BUSAsync busAsyncNew = new BUSAsync(new DATAsync(conn));

        #region Utility

        /*
        public static bool ProcessResponseFiles(ref string requestBody, string jsonproperty, bool isfoto = false)
        {
            try
            {
                JObject result = (JObject)JsonConvert.DeserializeObject(requestBody);

                JObject data = (JObject)result["data"];
                if (data.ContainsKey(jsonproperty))
                {
                    JArray fileList = (JArray)data[jsonproperty];
                    bool hasError = false;
                    foreach (JObject file in fileList)
                    {
                        int basvuru_kod = Convert.ToInt32(file["basvuru_kod"].ToString());
                        int kod = Convert.ToInt32(file["kod"].ToString());
                        string tipi = "";
                        string dosyatipi = "";
                        string adi = "";
                        string orjadi = "";
                        if (isfoto)
                        {
                            tipi = file["fotograf_turu"].ToString();
                            dosyatipi = "jpg";
                            adi = file["orjinal_adi"].ToString();
                            orjadi = file["orjinal_adi"].ToString();
                        }
                        else
                        {
                            tipi = file["tipi"].ToString();
                            dosyatipi = file["dosyatipi"].ToString();
                            adi = file["adi"].ToString();
                            orjadi = file["orjadi"].ToString();
                        }

                        string silindi_eh = file["silindi_eh"].ToString();
                        int silindi_kullanici = Convert.ToInt32(file["silindi_kullanici"].ToString());
                        int kullanici = Convert.ToInt32(file["kullanici"].ToString());
                        string tarih = file["tarih"].ToString();

                        string dosya = "";
                        if (isfoto)
                        {
                            dosya = file["foto"].ToString().Replace(@"\/", "/");
                        }
                        else
                        {
                            dosya = file["dosya"].ToString().Replace(@"\/", "/");
                        }

                        byte[] fileBytes = Convert.FromBase64String(dosya);
                        string res = busAsyncNew.tb.SaveFile(fileBytes, false, 0, 750, kod.ToString(), "." + dosyatipi, "reportfiles");
                        if (res.Length == 0 || res.Contains("a:"))
                        {
                            hasError = true;
                        }
                        else
                        {
                            bool ins = busAsyncNew.CreateUpdateInvexReportFile(basvuru_kod, kod, kullanici, (silindi_eh == "E" ? 1 : 0), silindi_kullanici, tipi, dosyatipi, adi, orjadi, tarih, 0).Result;
                            if (!ins)
                            {
                                hasError = true;
                            }
                        }
                    }


                    data.Remove(jsonproperty);
                    //data[jsonproperty].Remove();
                    requestBody = result.ToString();

                    return !hasError;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }
        public static int ProcessYetkiYazisi(ref string requestBody, bool isdeleted)
        {
            bool hasError = false;
            int basvuru_kod = 0;
            try
            {
                JObject result = (JObject)JsonConvert.DeserializeObject(requestBody);

                JObject data = (JObject)result["data"];
                if ((!isdeleted && data.ContainsKey("dosya")) || isdeleted)
                {
                    int kod = Convert.ToInt32(data["yetki_yazisi_id"].ToString());
                    basvuru_kod = Convert.ToInt32(data["basvuru_id"].ToString());
                    int kullanici = Convert.ToInt32(data["kullanici"].ToString());
                    string tarih = data["tarih"].ToString();
                    string il = data["il"].ToString();
                    string ilce = data["ilce"].ToString();

                    if (!isdeleted)
                    {
                        string dosya = data["dosya"].ToString().Replace(@"\/", "/");

                        byte[] fileBytes = Convert.FromBase64String(dosya);
                        string res = busAsyncNew.tb.SaveFile(fileBytes, false, 0, 750, kod.ToString(), ".pdf", "reportfiles");
                        if (res.Length == 0 || res.Contains("a:"))
                        {
                            hasError = true;
                        }
                    }

                    if (!hasError)
                    {
                        bool ins = busAsyncNew.CreateUpdateInvexReportFile(basvuru_kod, kod, kullanici, isdeleted ? 1 : 0, isdeleted ? kullanici : 0, "Yetki Yazısı", "pdf", il + " - " + ilce, il + " - " + ilce, tarih, 0).Result;
                        if (!ins)
                        {
                            hasError = true;
                        }
                    }
                    data.Remove("dosya");
                    requestBody = result.ToString();
                }
                else
                {
                    hasError = true;
                }
            }
            catch
            {
                hasError = true;
            }

            return hasError ? 0 : basvuru_kod;
        }
        static string RemoveFileContentFromResponse(string rawRequest)
        {
            bool hasChange = false;
            if (rawRequest.Contains("dosya_bilgi"))
            {
                rawRequest = rawRequest.Substring(0, rawRequest.IndexOf("dosya_bilgi") - 1);
                hasChange = true;
            }
            if (rawRequest.Contains("foto_bilgi"))
            {
                rawRequest = rawRequest.Substring(0, rawRequest.IndexOf("foto_bilgi") - 1);
                hasChange = true;
            }
            if (hasChange)
            {
                rawRequest = rawRequest.Trim().Trim(new char[] { ',' }) + "}}";
            }

            return rawRequest;
        }

        public static JsonObject getOrgPostData(string OrganizationInvexToken, string OrganizationInvexCode)
        {
            JsonObject data = new JsonObject();
            data["ip"] = "127.0.0.1";
            data["cihaz"] = "system";
            data["token"] = OrganizationInvexToken;
            data["code"] = "00";
            data["sirket_id"] = OrganizationInvexCode;

            JsonObject obj = new JsonObject();
            obj["data"] = data;

            return obj;
        }

        #endregion Utility

        #region Sync
        public static async Task<bool> ProcessSyncData(string requestBody, Statics.Enums.SyncTypes type, int syncid)
        {
            bool res = false;
            int reportid = 0;

            if (requestBody.StartsWith("A"))
            {
                requestBody = "{" + requestBody.Substring(1);
            }

            if (type == Statics.Enums.SyncTypes.YetkiYazisiKaydet)
            {
                reportid = ProcessYetkiYazisi(ref requestBody, false);
            }
            else if (type == Statics.Enums.SyncTypes.YetkiYazisiSil)
            {
                reportid = ProcessYetkiYazisi(ref requestBody, true);
            }
            else
            {
                var resp = await processGeneralOperations(requestBody, type).ConfigureAwait(false);
                res = resp.res;
                reportid = resp.basvuru_id;
            }


            await busAsyncNew.UpdateSyncDataState(syncid, res ? 1 : -2, reportid).ConfigureAwait(false);

            return res;
        }
        public static async Task<(bool res, int basvuru_id)> processGeneralOperations(string requestBody, Statics.Enums.SyncTypes type)
        {
            bool res = true;
            coInvexReportStateChange obj = new coInvexReportStateChange();

            try
            {
                string rawRequest = RemoveFileContentFromResponse(requestBody);

                if (CheckrespIsOk(rawRequest))
                {
                    obj = GetObjectFromResult<coInvexReportStateChange>(rawRequest);
                    if (obj != null)
                    {
                        if (type == Statics.Enums.SyncTypes.UzmanaAtandi)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);
                            JObject basvuru = (JObject)data["basvuru"];

                            var resBasvuru = await processBasvuru(basvuru, obj, Statics.Enums.BasvuruProcessTypes.UzmanaAtandi);

                            if (!resBasvuru.res)
                            {
                                res = false;
                            }

                            obj.basvuru_kod = resBasvuru.basvuru_id.ToString();

                            if (res && data.ContainsKey("tapu_bilgi"))
                            {
                                bool resTapu = await tapuOperations((JArray)data["tapu_bilgi"]).ConfigureAwait(false);
                                if (!resTapu)
                                {
                                    res = false;
                                }
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.AsamaDegistir)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, 0,
                                            obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);
                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.BirimDegistir)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);
                            int uzman = Convert.ToInt32(data["uzman"]);
                            int banka = Convert.ToInt32(data["banka"]);

                            var retVal = await busAsyncNew.UpdateBirimInvexSync(uzman, Convert.ToInt32(obj.basvuru_kod), banka).ConfigureAwait(false); //await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, 0, obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);
                            if (!retVal.res)
                            {
                                res = false;
                            }
                            else
                            {
                                if (retVal.userid > 0)
                                {
                                    string sirket_rapor_no = retVal.reportcode;
                                    await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportAssignedUser, sirket_rapor_no, "UserID_" + retVal.userid, 0, sirket_rapor_no, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                                }
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.UzmanRed)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, -1,
                                            obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);
                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportUzmanIade, "", "UserID_" + retVal.operasyonuserid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.DenetmenIade)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, 0,
                                            obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportDenetmenIade, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepIptal)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 0, 1, 0, 0,
                                                obj.info, (obj.iptal_gerekce == null ? "" : obj.iptal_gerekce) + " - " + (obj.iptal_aciklamasi == null ? "" : obj.iptal_aciklamasi),
                                                obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepIptal, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.IptalGeriAl)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, 0,
                                    obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportIptalGeriAl, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepSil)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 1, -1,
                                        obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepSil, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepSilGeriAl)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, 0,
                                        obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepSilGeriAl, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepDondur)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 0, 1, 0,
                                        obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepDondur, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepCoz)
                        {
                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 1, 0,
                                        obj.info, null, obj.basvuru_rapor, obj.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepCoz, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.BasvuruKaydet)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            if (data.ContainsKey("basvuru"))
                            {
                                JObject basvuru = (JObject)data["basvuru"];

                                var resBasvuru = await processBasvuru(basvuru, obj, Statics.Enums.BasvuruProcessTypes.Basvuru).ConfigureAwait(false);
                                res = resBasvuru.res;

                                obj.basvuru_kod = resBasvuru.basvuru_id.ToString();
                            }
                            else if (data.ContainsKey("basvuru_banka"))
                            {
                                JObject basvuru = (JObject)data["basvuru_banka"];

                                var resBasvuru = await processBasvuru(basvuru, obj, Statics.Enums.BasvuruProcessTypes.Basvuru_Banka).ConfigureAwait(false);
                                res = resBasvuru.res;

                                obj.basvuru_kod = resBasvuru.basvuru_id.ToString();
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.KonumKaydet)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            if (data.ContainsKey("basvuru"))
                            {
                                JObject basvuru = (JObject)data["basvuru"];

                                if (basvuru != null)
                                {
                                    var resBasvuru = await processBasvuru(basvuru, obj, Statics.Enums.BasvuruProcessTypes.Konum).ConfigureAwait(false);
                                    res = resBasvuru.res;

                                    obj.basvuru_kod = resBasvuru.basvuru_id.ToString();
                                }
                            }
                            else if (data.ContainsKey("basvuru_banka"))
                            {
                                JObject basvuru_banka = GetChildFromData(rawRequest, "basvuru_banka");

                                if (basvuru_banka != null)
                                {
                                    var resBasvuru = await processBasvuru(basvuru_banka, obj, Statics.Enums.BasvuruProcessTypes.Konum_Banka).ConfigureAwait(false);
                                    res = resBasvuru.res;

                                    obj.basvuru_kod = resBasvuru.basvuru_id.ToString();
                                }
                            }

                            /*JObject basvuru = GetChildFromData(rawRequest, "basvuru");
                            JObject basvuru_banka = GetChildFromData(rawRequest, "basvuru_banka");

                            if (basvuru != null)
                            {
                                var resBasvuru = await processBasvuru(basvuru, obj, Statics.Enums.BasvuruProcessTypes.Konum).ConfigureAwait(false);
                                res = resBasvuru.res;

                                obj.basvuru_kod = resBasvuru.basvuru_id.ToString();
                            }
                            else if (basvuru_banka != null)
                            {
                                var resBasvuru = await processBasvuru(basvuru_banka, obj, Statics.Enums.BasvuruProcessTypes.Konum_Banka).ConfigureAwait(false);
                                res = resBasvuru.res;

                                obj.basvuru_kod = resBasvuru.basvuru_id.ToString();
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TapuOzellikKaydet)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);
                            if (data["tapu"] != null && data["tapu_ozellik"] != null)
                            {
                                JObject tapu = (JObject)data["tapu"];
                                JObject tapu_ozellik = (JObject)data["tapu_ozellik"];

                                var resObj = await tapuOzellikOperations(tapu, tapu_ozellik).ConfigureAwait(false);
                                res = resObj.res;

                                obj.basvuru_kod = resObj.basvuru_id.ToString();

                            }
                            else if (data["anatasinmaz_tapu_banka"] != null)
                            {
                                //burada gelen alanlar hem tapu sicil hem tapu ozelliklerine gidiyor olabilir, bakilacak
                                JObject tapu = (JObject)data["anatasinmaz_tapu_banka"];

                                var resObj = await tapuOzellikOperations(tapu, null).ConfigureAwait(false);
                                res = resObj.res;

                                obj.basvuru_kod = resObj.basvuru_id.ToString();
                            }
                            else if (data["tapu_ozellik_banka"] != null)
                            {
                                JObject tapu_ozellik = (JObject)data["tapu_ozellik_banka"];

                                var resObj = await tapuOzellikOperations(null, tapu_ozellik).ConfigureAwait(false);
                                res = resObj.res;

                                obj.basvuru_kod = resObj.basvuru_id.ToString();
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TakbisTapuGuncellendi)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);
                            JObject tapu = (JObject)data["tapu_bilgi"];

                            var resObj = await tapuOzellikOperations(tapu, null).ConfigureAwait(false);
                            res = resObj.res;

                            obj.basvuru_kod = resObj.basvuru_id.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.AnaTasinmazKaydet)
                        {
                            JObject tapuAnaTasinmaz = GetChildFromData(rawRequest, "tapu_anatasinmaz_vasfi");

                            if (tapuAnaTasinmaz == null)
                            {
                                tapuAnaTasinmaz = GetChildFromData(rawRequest, "tapu_anatasinmaz_vasfi_banka");
                            }

                            var resObj = await anaTasinmazOperations(tapuAnaTasinmaz).ConfigureAwait(false);
                            res = resObj.res;

                            obj.basvuru_kod = resObj.basvuru_id.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.TapuDegerlemeKaydet)
                        {
                            JObject tapuDegerleme = GetChildFromData(rawRequest, "tapu_degerleme");

                            if (tapuDegerleme == null)
                            {
                                tapuDegerleme = GetChildFromData(rawRequest, "tapu_degerleme_banka");
                            }

                            if (tapuDegerleme == null)
                            {
                                tapuDegerleme = GetChildFromData(rawRequest, "tapu_degerleme_genel");
                            }

                            var resObj = await degerlemeOperations(tapuDegerleme).ConfigureAwait(false);
                            res = resObj.res;

                            obj.basvuru_kod = resObj.basvuru_id.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.TapuSicilKaydet)
                        {
                            JObject tapu = GetChildFromData(rawRequest, "tapu");
                            if (tapu == null)
                            {
                                tapu = GetChildFromData(rawRequest, "tapu_banka");
                            }

                            var resObj = await tapuOzellikOperations(tapu, null).ConfigureAwait(false);
                            res = resObj.res;

                            obj.basvuru_kod = resObj.basvuru_id.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.BasvuruRevizyonKaydet)
                        {

                            JObject rev = GetChildFromData(rawRequest, "revizyon");

                            coInvexRevizyon revizyon = rev.ToObject<coInvexRevizyon>();

                            var resrev = await busAsyncNew.CreateOrUpdateBasvuruRevizyon(obj.sirket_id, Convert.ToInt32(revizyon.kod), Convert.ToInt32(revizyon.basvuru_kod),
                                DateTime.ParseExact(revizyon.revizyon_tarihi, "dd.MM.yyyy", null), revizyon.revizyon_gerekcesi, revizyon.revizyon_gerekcesi_aciklama,
                                revizyon.revizyon_sonucu, revizyon.revizyon_sonucu_aciklama, revizyon.not_sonuc_ekle, revizyon.sonuc,
                                revizyon.sonuc_aciklama_banka, revizyon.sonuc_aciklama_sirket, revizyon.revizyon_notu).ConfigureAwait(false);

                            if (resrev.userid > 0)
                            {
                                res = resrev.res;
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportRevizyonEklendi, resrev.reportcode, "UserID_" + resrev.userid, 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.BelgelerYuklendi)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int kod = Convert.ToInt32(data["kod"].ToString());
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());
                            int kullanici = Convert.ToInt32(data["kullanici"].ToString());
                            string adi = data["adi"].ToString();
                            string tarih = data["tarih"].ToString();
                            string dosyatipi = data["dosyatipi"].ToString();
                            string tipi = data["tipi"].ToString();
                            string orjadi = data["orjadi"].ToString();
                            int size = Convert.ToInt32(data["size"].ToString());

                            bool ins = await busAsyncNew.CreateUpdateInvexReportFile(basvuru_kod, kod, kullanici, 0, 0, tipi, dosyatipi, adi, orjadi, tarih, size);

                            if (tipi == "TAKBİS")
                            {
                                (bool, int) upd = await busAsyncNew.UpdateReportTakbis(basvuru_kod).ConfigureAwait(false);
                                await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTakbisAlindi, "", "UserID_" + upd.Item2, 0, basvuru_kod.ToString(), basvuru_kod).ConfigureAwait(false);
                            }

                            res = ins;
                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepBelgebilgiGuncelle)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int kod = Convert.ToInt32(data["kod"].ToString());
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());
                            string tipi = data["tipi"].ToString();

                            bool upd = await busAsyncNew.UpdateInvexReportFile(kod, tipi);

                            res = upd;
                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepBelgeSil)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int kod = Convert.ToInt32(data["kod"].ToString());
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());

                            bool del = await busAsyncNew.DeleteInvexReportFile(kod);

                            res = del;
                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.FotoYukle)
                        {
                            obj = GetObjectFromResultWithoutData<coInvexReportStateChange>(rawRequest);
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int kod = Convert.ToInt32(data["kod"].ToString());
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());
                            int kullanici = Convert.ToInt32(data["kullanici"].ToString());
                            string adi = data["adi"].ToString();
                            string tarih = data["tarih"].ToString();
                            string dosyatipi = data["dosyatipi"].ToString();
                            //string orjadi = data["orjadi"].ToString();
                            int size = Convert.ToInt32(data["size"].ToString());
                            string fotograf_turu = data["fotograf_turu"].ToString();

                            int typeid = 0;
                            int subtypeid = 0;
                            int iskapak = 0;

                            string[] tasinmaztypes = new string[] { "İÇ MEKAN", "EKSPER ÖZÇEKİM", "DIŞ GÖRÜNÜŞ (KONUM)", "DIŞ MEKAN" };

                            if (tasinmaztypes.Contains(fotograf_turu))
                            {
                                if (fotograf_turu == "İÇ MEKAN")
                                {
                                    subtypeid = 1;
                                }
                                else if (fotograf_turu == "EKSPER ÖZÇEKİM")
                                {
                                    subtypeid = 5;
                                }
                                else
                                {
                                    subtypeid = 2;
                                }
                                typeid = 1;
                            }
                            else
                            {
                                if (fotograf_turu.Contains("BELEDİYE"))
                                {
                                    subtypeid = 4;
                                }
                                else
                                {
                                    subtypeid = 3;
                                }
                                typeid = 2;
                            }

                            if (fotograf_turu == "KAPAK FOTOĞRAFI")
                            {
                                iskapak = 1;
                            }

                            DateTime dateFoto = DateTime.ParseExact(tarih, "yyyy-MM-dd HH:mm:ss", null);

                            bool ins = await busAsyncNew.CreateUpdateInvexReportFoto(kod, basvuru_kod, kullanici, typeid, Guid.NewGuid().ToString(),
                                dateFoto, dateFoto.Ticks.ToString(), subtypeid, iskapak, 1, fotograf_turu, 1, size).ConfigureAwait(false);

                            res = ins;
                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepFotobilgiGuncelle)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int kod = Convert.ToInt32(data["kod"].ToString());
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());

                            bool upd = await busAsyncNew.UpdateInvexReportFoto(kod).ConfigureAwait(false);

                            res = upd;
                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepFotoSil)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int kod = Convert.ToInt32(data["kod"].ToString());
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());

                            bool del = await busAsyncNew.DeleteInvexReportFoto(kod, basvuru_kod).ConfigureAwait(false);

                            res = del;
                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                        else if (type == Statics.Enums.SyncTypes.IleriGeri)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);
                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());

                            coInvexRaporAdim geldigi_adim = data["geldigi_adim"].ToObject<coInvexRaporAdim>();
                            coInvexRaporAdim gittigi_adim = data["gittigi_adim"].ToObject<coInvexRaporAdim>();

                            var retVal = await busAsyncNew.UpdateReportInvexState(obj.sirket_id, Convert.ToInt32(obj.basvuru_kod), 1, 1, 0, 0,
                                        obj.info, gittigi_adim.adim, gittigi_adim.basvuru_rapor, gittigi_adim.birimi).ConfigureAwait(false);

                            if (retVal.res == 0)
                            {
                                res = false;
                            }
                            else
                            {
                                if (geldigi_adim.sira == 4 && gittigi_adim.sira == 3)
                                {
                                    await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportUzmanIade, "", "UserID_" + retVal.operasyonuserid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                                }
                                else if (geldigi_adim.sira == 5 && gittigi_adim.sira == 4)
                                {
                                    await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportDenetmenIade, "", "UserID_" + retVal.uzmanid.ToString(), 0, obj.basvuru_kod, Convert.ToInt32(obj.basvuru_kod)).ConfigureAwait(false);
                                }
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.EmsalGuncelleKaydet)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            if (data["emsal"] != null)
                            {
                                JObject emsal = (JObject)data["emsal"];

                                var resObj = await emsalOperations(emsal).ConfigureAwait(false);
                                res = resObj.res;

                                obj.basvuru_kod = resObj.basvuru_id.ToString();
                            }
                            else if (data["banka_emsal"] != null)
                            {
                                JObject banka_emsal = (JObject)data["banka_emsal"];

                                var resObj = await emsalOperations(banka_emsal).ConfigureAwait(false);
                                res = resObj.res;

                                obj.basvuru_kod = resObj.basvuru_id.ToString();
                            }
                        }
                        else if (type == Statics.Enums.SyncTypes.TalepNotEkle)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);
                            JObject not_bilgi = (JObject)data["not_bilgi"];

                            int kullanici = Convert.ToInt32(not_bilgi["kullanici"].ToString());
                            int basvuru_kod = Convert.ToInt32(not_bilgi["basvuru_kod"].ToString());
                            string tipi = not_bilgi["tip"].ToString();
                            string islemtarih = not_bilgi["islemtarih"].ToString();

                            DateTime date = DateTime.ParseExact(islemtarih, "yyyy-MM-dd HH:mm:ss", null);

                            string SD_gorsun = not_bilgi["SD_gorsun"].ToString();
                            string SU_gorsun = not_bilgi["SU_gorsun"].ToString();
                            string SDU_gorsun = not_bilgi["SDU_gorsun"].ToString();
                            string aciklama = not_bilgi["aciklama"].ToString();

                            string username = "";

                            if (not_bilgi["kullaniciadi"] != null)
                            {
                                username = not_bilgi["kullaniciadi"].ToString();
                            }

                            int sd = 0;
                            int su = 0;
                            int sdu = 0;

                            if (!string.IsNullOrEmpty(SD_gorsun) && SD_gorsun == "E")
                            {
                                sd = 1;
                            }

                            if (!string.IsNullOrEmpty(SU_gorsun) && SU_gorsun == "E")
                            {
                                su = 1;
                            }


                            if (!string.IsNullOrEmpty(SDU_gorsun) && SDU_gorsun == "E")
                            {
                                sdu = 1;
                            }

                            string[] textList = new string[] { "GENEL NOTLAR", "GÖRÜŞME", "RANDEVU", "RESMİ KURUM ARAŞTIRMALARI (TAPU)", "RESMİ KURUM ARAŞTIRMALARI (BELEDİYE)",
                                "RESMİ KURUM ARAŞTIRMALARI (DİĞER)", "YER GÖRME", "EMSAL ARAŞTIRMALARI", "RAPOR YAZILIYOR", "GECİKME BİLDİRİMİ",
                                "İPTAL BİLDİRİMİ", "ÇÖZME BİLDİRİMİ", "BANKA YAZIŞMALARI", "MUHASEBE", "REVİZYON", "PROJE", "DİĞER" };

                            List<string> tiplist = new();
                            tiplist.AddRange(new string[] { "N", "G", "R", "T", "B", "K", "Y", "E", "S", "C", "Q", "H", "Z", "M", "V", "P", "D" });

                            string text = textList[tiplist.IndexOf(tipi)];

                            if (!string.IsNullOrEmpty(aciklama))
                            {
                                text = aciklama;
                            }

                            //res = await busAsyncNew.CreateReportNoteByService(kullanici, basvuru_kod, tipi, text, 0).ConfigureAwait(false);
                            res = await busAsyncNew.CreateReportNoteByServiceNew(kullanici, basvuru_kod, tipi, text, 0,
                                sd, su, sdu, tipi, date.Ticks, username).ConfigureAwait(false);

                            obj.basvuru_kod = basvuru_kod.ToString();

                        }
                        else if (type == Statics.Enums.SyncTypes.MasrafKaydet)
                        {
                            JObject data = GetDataContentFromResponse(rawRequest);

                            int basvuru_kod = Convert.ToInt32(data["basvuru_kod"].ToString());
                            int kullanici = 0;
                            int resourcetype = 0;
                            int invexid = 0;

                            float degerleme_bedeli = Utility.ConvertFloat(data["degerleme_bedeli"]);
                            float degerleme_bedeli_kdv = Utility.ConvertFloat(data["degerleme_bedeli_kdv"]);
                            float degerleme_bedeli_toplam = Utility.ConvertFloat(data["degerleme_bedeli_toplam"]);
                            float resmi_harc_tapu = Utility.ConvertFloat(data["resmi_harc_tapu"]);
                            float resmi_harc_tapu_kdv = Utility.ConvertFloat(data["resmi_harc_tapu_kdv"]);
                            float resmi_harc_tapu_toplam = Utility.ConvertFloat(data["resmi_harc_tapu_toplam"]);
                            float resmi_harc_belediye = Utility.ConvertFloat(data["resmi_harc_belediye"]);
                            float resmi_harc_belediye_kdv = Utility.ConvertFloat(data["resmi_harc_belediye_kdv"]);
                            float resmi_harc_belediye_toplam = Utility.ConvertFloat(data["resmi_harc_belediye_toplam"]);
                            float ulasim_bedeli = Utility.ConvertFloat(data["ulasim_bedeli"]);
                            float ulasim_bedeli_kdv = Utility.ConvertFloat(data["ulasim_bedeli_kdv"]);
                            float ulasim_bedeli_toplam = Utility.ConvertFloat(data["ulasim_bedeli_toplam"]);
                            float konaklama_bedeli = Utility.ConvertFloat(data["konaklama_bedeli"]);
                            float konaklama_bedeli_kdv = Utility.ConvertFloat(data["konaklama_bedeli_kdv"]);
                            float konaklama_bedeli_toplam = Utility.ConvertFloat(data["konaklama_bedeli_toplam"]);
                            float diger_harcamalar = Utility.ConvertFloat(data["diger_harcamalar"]);
                            float diger_harcamalar_kdv = Utility.ConvertFloat(data["diger_harcamalar_kdv"]);
                            float diger_harcamalar_toplam = Utility.ConvertFloat(data["diger_harcamalar_toplam"]);

                            res = await busAsyncNew.CreateMasraf(basvuru_kod, kullanici, resourcetype,
                                degerleme_bedeli, degerleme_bedeli_kdv, degerleme_bedeli_toplam,
                                resmi_harc_tapu, resmi_harc_tapu_kdv, resmi_harc_tapu_toplam,
                                resmi_harc_belediye, resmi_harc_belediye_kdv, resmi_harc_belediye_toplam,
                                ulasim_bedeli, ulasim_bedeli_kdv, ulasim_bedeli_toplam,
                                konaklama_bedeli, konaklama_bedeli_kdv, konaklama_bedeli_toplam,
                                diger_harcamalar, diger_harcamalar_kdv, diger_harcamalar_toplam,
                                invexid, "");

                            obj.basvuru_kod = basvuru_kod.ToString();
                        }
                    }
                }

            }
            catch (Exception ex)
            {

            }

            return (res, string.IsNullOrEmpty(obj.basvuru_kod) ? 0 : Convert.ToInt32(obj.basvuru_kod));
        }
        static List<coInvexSyncFieldSubPartCodes> createKonumValues(string konum, coReport report)
        {
            List<coInvexSyncFieldSubPartCodes> retVal = new List<coInvexSyncFieldSubPartCodes>();

            if (string.IsNullOrEmpty(konum))
            {
                konum = report.ReportTasinmazLocation;
            }

            if (!string.IsNullOrEmpty(konum))
            {
                string[] enlemboylam = konum.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                string pozisyon_n = "";
                string pozisyon_e = "";

                if (enlemboylam.Length > 3)
                {
                    pozisyon_n = enlemboylam[0] + "," + enlemboylam[1];
                    pozisyon_e = enlemboylam[2] + "," + enlemboylam[3];
                }
                else if (enlemboylam.Length > 1)
                {
                    pozisyon_n = enlemboylam[0];
                    pozisyon_e = enlemboylam[1];
                }

                if (pozisyon_n.Length < 9)
                {
                    pozisyon_n += "0";
                }

                if (pozisyon_e.Length < 9)
                {
                    pozisyon_e += "0";
                }

                coInvexSyncFieldSubPartCodes f_pozisyon_n = new coInvexSyncFieldSubPartCodes();
                f_pozisyon_n.FieldSysCode = "pozisyon_n";
                f_pozisyon_n.ReportSubPartUserFieldValue = pozisyon_n;

                retVal.Add(f_pozisyon_n);

                coInvexSyncFieldSubPartCodes f_pozisyon_e = new coInvexSyncFieldSubPartCodes();
                f_pozisyon_e.FieldSysCode = "pozisyon_e";
                f_pozisyon_e.ReportSubPartUserFieldValue = pozisyon_e;

                retVal.Add(f_pozisyon_e);
            }

            return retVal;
        }
        public static async Task<(bool res, string progress)> processSyncReportToInvex(coInvexSyncReport reportSyncLog, coTemplate template)
        {
            string progress = "";
            bool res = false;

            string invexCode = "";
            string invexToken = "";

            coTemplateSDO formData = await busAsyncNew.GetReportFormData_New(0, reportSyncLog.ReportID).ConfigureAwait(false);
            coTemplate templateFormData = formData.convertToTemplate();
            coReport report = await busAsyncNew.GetReportByReportID(0, (int)Statics.Enums.UserRoles.Admin, reportSyncLog.ReportID, reportSyncLog.OrganizationID).ConfigureAwait(false);

            List<coUser> userlist = await busAsyncNew.GetUserByUserID(report.ReportAssignedUserID).ConfigureAwait(false);
            coUser uzman = userlist != null && userlist.Count > 0 ? userlist.FirstOrDefault() : new coUser();

            templateFormData = SharedUtil.processFormData(templateFormData, template);

            List<coInvexSyncFieldSubPartCodes> temel_bilgi = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "temel_bilgiler").ConfigureAwait(false);

            invexCode = temel_bilgi[0].OrganizationInvexCode;
            invexToken = temel_bilgi[0].OrganizationInvexToken;

            int a = 0;

            if (true)
            {
                #region Temel Bilgi

                bool temelbilgi_isok = await createDataSync(report, templateFormData, temel_bilgi, "basvuru", false,
                    "basvuru", "temel-bilgi/guncelle", null, 0, 0, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);

                if (temelbilgi_isok)
                {
                    progress += "[temel_bilgi_ok]";
                }
                else
                {
                    progress += "[temel_bilgi_error]";
                }

                if (reportSyncLog.OrganizationID != 1 || true)
                {
                    bool temelbilgi_banka_isok = await createDataSync(report, templateFormData, temel_bilgi, "basvuru_banka", true,
                        "basvuru", "temel-bilgi/guncelle", null, 0, 0, 0,
                        uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);

                    if (temelbilgi_banka_isok)
                    {
                        progress += "[temelbilgi_banka_ok]";
                    }
                    else
                    {
                        progress += "[temelbilgi_banka_error]";
                    }
                }

                #endregion Temel Bilgi
                a = 1;
                #region Konum

                List<coInvexSyncFieldSubPartCodes> konum_bilgi = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "konum_bilgi").ConfigureAwait(false);
                List<coInvexSyncFieldSubPartCodes> newVals = new List<coInvexSyncFieldSubPartCodes>();

                foreach (coInvexSyncFieldSubPartCodes field in konum_bilgi)
                {
                    if (field.FieldSysCode == "pozisyon")
                    {
                        newVals.AddRange(createKonumValues(field.ReportSubPartUserFieldValue, report));
                    }
                }

                konum_bilgi.AddRange(newVals);


                bool konum_isok = await createDataSync(report, templateFormData, konum_bilgi, "basvuru", false,
                    "basvuru", "temel-bilgi/konum-kaydet", new List<string>() { "pozisyon" }, 0, 0, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);

                if (konum_isok)
                {
                    progress += "[konum_ok]";
                }
                else
                {
                    progress += "[konum_error]";
                }

                if (reportSyncLog.OrganizationID != 1 || true)
                {
                    bool konumbanka_isok = await createDataSync(report, templateFormData, konum_bilgi, "basvuru_banka", true,
                        "basvuru", "temel-bilgi/konum-kaydet", new List<string>() { "pozisyon" }, 0,
                        konum_bilgi[0].ReportSysID, 0,
                        uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);

                    if (konumbanka_isok)
                    {
                        progress += "[konumbanka_ok]";
                    }
                    else
                    {
                        progress += "[konumbanka_error]";
                    }
                }

                #endregion Konum
                a = 2;
                #region Tapu Sicil

                if (true)
                {
                    List<coInvexSyncFieldSubPartCodes> tapuSicilList = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "tapu_sicil_bilgi").ConfigureAwait(false);

                    bool tapusicil_isok = await createTapuSicil(report, tapuSicilList, templateFormData, false, uzman);

                    if (tapusicil_isok)
                    {
                        progress += "[tapusicil_ok]";
                    }
                    else
                    {
                        progress += "[tapusicil_error]";
                    }

                    bool tapusicil_banka_isok = await createTapuSicil(report, tapuSicilList, templateFormData, true, uzman);

                    if (tapusicil_banka_isok)
                    {
                        progress += "[tapusicil_banka_ok]";
                    }
                    else
                    {
                        progress += "[tapusicil_banka_error]";
                    }
                }

                List<coInvexSyncFieldSubPartCodes> tapuSicilGenel = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "tapu_sicil_genel").ConfigureAwait(false);

                bool tapu_sicil_genel_isok = await createDataSync(report, templateFormData, tapuSicilGenel, "tapu_banka",
                    true, "basvuru", "temel-bilgi/tapu-sicil-genel-kaydet", null, 0, 0, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);
                if (tapu_sicil_genel_isok)
                {
                    progress += "[tapusicilgenel_ok]";
                }
                else
                {
                    progress += "[tapusicilgenel_error]";
                }

                #endregion Tapu Sicil
                a = 3;
                #region Tapu Fiziki

                List<coInvexSyncFieldSubPartCodes> tapuFizikiList = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "tapu_ozellikleri").ConfigureAwait(false);

                if (true)
                {

                    bool tapufiziki_ozellikleri_isok = await createTapuFiziki(report, tapuFizikiList, templateFormData,
                        false, uzman);

                    if (tapufiziki_ozellikleri_isok)
                    {
                        progress += "[tapufiziki_ozellikleri_ok]";
                    }
                    else
                    {
                        progress += "[tapufiziki_ozellikleri_error]";
                    }
                }


                bool tapufiziki_ozellikleri_banka_isok = await createTapuFiziki(report, tapuFizikiList, templateFormData,
                    true, uzman);

                if (tapufiziki_ozellikleri_banka_isok)
                {
                    progress += "[tapufiziki_ozellikleri_banka_ok]";
                }
                else
                {
                    progress += "[tapufiziki_ozellikleri_banka_error]";
                }

                #endregion Tapu Fiziki
                a = 4;
                #region Degerleme

                List<coInvexSyncFieldSubPartCodes> tapuDegerleme = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "tapu_degerleme").ConfigureAwait(false);

                bool tapudegerleme_isok = await createDegerleme(report, tapuDegerleme, templateFormData, false, false, uzman).ConfigureAwait(false);
                if (tapudegerleme_isok)
                {
                    progress += "[tapudegerleme_ok]";
                }
                else
                {
                    progress += "[tapudegerleme_error]";
                }

                bool tapudegerleme_banka_isok = await createDegerleme(report, tapuDegerleme, templateFormData, true, false, uzman).ConfigureAwait(false);

                if (tapudegerleme_banka_isok)
                {
                    progress += "[tapudegerleme_banka_ok]";
                }
                else
                {
                    progress += "[tapudegerleme_banka_error]";
                }

                List<coInvexSyncFieldSubPartCodes> tapuDegerlemeGenel = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "tapu_degerleme_genel").ConfigureAwait(false);

                bool tapudegerleme_genel = await createDegerleme(report, tapuDegerlemeGenel, templateFormData, true, true, uzman).ConfigureAwait(false);

                if (tapudegerleme_genel)
                {
                    progress += "[tapudegerleme_genel_ok]";
                }
                else
                {
                    progress += "[tapudegerleme_genel_error]";
                }

                #endregion Degerleme
                a = 5;
                #region Ana Taşınmaz

                List<coInvexSyncFieldSubPartCodes> anaTasinmaz = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "tapu_anatasinmaz_vasfi").ConfigureAwait(false);

                bool anatasinmaz_isok = await createDataSync(report, templateFormData, anaTasinmaz, "tapu_anatasinmaz_vasfi",
                    false, "basvuru", "temel-bilgi/tapu-fiziki-ozellik-genel-kaydet", null, 0, 0, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);
                if (anatasinmaz_isok)
                {
                    progress += "[anatasinmaz_ok]";
                }
                else
                {
                    progress += "[anatasinmaz_error]";
                }

                if (reportSyncLog.OrganizationID != 1 || true)
                {
                    bool anatasinmaz_banka_isok = await createDataSync(report, templateFormData, anaTasinmaz,
                        "tapu_anatasinmaz_vasfi_banka", true, "basvuru", "temel-bilgi/tapu-fiziki-ozellik-genel-kaydet",
                        null, 0, 0, 0,
                        uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);

                    if (anatasinmaz_banka_isok)
                    {
                        progress += "[anatasinmaz_banka_ok]";
                    }
                    else
                    {
                        progress += "[anatasinmaz_banka_error]";
                    }
                }

                #endregion Ana Taşınmaz
                a = 6;
                #region Emsal

                List<coInvexSyncFieldSubPartCodes> emsal = await busAsyncNew.GetReportSubPartSyncData(reportSyncLog.ReportID, "emsal").ConfigureAwait(false);
                bool emsal_isok = await createEmsal(report, emsal, templateFormData, false, uzman).ConfigureAwait(false);

                if (emsal_isok)
                {
                    progress += "[emsal_ok]";
                }
                else
                {
                    progress += "[emsal_error]";
                }

                if (reportSyncLog.OrganizationID != 1)
                {
                    bool emsal_banka_isok = await createEmsal(report, emsal, templateFormData, true, uzman).ConfigureAwait(false);

                    if (emsal_banka_isok)
                    {
                        progress += "[emsal_banka_ok]";
                    }
                    else
                    {
                        progress += "[emsal_banka_error]";
                    }
                }

                #endregion Emsal
                a = 7;

                a = 8;
            }

            #region Photos

            bool hasAnyError = false;

            List<coReportFile> fileList = await busAsyncNew.GetSyncReportFilesByReportID(reportSyncLog.ReportID).ConfigureAwait(false);

            foreach (coReportFile file in fileList)
            {
                var kod = await sendImageToInvex(file, invexCode, invexToken, reportSyncLog.ReportSysID, uzman.UserInvexToken, "127.0.0.1", "system").ConfigureAwait(false);

                if (kod.Item1 > 0)
                {
                    await busAsyncNew.UpdateReportFileSyncState(file.ReportFileID, 1, kod.Item1).ConfigureAwait(false);
                }
                else
                {
                    hasAnyError = true;
                    await busAsyncNew.UpdateReportFileSyncState(file.ReportFileID, 1, 1).ConfigureAwait(false);
                }

                /*
                try
                {
                    if (file.ReportFileUseInReport == 1 && file.ReportFileSourceType == 0)
                    {
                        JObject fileData = getRootObject(invexCode, invexToken);

                        fileData["yer"] = "F";

                        if (file.ReportFileIsKapak == 1)
                        {
                            fileData["fotograf_turu"] = "KAPAK FOTOĞRAFI";
                        }
                        else
                        {
                            string[] fototurnames = new string[] { "İÇ MEKAN", "DIŞ MEKAN", "RESMİ BELGE", "RESMİ BELGE", "EKSPER ÖZÇEKİM" };

                            if (file.ReportFileTypeID == 1 || file.ReportFileTypeID == 2)
                            {
                                if (file.ReportFileSubType > 0 && file.ReportFileSubType < 6)
                                {
                                    fileData["fotograf_turu"] = fototurnames[file.ReportFileSubType - 1];
                                }
                                else
                                {
                                    fileData["fotograf_turu"] = "DİĞER";
                                }
                            }
                            else if (file.ReportFileTypeID == 4)
                            {
                                fileData["fotograf_turu"] = "TAŞINMAZ HARİTA";
                            }
                        }

                        fileData["dosya_foto_adi"] = file.ReportFileGuid + ".jpg";
                        fileData["basvuru_kod"] = reportSyncLog.ReportSysID.ToString();
                        if (file.ReportFileSysID > 0)
                        {
                            fileData["dosya_kod"] = file.ReportFileSysID.ToString();
                        }
                        fileData["kullanici"] = file.UserInvexID.ToString();

                        string uri = Statics.SystemValues.blobUrl + file.ReportFileGuid + ".jpg";

                        HttpClient client = new HttpClient();
                        byte[] response = await client.GetByteArrayAsync(uri);

                        string base64String = Convert.ToBase64String(response, 0, response.Length);

                        fileData["dosya_foto"] = base64String;

                        try
                        {
                            RestResponse fotoResp = await ServiceUtility.getPostDataResponse("basvuru", "temel-bilgi/dosya-foto-yukle", fileData);

                            var result = GetResFromResponse(fotoResp, "dosya_kod");

                            if (result.issuccess)
                            {
                                await busAsyncNew.UpdateReportFileSyncState(file.ReportFileID, 1, result.kod > 0 ? result.kod : file.ReportFileSysID).ConfigureAwait(false);
                            }
                            else
                            {
                                hasAnyError = true;
                            }
                        }
                        catch (Exception ex)
                        {
                            //hasAnyError = true;

                            await busAsyncNew.UpdateReportFileSyncState(file.ReportFileID, 1, 1).ConfigureAwait(false);
                        }


                    }

                }
                catch
                {

                }
            }

            if (!hasAnyError)
            {
                progress += "[foto_ok]";
            }
            else
            {
                progress += "[foto_error]";
            }

            #endregion Photos

            res = true;


            return (res, progress);
        }
        public static async Task<(int, string)> sendImageToInvex(coReportFile file, string invexCode, string invexToken,
            int reportSysID, string invexUserToken, string ip, string device, bool forcetoSend = false, byte[] filecontent = null)
        {
            try
            {
                JObject fileData = getRootObject(invexCode, invexToken, invexUserToken);

                fileData["ip"] = ip;
                fileData["cihaz"] = device;

                if ((file.ReportFileUseInReport == 1 && file.ReportFileSourceType == 0) || forcetoSend)
                {
                    fileData["yer"] = "F";

                    if (file.ReportFileIsKapak == 1)
                    {
                        fileData["fotograf_turu"] = "KAPAK FOTOĞRAFI";
                    }
                    else
                    {
                        string[] fototurnames = new string[] { "İÇ MEKAN", "DIŞ MEKAN", "RESMİ BELGE", "RESMİ BELGE", "EKSPER ÖZÇEKİM" };

                        if (file.ReportFileTypeID == 1 || file.ReportFileTypeID == 2)
                        {
                            if (file.ReportFileSubType > 0 && file.ReportFileSubType < 6)
                            {
                                fileData["fotograf_turu"] = fototurnames[file.ReportFileSubType - 1];
                            }
                            else
                            {
                                fileData["fotograf_turu"] = "DİĞER";
                            }
                        }
                        else if (file.ReportFileTypeID == 4)
                        {
                            fileData["fotograf_turu"] = "TAŞINMAZ HARİTA";
                        }
                    }

                    fileData["dosya_foto_adi"] = file.ReportFileGuid + ".jpg";
                    fileData["basvuru_kod"] = reportSysID.ToString();
                    if (file.ReportFileSysID > 0)
                    {
                        fileData["dosya_kod"] = file.ReportFileSysID.ToString();
                    }
                    fileData["kullanici"] = file.UserInvexID.ToString();

                    string base64String = "";

                    if (filecontent == null)
                    {
                        string uri = Statics.SystemValues.blobUrl + file.ReportFileGuid + ".jpg";

                        HttpClient client = new HttpClient();
                        byte[] response = await client.GetByteArrayAsync(uri);

                        base64String = Convert.ToBase64String(response, 0, response.Length);
                    }
                    else
                    {
                        base64String = Convert.ToBase64String(filecontent, 0, filecontent.Length);
                    }

                    fileData["dosya_foto"] = base64String;

                    try
                    {
                        RestResponse fotoResp = await ServiceUtility.getPostDataResponse("basvuru", "temel-bilgi/dosya-foto-yukle", fileData);

                        var result = GetResFromResponse(fotoResp, "dosya_kod");

                        fileData["dosya_foto"] = "";

                        return (result.kod, fileData.ToString());
                        /*if (result.issuccess)
                        {
                            await busAsyncNew.UpdateReportFileSyncState(file.ReportFileID, 1, result.kod > 0 ? result.kod : file.ReportFileSysID).ConfigureAwait(false);
                        }
                        else
                        {
                            hasAnyError = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        fileData["dosya_foto"] = "";
                        return (0, ex.Message + "-" + fileData.ToString());
                        //await busAsyncNew.UpdateReportFileSyncState(file.ReportFileID, 1, 1).ConfigureAwait(false);
                    }
                }

            }
            catch
            {

            }

            return (0, "");
        }

        public static async Task<bool> deleteImageFromInvex(string fileid, string invexCode, string invexToken,
            int reportSysID, string invexUserToken, string ip, string device)
        {
            try
            {
                JObject fileData = getRootObject(invexCode, invexToken, invexUserToken);

                fileData["yer"] = "F";
                fileData["basvuru_kod"] = reportSysID.ToString();
                fileData["kod"] = fileid;
                fileData["ip"] = ip;
                fileData["cihaz"] = device;

                try
                {
                    RestResponse fotoResp = await ServiceUtility.getPostDataResponse("basvuru", "temel-bilgi/dosya-foto-sil", fileData);

                    var result = GetResFromResponse(fotoResp);

                    return result.issuccess;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }

        public static async Task<bool> sendMasrafToInvex(string invexCode, string invexToken,
            int reportSysID, string invexUserToken, string ip, string device,
            string degerleme_bedeli, string degerleme_bedeli_kdv, string degerleme_bedeli_toplam,
            string resmi_harc_tapu, string resmi_harc_tapu_kdv, string resmi_harc_tapu_toplam,
            string resmi_harc_belediye, string resmi_harc_belediye_kdv, string resmi_harc_belediye_toplam,
            string ulasim_bedeli, string ulasim_bedeli_kdv, string ulasim_bedeli_toplam,
            string konaklama_bedeli, string konaklama_bedeli_kdv, string konaklama_bedeli_toplam,
            string diger_harcamalar, string diger_harcamalar_kdv, string diger_harcamalar_toplam)
        {
            try
            {
                JObject masrafData = getRootObject(invexCode, invexToken, invexUserToken);

                masrafData["ip"] = ip;
                masrafData["basvuru_kod"] = reportSysID.ToString();
                masrafData["cihaz"] = device;
                masrafData["info"] = "Masraf Bilgileri Güncellendi.";

                masrafData["degerleme_bedeli"] = degerleme_bedeli;
                masrafData["degerleme_bedeli_kdv"] = degerleme_bedeli_kdv;
                masrafData["degerleme_bedeli_toplam"] = degerleme_bedeli_toplam;

                masrafData["resmi_harc_tapu"] = resmi_harc_tapu;
                masrafData["resmi_harc_tapu_kdv"] = resmi_harc_tapu_kdv;
                masrafData["resmi_harc_tapu_toplam"] = resmi_harc_tapu_toplam;

                masrafData["resmi_harc_belediye"] = resmi_harc_belediye;
                masrafData["resmi_harc_belediye_kdv"] = resmi_harc_belediye_kdv;
                masrafData["resmi_harc_belediye_toplam"] = resmi_harc_belediye_toplam;

                masrafData["ulasim_bedeli"] = ulasim_bedeli;
                masrafData["ulasim_bedeli_kdv"] = ulasim_bedeli_kdv;
                masrafData["ulasim_bedeli_toplam"] = ulasim_bedeli_toplam;

                masrafData["konaklama_bedeli"] = konaklama_bedeli;
                masrafData["konaklama_bedeli_kdv"] = konaklama_bedeli_kdv;
                masrafData["konaklama_bedeli_toplam"] = konaklama_bedeli_toplam;

                masrafData["diger_harcamalar"] = diger_harcamalar;
                masrafData["diger_harcamalar_kdv"] = diger_harcamalar_kdv;
                masrafData["diger_harcamalar_toplam"] = diger_harcamalar_toplam;

                try
                {
                    RestResponse fotoResp = await ServiceUtility.getPostDataResponse("basvuru", "temel-bilgi/masraf-kaydet", masrafData);

                    var result = GetResFromResponse(fotoResp);

                    return result.issuccess;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }

        public static async Task<bool> sendNoteToInvex(string invexCode, string invexToken,
            string reportSysID, string invexUserToken, string ip, string device,
            string userinvexid, string tip, string sd, string su, string aciklama)
        {
            try
            {
                JObject noteData = getRootObject(invexCode, invexToken, invexUserToken);

                noteData["ip"] = ip;
                noteData["cihaz"] = device;
                noteData["kullanici_token"] = invexUserToken;
                noteData["info"] = "Talep Not Eklendi.";
                JObject not_bilgi = new JObject();

                not_bilgi["basvuru_kod"] = reportSysID;
                not_bilgi["kullanici"] = userinvexid;
                not_bilgi["islemtarih"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                not_bilgi["tip"] = tip;
                not_bilgi["SD_gorsun"] = sd;
                not_bilgi["SU_gorsun"] = su;
                not_bilgi["aciklama"] = aciklama;

                noteData["not_bilgi"] = not_bilgi;

                try
                {
                    RestResponse fotoResp = await ServiceUtility.getPostDataResponse("basvuru", "temel-bilgi/not-kaydet", noteData);

                    var result = GetResFromResponse(fotoResp);

                    return result.issuccess;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            catch
            {

            }

            return false;
        }

        public static async Task processReportYazimMail(string htmlTemplate, List<coInvexSyncReport> reportList)
        {
            coTemplate FormData;
            coTemplate defaultTemplate = await busAsyncNew.GetTemplateData(0, 0).ConfigureAwait(false);

            /*string projectRootPath = hostingEnvironment.ContentRootPath;
            projectRootPath = Path.Combine(projectRootPath, "SupportingFiles");
            var filename = Path.Combine(projectRootPath, "emptyreporttemplate.html");

            var htmlTemplate = System.IO.File.ReadAllText(filename);

            int selectedTapuSysID = 0;

            if (reportList != null && reportList.Count > 0)
            {
                foreach (coInvexSyncReport data in reportList)
                {
                    bool res = false;
                    int progress = 0;

                    try
                    {
                        coReport report = await ServiceUtility.busAsyncNew.GetReportByReportID(0, 1, data.ReportID, data.OrganizationID);

                        coTemplate resTemplate = await ServiceUtility.busAsyncNew.GetReportFormDataWithOriginalObjects(0, report.ReportID).ConfigureAwait(false);
                        FormData = SharedUtil.processFormData(resTemplate, defaultTemplate);

                        string reportTemplate = htmlTemplate;

                        string reportBody = "";

                        //@@@ title part
                        if (report != null)
                        {
                            progress = 1;
                            string reportHeader = "";

                            if (report.ReportAcil == 1)
                            {
                                reportHeader += "<i class=\"fa fa-circle text-danger\" style=\"margin-left:2px; margin-right:2px; font-size:20px\"></i>";
                            }

                            if (report.ReportVip == 1)
                            {
                                reportHeader += "<i class=\"fa fa-circle text-warning\" style=\"margin-left:2px; margin-right:2px; font-size:20px\"></i>";
                            }

                            reportHeader += "<span style=\"margin-left:4px\">" + report.ReportCode + "</span>";

                            if (report.ReportMapState > 0)
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:60px; margin-right:4px\" class= \"fa fa-location-dot text-danger\" ></i>";
                            }
                            else
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:60px; margin-right:4px; color:#676a6c\" class= \"fa fa-location-dot\" ></i>";
                            }

                            if (report.ReportTakbisAlindi > 0)
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:4px; margin-right:4px; color:#62cb31\" class=\"fa fa-file\"></i>";
                            }
                            else
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:4px; margin-right:4px; color:#676a6c\" class=\"fa fa-file\"></i>";
                            }

                            if (report.ReportRandevuAlindi > 0)
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:4px; margin-right:4px; color:#62cb31\" class=\"fa fa-calendar-check\"></i>";
                            }
                            else
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:4px; margin-right:4px; color:#676a6c\" class=\"fa fa-calendar-check\"></i>";
                            }

                            if (report.isLate)
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:4px; margin-right:4px; color:#d9534f\" class=\"fa fa-exclamation-circle\"></i>";
                            }

                            if (report.ReportYazimda == 1)
                            {
                                reportHeader += "<i style=\"font-size:20px; margin-left:4px; color:#62cb31\" class=\"fa fa-square-pen\"></i>";
                            }

                            reportTemplate = reportTemplate.Replace("@@ReportName@@", reportHeader);


                            reportTemplate = reportTemplate.Replace("@@UzmanName@@", report.UzmanFullName);
                        }

                        //@@@ first tabs
                        if (report != null)
                        {
                            progress = 2;

                            string partTabs = "<ul class=\"nav nav-tabs\">";

                            int i = 0;
                            foreach (coTemplatePart part in FormData.TemplateParts)
                            {
                                partTabs += String.Format("<li id=\"li{0}\" class=\"{1}\">" +
                                                            "<a data-toggle=\"tab\" href=\"#tab{2}\" aria-expanded=\"{3}\">{4}</a>" +
                                                          "</li>", (i + 1).ToString(), (i == 0 ? "active" : ""), (i + 1).ToString(), (i == 0 ? "true" : "false"), part.TemplatePartName);

                                i++;
                            }

                            partTabs += "</ul>";

                            reportBody += partTabs;
                        }

                        //@@@ tabcontents
                        if (report != null)
                        {
                            progress = 3;
                            string divContent = "<div class=\"tab-content\">";

                            int j = 0;
                            foreach (coTemplatePart part in FormData.TemplateParts)
                            {
                                divContent += String.Format("<div id=\"tab{0}\" class=\"{1}\">" +
                                                                "<div class=\"panel-body\">" +
                                                                    "<div class=\"row\">" +
                                                                        "<div class=\"col-lg-12\">", (j + 1).ToString(), (j == 0 ? "tab-pane active" : "tab-pane"));


                                divContent += "<ul class=\"nav nav-tabs\">";

                                List<string> addedTapuBasedParts = new List<string>();

                                int k = 0;

                                foreach (coTemplateSubPart subpart in part.TemplateSubParts)
                                {
                                    if (subpart.TemplateSubPartIsTapuBased)
                                    {
                                        if (addedTapuBasedParts.IndexOf(subpart.TemplateSubPartID.ToString()) > -1)
                                        {
                                            continue;
                                        }
                                        else
                                        {
                                            addedTapuBasedParts.Add(subpart.TemplateSubPartID.ToString());

                                        }
                                    }

                                    string title = SharedUtil.getsubparttitle(subpart, part, false, FormData);

                                    int ind = (1000 * (j + 1)) + (k + 1);

                                    divContent += String.Format("<li id=\"li{0}\" class=\"{1}\"><a data-toggle=\"tab\" href=\"#tab{2}\" aria-expanded=\"{3}\">{4}</a></li>", ind, (k == 0 ? "active" : ""), ind.ToString(), (k == 0 ? "true" : "false"), title);

                                    k++;

                                }

                                divContent += "</ul>";


                                divContent += "<div class=\"tab-content\" style=\"overflow-y: auto;\">";


                                List<string> addedTapuBasedParts2 = new List<string>();

                                int m = 0;

                                foreach (coTemplateSubPart tempSubPart in part.TemplateSubParts.OrderBy(x => x.TemplateSubPartIndex).ThenBy(x => x.ReportSubPartUserValueSysTapuID).ThenBy(x => x.ReportSubPartUserValueID).ToList())
                                {
                                    List<coTemplateSubPart> subpartList = new List<coTemplateSubPart>();

                                    if (tempSubPart.TemplateSubPartIsTapuBased)
                                    {
                                        subpartList.AddRange(part.TemplateSubParts.Where(x => x.TemplateSubPartID == tempSubPart.TemplateSubPartID).OrderBy(x => x.ReportSubPartUserValueID).ToList());

                                        if (((selectedTapuSysID == 0 && addedTapuBasedParts2.IndexOf(tempSubPart.TemplateSubPartID.ToString()) > -1)
                                        || (selectedTapuSysID != 0 && selectedTapuSysID != tempSubPart.ReportSubPartUserValueSysTapuID))
                                        && addedTapuBasedParts2.IndexOf(tempSubPart.TemplateSubPartID.ToString()) > -1)
                                        {
                                            /*if (selectedTapuSysID != 0 && selectedTapuSysID != subpart.ReportSubPartUserValueSysTapuID)
                                            {
                                                selectedTapuSysID = subpart.ReportSubPartUserValueSysTapuID;
                                            }
                                            else
                                            {
                                                continue;
                                            }

                                            continue;
                                        }
                                        else
                                        {
                                            addedTapuBasedParts2.Add(tempSubPart.TemplateSubPartID.ToString());
                                        }
                                    }
                                    else
                                    {
                                        subpartList.Add(tempSubPart);
                                    }

                                    int ind = (1000 * (j + 1)) + (m + 1);


                                    divContent += String.Format("<div id=\"tab{0}\" class=\"{1}\">", ind, (m == 0 ? "tab-pane active" : "tab-pane"));
                                    divContent += "<div class=\"panel-body\"><div class=\"row\"><div class=\"col-lg-12\">";

                                    if (tempSubPart.TemplateSubPartIsTapuBased)
                                    {
                                        divContent += "<div class=\"row\"><div class=\"col-lg-12\"><select id=\"sel" + tempSubPart.TemplateSubPartID.ToString() + "\" onchange=\"changeTapuCombo('" + tempSubPart.TemplateSubPartID.ToString() + "')\"  class=\"form-control m-b text-center font-extra-bold\" style=\"font-size:16px; margin:0px; width:160px\">";

                                        List<string> addedList = new List<string>();

                                        foreach (coTemplatePart combopart in FormData.TemplateParts)
                                        {
                                            foreach (coTemplateSubPart combosubpart in combopart.TemplateSubParts.OrderBy(x => x.TemplateSubPartIndex).ThenBy(x => x.ReportSubPartUserValueSysTapuID).ThenBy(x => x.ReportSubPartUserValueID).ToList())
                                            {
                                                if (addedList.IndexOf(combosubpart.ReportSubPartUserValueSysTapuID.ToString()) > -1)
                                                {
                                                    continue;
                                                }

                                                if (!string.IsNullOrEmpty(combosubpart.TemplateSubPartCode) && combosubpart.TemplateSubPartCode == "TAKBISTAPU")
                                                {
                                                    if (selectedTapuSysID == 0)
                                                    {
                                                        selectedTapuSysID = combosubpart.ReportSubPartUserValueSysTapuID;
                                                    }

                                                    string title = SharedUtil.getsubparttitle(combosubpart, combopart, true, FormData);

                                                    addedList.Add(combosubpart.ReportSubPartUserValueSysTapuID.ToString());

                                                    if (selectedTapuSysID == combosubpart.ReportSubPartUserValueSysTapuID)
                                                    {
                                                        divContent += String.Format("<option value=\"{0}\" selected>{1}</option>", combosubpart.ReportSubPartUserValueSysTapuID, title);
                                                    }
                                                    else
                                                    {
                                                        divContent += String.Format("<option value=\"{0}\">{1}</option>", combosubpart.ReportSubPartUserValueSysTapuID, title);
                                                    }
                                                }
                                            }
                                        }

                                        divContent += "</select></div></div><p style=\"height:20px\"></p>";
                                    }

                                    foreach (coTemplateSubPart subPart in subpartList)
                                    {
                                        coTemplateSubPart subpart = subPart;

                                        divContent += string.Format("<div class=\"div{3}\" style=\"display:{0}\" id=\"div_subpart_{1}_{2}\">", subpartList.IndexOf(subPart) == 0 ? "" : "none",
                                            subpart.TemplateSubPartID, subpart.ReportSubPartUserValueSysTapuID, subpart.TemplateSubPartID);

                                        foreach (coTemplateSection section in subpart.TemplateSections)
                                        {
                                            bool checkvisible = SharedUtil.checkVisibility(section.TemplateSectionBankVisibilityIds, section.TemplateSectionVisibilityIds, report, FormData);

                                            if (!checkvisible)
                                            {
                                                continue;
                                            }

                                            if (section.TemplateSectionHasTitle)
                                            {
                                                divContent += String.Format("<div id=\"{0}\" class=\"hpanel\" style=\"margin-top:10px\"><div class=\"panel-heading list-group-item active\"><span style=\"color:#fff; padding-left:10px\">{1}</span></div></div>",
                                                    ("sec" + subpart.ReportSubPartUserValueID + "_" + section.TemplateSectionID), section.TemplateSectionName);
                                            }
                                            else
                                            {
                                                divContent += String.Format("<div id=\"\"></div>", ("sec" + subpart.ReportSubPartUserValueID + "_" + section.TemplateSectionID));
                                            }

                                            divContent += "<div class=\"form-horizontal\">";

                                            foreach (coField field in section.Fields)
                                            {
                                                bool checkfieldvisible = SharedUtil.checkVisibility(field.SectionFieldBankVisibilityIds, field.SectionFieldVisibilityIds, report, FormData);

                                                if (!checkfieldvisible)
                                                {
                                                    continue;
                                                }

                                                if (field.SectionFieldParametricVisible)
                                                {
                                                    bool checkVisibility = SharedUtil.checkFieldVisibleIsOk(field.SectionFieldParametricVisibleSectionFieldID, field.SectionFieldParametricVisibleSectionFieldValueID, subpart);
                                                    if (!checkVisibility)
                                                    {
                                                        continue;
                                                    }
                                                }

                                                string icerik = "";

                                                if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCombobox || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeRadioButton)
                                                {
                                                    string val = "";
                                                    if (field.ReportSubPartUserFieldValue != null)
                                                    {
                                                        val = field.ReportSubPartUserFieldValue;
                                                    }

                                                    foreach (coFieldParameter param in field.FieldParameters)
                                                    {
                                                        if ((!string.IsNullOrEmpty(val) && val == param.FieldParameterValue) || (string.IsNullOrEmpty(val) && param.FieldParameterIsDefault))
                                                        {
                                                            icerik = param.FieldParameterText;
                                                            break;
                                                        }
                                                    }
                                                }
                                                else if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCheckbox)
                                                {
                                                    string val = "";
                                                    if (field.ReportSubPartUserFieldValue != null)
                                                    {
                                                        val = field.ReportSubPartUserFieldValue;
                                                    }

                                                    foreach (coFieldParameter param in field.FieldParameters)
                                                    {
                                                        if (val != null && val.Length > 0 && val.Contains("," + param.FieldParameterValue + ","))
                                                        {
                                                            icerik += param.FieldParameterText + ",";
                                                        }
                                                    }
                                                }
                                                else if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeText || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeInteger
                                                    || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeFloat || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeShortText ||
                                                field.FieldType == (int)Statics.Enums.FieldType.FieldTypeProvince || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCounty ||
                                                field.FieldType == (int)Statics.Enums.FieldType.FieldTypeNeighborhood || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeVillage ||
                                                field.FieldType == (int)Statics.Enums.FieldType.FieldTypeLabel || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeDatetime ||
                                                field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCoordinate || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCoordinateWithWeb)
                                                {
                                                    if (field.ReportSubPartUserFieldValue != null)
                                                    {
                                                        icerik = field.ReportSubPartUserFieldValue;
                                                    }

                                                    if (!string.IsNullOrEmpty(field.FieldAltlikFormat) /*&& string.IsNullOrEmpty(field.ReportSubPartUserFieldValue))
                                                    {
                                                        if (field.FieldID == 251)
                                                        {
                                                            if (field != null)
                                                            {

                                                            }
                                                        }
                                                        icerik = SharedUtil.generateAltlik(field.FieldAltlikFormat, (subpart.TemplatePartHasMultiple ? subpart.ReportSubPartUserValueID : 0), FormData.TemplateParts, subpart.TemplateSubPartIsTapuBased, selectedTapuSysID, 0);
                                                    }

                                                    if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCoordinate || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCoordinateWithWeb)
                                                    {
                                                        if (string.IsNullOrEmpty(icerik))
                                                        {
                                                            icerik = report.ReportTasinmazLocation;
                                                        }
                                                    }
                                                }
                                                else if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeMultilineTextbox)
                                                {
                                                    if (field.ReportSubPartUserFieldValue != null)
                                                    {
                                                        icerik = field.ReportSubPartUserFieldValue;
                                                    }

                                                    if (!string.IsNullOrEmpty(field.FieldAltlikFormat) /*&& string.IsNullOrEmpty(field.ReportSubPartUserFieldValue))
                                                    {
                                                        if (field.FieldID == 15)
                                                        {
                                                            if (field != null)
                                                            {

                                                            }
                                                        }
                                                        icerik = SharedUtil.generateAltlik(field.FieldAltlikFormat, (subpart.TemplatePartHasMultiple ? subpart.ReportSubPartUserValueID : 0),
                                                            FormData.TemplateParts, subpart.TemplateSubPartIsTapuBased, selectedTapuSysID, 0);
                                                    }
                                                }

                                                divContent += String.Format("<div class=\"form-group\" style=\"margin-bottom:0px; border-bottom:1px solid #E8E8E8; padding-bottom:8px; padding-top:8px\"><label class=\"col-lg-2 control-label\">{0}</label>", (field.FieldName + ":"));

                                                if (!string.IsNullOrEmpty(icerik))
                                                {
                                                    divContent += String.Format("<div class=\"col-lg-9\"><p class=\"form-control-static\">{0}<a onclick=\"copyClipboard('{1}')\" style=\"margin-left:10px; font-size:14px\" class=\"fa fa-copy font-extra-bold text-danger\"></a></p></div>", icerik, icerik);
                                                }
                                                else
                                                {
                                                    divContent += "<div class=\"col-lg-9\"><p class=\"form-control-static\"></p></div>";
                                                }


                                                divContent += "</div>";
                                            }


                                            divContent += "</div>";
                                        }

                                        divContent += "</div>";

                                    }

                                    divContent += "</div></div></div></div>";

                                    m++;
                                }

                                divContent += "</div>";

                                divContent += "</div></div></div></div>";

                                j++;
                            }


                            divContent += "</div>";

                            reportBody += divContent;
                        }

                        reportTemplate = reportTemplate.Replace("@@ReportData@@", reportBody);

                        List<coReportFile> fileList = await ServiceUtility.busAsyncNew.GetAllReportFilesByReportID(report.ReportID, 0).ConfigureAwait(false);

                        progress = 4;

                        //List<List<coReportFile>> reportFileListList = new List<List<coReportFile>>();

                        int filePerMail = 30;
                        int totalMailCount = (fileList.Count / filePerMail + (fileList.Count % filePerMail > 0 ? 1 : 0));

                        if (totalMailCount > 0)
                        {
                            for (int mailIndex = 0; mailIndex < totalMailCount; mailIndex++)
                            {
                                using (var compressedFileStream = new MemoryStream())
                                {
                                    using (var zipArchive = new ZipArchive(compressedFileStream, ZipArchiveMode.Update, false))
                                    {
                                        for (int i = 0; i < 4; i++)
                                        {
                                            List<coReportFile> files = SharedUtil.getFilesByType(i + 1, fileList.Skip(mailIndex * filePerMail).Take(filePerMail).ToList());

                                            //List<coReportFile> files = new List<coReportFile>();

                                            if (files.Count > 0)
                                            {
                                                List<(byte[] data, int index, int subtype, bool iskapak)> fileResList = new List<(byte[] data, int index, int subtype, bool iskapak)>();
                                                int kapakindex = -1;

                                                progress = 5;

                                                Parallel.ForEach(files, (file) =>
                                                {
                                                    if (file.ReportFileSourceType == 0)
                                                    {
                                                        string fileguid = file.ReportFileGuid;
                                                        string uri = Statics.SystemValues.blobUrl + fileguid + ".jpg";

                                                        HttpClient client = new HttpClient();
                                                        byte[] response = client.GetByteArrayAsync(uri).Result;

                                                        if (file.ReportFileIsKapak == 1)
                                                        {
                                                            kapakindex = fileResList.Count;
                                                        }

                                                        //Task<(ApiResponse<byte[]> data, int index, int subtype, bool iskapak)> resp = srvReport.GetReportFile(file.ReportFileGuid, files.IndexOf(file), file.ReportFileSubType, file.ReportFileIsKapak == 1);


                                                        (byte[] data, int index, int subtype, bool iskapak) fileRes = (response, files.IndexOf(file), file.ReportFileSubType, file.ReportFileIsKapak == 1);
                                                        fileResList.Add(fileRes);
                                                    }
                                                    else if (file.ReportFileSourceType == 1)
                                                    {
                                                        try
                                                        {
                                                            byte[] response = getInvexFile(data.OrganizationCode, file.ReportFileSysID.ToString(), "F").Result;

                                                            if (file.ReportFileIsKapak == 1)
                                                            {
                                                                kapakindex = fileResList.Count;
                                                            }

                                                            (byte[] data, int index, int subtype, bool iskapak) fileRes = (response, files.IndexOf(file), file.ReportFileSubType, file.ReportFileIsKapak == 1);
                                                            fileResList.Add(fileRes);
                                                        }
                                                        catch (Exception ex)
                                                        {
                                                            if (ex != null)
                                                            {

                                                            }
                                                        }
                                                    }
                                                });

                                                progress = 6;

                                                int k = 0;
                                                foreach ((byte[] data, int index, int subtype, bool iskapak) resp in fileResList)
                                                {
                                                    if (resp.data != null)
                                                    {
                                                        byte[] fileData = resp.data;
                                                        string fileName = report.ReportCode + "-" + Utility.getFilePrefixByType(report.ReportCode, (i + 1), resp.subtype, "_", resp.iskapak) + (k + 1).ToString() + ".jpg";

                                                        if (data != null)
                                                        {
                                                            var zipEntry = zipArchive.CreateEntry(fileName);

                                                            using (var originalFileStream = new MemoryStream(fileData))
                                                            {
                                                                using (var zipEntryStream = zipEntry.Open())
                                                                {
                                                                    originalFileStream.CopyTo(zipEntryStream);
                                                                }
                                                            }
                                                        }
                                                    }


                                                    k++;
                                                }

                                                progress = 7;
                                            }
                                        }

                                    }

                                    bool mailRes = false;

                                    bool istest = false;
                                    if (istest)
                                    {
                                        mailRes = true;
                                        mailRes = SharedUtil.SendMail(reportTemplate, report.ReportCode, null, "zafercalik@gmail.com",
                                            @"Zafer Çalık", null, null, 0, 1);
                                    }
                                    else
                                    {
                                        mailRes = SharedUtil.SendMail(reportTemplate, report.ReportCode, compressedFileStream.ToArray(), data.UzmanEmail,
                                            data.UzmanFullName, data.OperasyonEmail, data.OperasyonFullName, mailIndex, totalMailCount);
                                    }

                                    progress = 8;

                                    if (mailRes)
                                    {
                                        res = true;
                                        progress = 9;
                                    }
                                    else
                                    {
                                        progress = 10;
                                    }
                                }
                            }
                        }
                        else
                        {
                            bool mailRes = false;

                            bool istest = false;
                            if (istest)
                            {
                                mailRes = true;
                                mailRes = SharedUtil.SendMail(reportTemplate, report.ReportCode, null, "zafercalik@gmail.com",
                                            @"Zafer Çalık", null, null, 0, 1);
                            }
                            else
                            {
                                mailRes = SharedUtil.SendMail(reportTemplate, report.ReportCode, null, data.UzmanEmail,
                                            data.UzmanFullName, data.OperasyonEmail, data.OperasyonFullName, 0, 1);
                            }

                            progress = 8;

                            if (mailRes)
                            {
                                res = true;
                                progress = 9;
                            }
                            else
                            {
                                progress = 10;
                            }
                        }


                    }
                    catch (Exception ex)
                    {
                        res = false;
                    }

                    await ServiceUtility.busAsyncNew.UpdateInvexSyncReportYazimState(data.ReportID, res ? 1 : -1, progress.ToString());
                }
            }
        }
        public static JObject getRootObject(string invexCode, string invexToken, string invexUserToken = null)
        {
            JObject data = new JObject();
            //data["code"] = "00";
            data["sirket_id"] = invexCode;
            //data["error_message"] = "";
            //data["info"] = "";
            data["token"] = invexToken;

            if (!string.IsNullOrEmpty(invexUserToken))
            {
                data["kullanici_token"] = invexUserToken;
            }

            return data;
        }
        static async Task<bool> createTapuSicil(coReport report, List<coInvexSyncFieldSubPartCodes> tapuList,
            coTemplate formData, bool isbanka, coUser uzman)
        {
            bool hasErrors = false;

            List<coInvexSyncFieldSubPart> subpartList = getTapuSubPartList(tapuList);

            foreach (coInvexSyncFieldSubPart subPart in subpartList)
            {
                bool resp = await createDataSync(report, formData, subPart.fieldCodes,
                    isbanka ? "tapu_banka" : "tapu", isbanka, "basvuru", "temel-bilgi/tapu-sicil-kaydet", null,
                    subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID,
                    subPart.fieldCodes[0].ReportSubPartUserValueSysID, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);
                if (!resp)
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }

        /*static async Task<bool> createTapuSicil_Banka(List<coInvexSyncFieldSubPartCodes> tapuList, coTemplate formData)
        {
            bool hasErrors = false;

            List<coInvexSyncFieldSubPart> subpartList = getTapuSubPartList(tapuList);

            foreach (coInvexSyncFieldSubPart subPart in subpartList)
            {
                bool resp = await createDataSync(formData, subPart.fieldCodes, "tapu_banka", true, "basvuru", "temel-bilgi/tapu-sicil-kaydet", null, subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID, subPart.fieldCodes[0].ReportSubPartUserValueSysID).ConfigureAwait(false);
                if (!resp)
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }

        static async Task<bool> createTapuFiziki(coReport report, List<coInvexSyncFieldSubPartCodes> tapuList,
            coTemplate formData, bool isbanka, coUser uzman)
        {
            bool hasErrors = false;

            List<coInvexSyncFieldSubPart> subpartList = getTapuSubPartList(tapuList);

            foreach (coInvexSyncFieldSubPart subPart in subpartList)
            {
                bool resp = await createDataSync(report, formData, subPart.fieldCodes,
                    isbanka ? "tapu_ozellik_banka" : "tapu", isbanka, "basvuru", "temel-bilgi/tapu-fiziki-ozellik-kaydet",
                    null, subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID,
                    subPart.fieldCodes[0].ReportSubPartUserValueSysID, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);
                if (!resp)
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }

        static List<coInvexSyncFieldSubPart> getTapuSubPartList(List<coInvexSyncFieldSubPartCodes> tapuList)
        {
            List<coInvexSyncFieldSubPart> subpartList = new List<coInvexSyncFieldSubPart>();
            foreach (coInvexSyncFieldSubPartCodes field in tapuList)
            {
                if (!subpartList.Where(x => x.TapuID == field.ReportSubPartUserValueSysTapuID).Any())
                {
                    coInvexSyncFieldSubPart subPart = new coInvexSyncFieldSubPart();
                    subPart.TapuID = field.ReportSubPartUserValueSysTapuID;
                    subPart.Basvuru_Kod = field.ReportSysID;
                    subPart.SysCode = field.SubPartSysCode;
                    subPart.fieldCodes = new List<coInvexSyncFieldSubPartCodes>();

                    subPart.fieldCodes.AddRange(tapuList.Where(x => x.ReportSubPartUserValueSysTapuID == field.ReportSubPartUserValueSysTapuID).ToList());
                    subpartList.Add(subPart);
                }
            }

            return subpartList;
        }

        /*static async Task<RestResponse> createTapu(coReport report, List<coInvexSyncFieldSubPartCodes> tapuList, coTemplate formData)
        {
            JObject data = getRootObject(tapuList[0].OrganizationInvexCode, tapuList[0].OrganizationInvexToken);

            List<coInvexSyncFieldSubPart> subpartList = new List<coInvexSyncFieldSubPart>();
            foreach (coInvexSyncFieldSubPartCodes field in tapuList)
            {
                if (!subpartList.Where(x => x.TapuID == field.ReportSubPartUserValueSysTapuID).Any())
                {
                    coInvexSyncFieldSubPart subPart = new coInvexSyncFieldSubPart();
                    subPart.TapuID = field.ReportSubPartUserValueSysTapuID;
                    subPart.Basvuru_Kod = field.ReportSysID;
                    subPart.SysCode = field.SubPartSysCode;
                    subPart.fieldCodes = new List<coInvexSyncFieldSubPartCodes>();

                    subPart.fieldCodes.AddRange(tapuList.Where(x => x.ReportSubPartUserValueSysTapuID == field.ReportSubPartUserValueSysTapuID).ToList());
                    subpartList.Add(subPart);
                }
            }

            RestResponse response = null;

            foreach (coInvexSyncFieldSubPart subPart in subpartList)
            {
                //tapu 

                JObject tapuBody = new JObject();

                tapuBody["kod"] = subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID;
                tapuBody["basvuru_kod"] = subPart.fieldCodes[0].ReportSysID;

                createPostBody(report, tapuBody, subPart.fieldCodes.Where(x => x.SubPartSysCode == "tapu_sicil_bilgi").ToList(), null, false, formData, subPart.TapuID > 0);

                data["tapu"] = tapuBody;

                //tapu banka

                JObject tapuBodyBanka = new JObject();

                tapuBodyBanka["kod"] = subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID;
                tapuBodyBanka["basvuru_kod"] = subPart.fieldCodes[0].ReportSysID;

                createPostBody(report, tapuBodyBanka, subPart.fieldCodes.Where(x => x.SubPartSysCode == "tapu_sicil_bilgi").ToList(), null, true, formData, subPart.TapuID > 0);

                data["anatasinmaz_tapu_banka"] = tapuBodyBanka;

                //tapu ozellik

                JObject tapuOzellikBody = new JObject();

                tapuOzellikBody["kod"] = subPart.fieldCodes.Where(x => x.SubPartSysCode == "tapu_ozellikleri").ToList()[0].ReportSubPartUserValueSysID;
                tapuOzellikBody["basvuru_kod"] = subPart.fieldCodes[0].ReportSysID;
                tapuOzellikBody["tapu_kod"] = subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID;

                createPostBody(report, tapuOzellikBody, subPart.fieldCodes.Where(x => x.SubPartSysCode == "tapu_ozellikleri").ToList(), null, false, formData, false);

                data["tapu_ozellik"] = tapuOzellikBody;

                //tapu ozellik banka

                JObject tapuOzellikBankaBody = new JObject();

                tapuOzellikBankaBody["kod"] = subPart.fieldCodes.Where(x => x.SubPartSysCode == "tapu_ozellikleri").ToList()[0].ReportSubPartUserValueSysID;
                tapuOzellikBankaBody["basvuru_kod"] = subPart.fieldCodes[0].ReportSysID;
                tapuOzellikBankaBody["tapu_kod"] = subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID;

                createPostBody(report, tapuOzellikBankaBody, subPart.fieldCodes.Where(x => x.SubPartSysCode == "tapu_ozellikleri").ToList(), null, true, formData, false);

                data["tapu_ozellik_banka"] = tapuOzellikBody;

                response = await ServiceUtility.getPostDataResponse("tapu", "tapu-kaydet", data);
            }

            return response;
        }
        static async Task<bool> createDegerleme(coReport report, List<coInvexSyncFieldSubPartCodes> tapuDegerlemeList,
            coTemplate formData, bool isbanka, bool isgenel, coUser uzman)
        {
            bool hasErrors = false;

            List<coInvexSyncFieldSubPart> subpartList = getTapuSubPartList(tapuDegerlemeList);

            foreach (coInvexSyncFieldSubPart subPart in subpartList)
            {
                bool resp = await createDataSync(report, formData, subPart.fieldCodes,
                    isgenel ? "tapu_degerleme_genel" : (isbanka ? "tapu_degerleme_banka" : "tapu_degerleme"), isbanka,
                    "basvuru", isgenel ? "temel-bilgi/tapu-degerleme-bilgileri-genel-kaydet" : "temel-bilgi/tapu-degerleme-bilgileri-kaydet",
                    null, subPart.fieldCodes[0].ReportSubPartUserValueSysTapuID,
                    subPart.fieldCodes[0].ReportSubPartUserValueSysID, 0,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);

                if (!resp)
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }
        static async Task<bool> createDataSync(coReport report, coTemplate formData, List<coInvexSyncFieldSubPartCodes> fieldVals, string dataobjectfield,
            bool isBankaPost, string postcontroller, string postmethod, List<string> excludedVals,
            int tapukod, int kod, int subpartuservalueid, string deviceip, string devicename, string invextoken)
        {
            if (fieldVals != null && fieldVals.Count > 0)
            {
                JObject data = getRootObject(fieldVals[0].OrganizationInvexCode, fieldVals[0].OrganizationInvexToken);
                JObject postBody = new JObject();

                if (kod > 0)
                {
                    postBody["kod"] = kod;
                }
                else if (tapukod > 0)
                {
                    postBody["kod"] = tapukod;
                }

                postBody["basvuru_kod"] = fieldVals[0].ReportSysID;

                createPostBody(report, postBody, fieldVals, excludedVals, isBankaPost, formData, tapukod > 0);

                data[dataobjectfield] = postBody;
                data["ip"] = deviceip;
                data["cihaz"] = devicename;
                data["kullanici_token"] = invextoken;

                try
                {
                    RestResponse response = await ServiceUtility.getPostDataResponse(postcontroller, postmethod, data);

                    var resulst = GetResFromResponse(response);
                    if (resulst.issuccess)
                    {
                        if (subpartuservalueid > 0 && resulst.kod > 0)
                        {
                            bool res = await busAsyncNew.UpdateInvexSubpartCode(resulst.kod, subpartuservalueid).ConfigureAwait(false);

                            return res;
                        }
                        else
                        {
                            return true;
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (ex != null)
                    {

                    }
                }
            }

            return false;
        }
        /*static async Task<RestResponse> createAnaTasinmaz(coReport report, List<coInvexSyncFieldSubPartCodes> fieldVals, coTemplate formData)
        {
            if (fieldVals != null && fieldVals.Count > 0)
            {
                JObject data = getRootObject(fieldVals[0].OrganizationInvexCode, fieldVals[0].OrganizationInvexToken);

                //tapu_anatasinmaz_vasfi

                JObject postBody = new JObject();

                postBody["kod"] = fieldVals[0].ReportSubPartUserValueSysID;
                postBody["basvuru_kod"] = fieldVals[0].ReportSysID;

                createPostBody(report, postBody, fieldVals, null, false, formData, false);

                data["tapu_anatasinmaz_vasfi"] = postBody;

                //tapu_anatasinmaz_vasfi Banka

                JObject postBodyBanka = new JObject();

                postBodyBanka["kod"] = fieldVals[0].ReportSubPartUserValueSysID;
                postBodyBanka["basvuru_kod"] = fieldVals[0].ReportSysID;

                createPostBody(report, postBodyBanka, fieldVals, null, true, formData, false);

                data["tapu_anatasinmaz_vasfi_banka"] = postBody;

                RestResponse response = await ServiceUtility.getPostDataResponse("tapu", "tapu-anatasinmaz", data);

                return response;
            }

            return null;
        }
        static void createPostBody(coReport report, JObject postBody, List<coInvexSyncFieldSubPartCodes> fieldVals,
            List<string> excludedVals, bool isBankaPost, coTemplate formData, bool isTapuBased)
        {
            List<coInvexSyncFieldSubPartCodes> newVals = new List<coInvexSyncFieldSubPartCodes>();

            foreach (coInvexSyncFieldSubPartCodes field in fieldVals)
            {
                if (field.FieldSysCode == "pozisyon")
                {
                    newVals.AddRange(createKonumValues(field.ReportSubPartUserFieldValue, report));
                }
            }

            fieldVals.AddRange(newVals);

            foreach (coInvexSyncFieldSubPartCodes field in fieldVals)
            {
                if (excludedVals != null && excludedVals.Contains(field.FieldSysCode))
                {
                    continue;
                }

                if ((!isBankaPost && field.SectionFieldIsBankField == 0) || (isBankaPost && field.SectionFieldIsBankField == 1))
                {
                    if (!string.IsNullOrEmpty(field.FieldAltlikFormat))
                    {
                        if (field.FieldSysCode == "mimari_proje")
                        {
                            int a = 5;
                        }

                        string val = Utility.FirstLetterToUpper(SharedUtil.generateAltlik(field.FieldAltlikFormat, (field.TemplatePartHasMultiple ? field.ReportSubPartUserValueID : 0),
                            formData.TemplateParts, isTapuBased, field.ReportSubPartUserValueSysTapuID, 0));
                        field.ReportSubPartUserFieldValue = val;
                    }

                    if (field.ReportSubPartUserFieldValue == null && !(field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCombobox || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeRadioButton || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCheckbox))
                    {
                        postBody[field.FieldSysCode] = "";
                    }
                    else
                    {
                        string retVal = field.ReportSubPartUserFieldValue;

                        if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCombobox || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeRadioButton || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCheckbox)
                        {
                            retVal = "";
                            coField metaField = SharedUtil.getFieldByID(field.FieldID, 0, 0, formData.TemplateParts);
                            if (metaField != null)
                            {
                                if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCombobox || field.FieldType == (int)Statics.Enums.FieldType.FieldTypeRadioButton)
                                {
                                    foreach (coFieldParameter param in metaField.FieldParameters)
                                    {
                                        if (field.FieldID == 35)
                                        {
                                            int a = 5;
                                        }
                                        if ((!string.IsNullOrEmpty(field.ReportSubPartUserFieldValue) && param.FieldParameterValue == field.ReportSubPartUserFieldValue) ||
                                            (string.IsNullOrEmpty(field.ReportSubPartUserFieldValue) || param.FieldParameterIsDefault))
                                        {
                                            retVal = param.FieldParameterSysCode;
                                            break;
                                        }
                                    }
                                }
                                else if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeCheckbox)
                                {
                                    //zzz serialize ozelligi aktif edilecek
                                    //a:3:{i:0;s:8:"Elektrik";i:1;s:11:"Şehir suyu";i:2;s:12:"Kanalizasyon";}
                                    if (field.ReportSubPartUserFieldValue != null)
                                    {
                                        string[] vals = field.ReportSubPartUserFieldValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                        List<string> seltexts = new List<string>();
                                        foreach (string val in vals)
                                        {
                                            foreach (coFieldParameter param in metaField.FieldParameters)
                                            {
                                                if (param.FieldParameterValue == val)
                                                {
                                                    seltexts.Add(param.FieldParameterSysCode);
                                                    //retVal += val + ",";
                                                    break;
                                                }
                                            }
                                        }

                                        //retVal += "a:" + seltexts.Count.ToString() + ":{";
                                        retVal += "[";
                                        //int i = 0;
                                        foreach (string st in seltexts)
                                        {
                                            //retVal += "i:" + i.ToString() + ";s:" + st.Length.ToString() + ":" + "\"" + st + "\";";
                                            retVal += "\"" + st + "\"";
                                            if (seltexts.IndexOf(st) != seltexts.Count - 1)
                                            {
                                                retVal += ",";
                                            }
                                            //i++;
                                        }
                                        //retVal += "}";
                                        retVal += "]";
                                    }
                                }
                            }
                        }

                        if (field.FieldType == (int)Statics.Enums.FieldType.FieldTypeDatetime)
                        {
                            retVal = retVal.Replace("-", ".");
                        }

                        postBody[field.FieldSysCode] = retVal;
                    }
                }
            }
        }
        static async Task<bool> createEmsal(coReport report, List<coInvexSyncFieldSubPartCodes> fieldVals,
            coTemplate formData, bool isbanka, coUser uzman)
        {
            bool hasErrors = false;

            JObject data = getRootObject(fieldVals[0].OrganizationInvexCode, fieldVals[0].OrganizationInvexToken);

            List<coInvexSyncFieldSubPart> subpartList = new List<coInvexSyncFieldSubPart>();
            foreach (coInvexSyncFieldSubPartCodes field in fieldVals)
            {
                if (!subpartList.Where(x => x.TapuID == field.ReportSubPartUserValueID).Any()) //burada tapudaki gibi sysid uzerinden degil, ReportSubPartUserValueID uzerinden ilerliyoruz cunku sistemde olmayan localde create edilmis kayitlar olabilir
                {
                    coInvexSyncFieldSubPart subPart = new coInvexSyncFieldSubPart();
                    subPart.TapuID = field.ReportSubPartUserValueID;
                    subPart.Basvuru_Kod = field.ReportSysID;
                    subPart.SysCode = field.SubPartSysCode;
                    subPart.fieldCodes = new List<coInvexSyncFieldSubPartCodes>();

                    subPart.fieldCodes.AddRange(fieldVals.Where(x => x.ReportSubPartUserValueID == field.ReportSubPartUserValueID).ToList());
                    subpartList.Add(subPart);
                }
            }

            foreach (coInvexSyncFieldSubPart subPart in subpartList)
            {
                bool resp = await createDataSync(report, formData, subPart.fieldCodes, isbanka ? "banka_emsal" : "emsal",
                    isbanka, "basvuru", "temel-bilgi/emsal-kaydet", null, 0,
                    subPart.fieldCodes[0].ReportSubPartUserValueSysID, subPart.TapuID,
                    uzman.UserDeviceIp, uzman.UserDeviceName, uzman.UserInvexToken).ConfigureAwait(false);
                if (!resp)
                {
                    hasErrors = true;
                }
            }

            return !hasErrors;
        }
        public static async Task<byte[]> getInvexFile(string organizationcode, string filekod, string type)
        {
            try
            {
                coOrganization organization = await busAsyncNew.GetOrganizationByCode(organizationcode).ConfigureAwait(false);

                string postdata = "{\"data\":{ \"token\":\"" + organization.OrganizationInvexToken + "\", \"code\":\"00\", \"sirket_id\":\""
                    + organization.OrganizationInvexCode + "\", \"kod\":\"" + filekod + "\",\"yer\":\"" + type + "\", \"indir\":\"0\" } }";

                RestResponse response = await getResponse("basvuru/temel-bilgi", "dosya-fotograf-goster", organization.OrganizationInvexToken, organization.OrganizationInvexCode,
                        new string[] { "data" }, new string[] { postdata });

                byte[] content = response.RawBytes;

                if (response != null && response.Content != null && response.StatusCode == HttpStatusCode.OK)
                {
                    return content;
                }

            }
            catch
            {

            }

            return null;
        }
        static (bool issuccess, int kod) GetResFromResponse(RestResponse response, string kodname = "kod")
        {
            bool res = false;
            int kod = 0;

            if (response.IsSuccessful)
            {
                if (!string.IsNullOrEmpty(response.Content))
                {
                    try
                    {
                        string json = response.Content.Trim().Trim(new char[] { '\n' });
                        JObject jObj = JObject.Parse(json);

                        if (jObj["status"].ToString() == "OK")
                        {
                            if (jObj.ContainsKey(kodname)) //bu 0'dan buyuk ise servisten gelen cevaptan kod bilgisi alinip ilgili subpartuservalue update edilmeli
                            {
                                kod = Convert.ToInt32(jObj[kodname]);
                            }

                            res = true;
                        }
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }

            return (res, kod);
        }
        #endregion Sync

        #region processParts

        static async Task<(bool res, int reportid, int basvuru_id, string uzman)> processBasvuru(JObject basvuru,
            coInvexReportStateChange obj, Statics.Enums.BasvuruProcessTypes type)
        {
            /* type 
            1- uzmanaatandi
            2- basvuru
            3- konum
            4- basvuru_banka
            5- konum_banka
             
            bool res = true;

            string kod = Utility.JTokenToString(basvuru, "kod");
            if (string.IsNullOrEmpty(kod))
            {
                kod = Utility.JTokenToString(basvuru, "basvuru_kod");
            }

            int basvuru_id = Convert.ToInt32(kod);
            int reportid = 0;
            string uzman = "0";

            if ((int)type < 4)
            {
                string sirket_rapor_no = Utility.JTokenToString(basvuru, "sirket_rapor_no");
                uzman = Utility.JTokenToString(basvuru, "uzman");
                string il = Utility.JTokenToString(basvuru, "il");
                string ilce = Utility.JTokenToString(basvuru, "ilce");
                string mahalle = Utility.JTokenToString(basvuru, "mahalle");
                string gayrimenkulu_gosterecek_kisi = Utility.JTokenToString(basvuru, "gayrimenkulu_gosterecek_kisi");
                string musteri_unvani = Utility.JTokenToString(basvuru, "musteri_unvani");
                string gayrimenkulu_gosterecek_kisi_tel_alan = Utility.JTokenToString(basvuru, "gayrimenkulu_gosterecek_kisi_tel_alan");
                string gayrimenkulu_gosterecek_kisi_tel = Utility.JTokenToString(basvuru, "gayrimenkulu_gosterecek_kisi_tel");
                string gayrimenkulu_gosterecek_kisi_tel_dahili = Utility.JTokenToString(basvuru, "gayrimenkulu_gosterecek_kisi_tel_dahili");

                string yergostericitel = (gayrimenkulu_gosterecek_kisi_tel_alan == null ? "" : gayrimenkulu_gosterecek_kisi_tel_alan) + " " +
                    (gayrimenkulu_gosterecek_kisi_tel == null ? "" : gayrimenkulu_gosterecek_kisi_tel) + " " +
                    (gayrimenkulu_gosterecek_kisi_tel_dahili == null ? "" : gayrimenkulu_gosterecek_kisi_tel_dahili).Trim();

                string banka = Utility.JTokenToString(basvuru, "banka");

                string etkin = Utility.JTokenToString(basvuru, "etkin");
                string islemde = Utility.JTokenToString(basvuru, "islemde");
                string silindi_eh = Utility.JTokenToString(basvuru, "silindi_eh");
                string basvuru_tarihi = Utility.JTokenToString(basvuru, "basvuru_tarihi");
                string son_tarih_uzman = Utility.JTokenToString(basvuru, "son_tarih_uzman");

                string acil_kontrol = Utility.JTokenToString(basvuru, "acil_kontrol");
                string vip_kontrol = Utility.JTokenToString(basvuru, "vip_kontrol");

                DateTime createdDate = DateTime.ParseExact(basvuru_tarihi, "yyyy-MM-dd HH:mm:ss", null);
                DateTime sonTarihUzman = DateTime.ParseExact(son_tarih_uzman, "yyyy-MM-dd HH:mm:ss", null);

                string basvuru_rapor = Utility.JTokenToString(basvuru, "basvuru_rapor");
                string birimi = Utility.JTokenToString(basvuru, "birimi");

                string takbis_kontrol = Utility.JTokenToString(basvuru, "takbis_kontrol");

                reportid = await busAsyncNew.CreateUpdateReportByService(basvuru_id, sirket_rapor_no, Convert.ToInt32(uzman), il, ilce, mahalle,
                    gayrimenkulu_gosterecek_kisi, musteri_unvani, yergostericitel, obj.sirket_id, Convert.ToInt32(banka), etkin == "E" ? 1 : 0, islemde == "E" ? 1 : 0,
                    silindi_eh == "E" ? 1 : 0, obj.info, null, basvuru_rapor, birimi, createdDate, sonTarihUzman,
                    acil_kontrol == "E" ? 1 : 0, vip_kontrol == "E" ? 1 : 0, takbis_kontrol == "A" ? 0 : 1).ConfigureAwait(false);

                //type 1 = atama sirasinda gelinmis
                if (uzman != "0" && type == Statics.Enums.BasvuruProcessTypes.UzmanaAtandi)
                {
                    await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportAssignedUser, sirket_rapor_no, "UserInvexID_" + uzman, 0, sirket_rapor_no, basvuru_id).ConfigureAwait(false);
                    /*if (type == Statics.Enums.BasvuruProcessTypes.UzmanaAtandi)
                    {
                        await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportAssignedUser, sirket_rapor_no, "UserInvexID_" + uzman, 0, sirket_rapor_no, basvuru_id).ConfigureAwait(false);
                    }
                    else if (type == Statics.Enums.BasvuruProcessTypes.Basvuru)
                    {
                        await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepBasvuruGuncellendi, sirket_rapor_no, "UserInvexID_" + uzman, 0, sirket_rapor_no, basvuru_id).ConfigureAwait(false);
                    }
                    else if (type == Statics.Enums.BasvuruProcessTypes.Konum)
                    {
                        await busAsyncNew.InsertPushLog((int)Statics.Enums.NotificationTypes.ReportTalepKonumGuncellendi, sirket_rapor_no, "UserInvexID_" + uzman, 0, sirket_rapor_no, basvuru_id).ConfigureAwait(false);
                    }
                }
            }
            else
            {
                reportid = await busAsyncNew.GetReportIDByBasvuruKod(basvuru_id).ConfigureAwait(false);
            }


            if (reportid > 0)
            {
                string subpartlist = "";

                if (type == Statics.Enums.BasvuruProcessTypes.UzmanaAtandi)
                {
                    subpartlist = "temel_bilgiler,konum_bilgi";
                }
                else if (type == Statics.Enums.BasvuruProcessTypes.Basvuru)
                {
                    subpartlist = "temel_bilgiler";
                }
                else if (type == Statics.Enums.BasvuruProcessTypes.Konum)
                {
                    subpartlist = "konum_bilgi";
                }
                else if (type == Statics.Enums.BasvuruProcessTypes.Basvuru_Banka)
                {
                    subpartlist = "temel_bilgiler,konum_bilgi";
                }
                else if (type == Statics.Enums.BasvuruProcessTypes.Konum_Banka)
                {
                    subpartlist = "konum_bilgi";
                }


                List<coInvexSyncFieldSubPartCodes> subPartCodes = await busAsyncNew.GetTemplateSubPartFieldCodes(subpartlist).ConfigureAwait(false);

                List<coInvexSubPartSync> subPartList = new List<coInvexSubPartSync>();

                foreach (coInvexSyncFieldSubPartCodes subPartCode in subPartCodes)
                {
                    if (!subPartList.Where(x => x.SubpartCode == subPartCode.SubPartSysCode).Any())
                    {
                        coInvexSubPartSync invexSubPartSync = new coInvexSubPartSync();
                        invexSubPartSync.SubpartCode = subPartCode.SubPartSysCode;

                        invexSubPartSync.invexFields = new List<coInvexField>();

                        List<coInvexSyncFieldSubPartCodes> subPartCodeList = subPartCodes.Where(x => x.SubPartSysCode == subPartCode.SubPartSysCode).ToList();

                        foreach (coInvexSyncFieldSubPartCodes subPartCodeLocal in subPartCodeList)
                        {
                            if (basvuru.ContainsKey(subPartCodeLocal.FieldSysCode))
                            {
                                string val = Utility.JTokenToString(basvuru, subPartCodeLocal.FieldSysCode);
                                invexSubPartSync.invexFields.Add(new coInvexField() { fieldname = subPartCodeLocal.FieldSysCode, fieldvalue = val, fieldtype = subPartCodeLocal.FieldType });
                            }
                            else
                            {
                                if (basvuru != null)
                                {

                                }
                            }
                        }

                        subPartList.Add(invexSubPartSync);
                    }
                }

                foreach (coInvexSubPartSync subPart in subPartList)
                {
                    int subpartuservalueidTemel = await busAsyncNew.CreateInvexSyncReportSubPartUserValue(reportid, subPart.SubpartCode, Convert.ToInt32(uzman), 0, 0, basvuru_id).ConfigureAwait(false);
                    if (subpartuservalueidTemel > 0)
                    {
                        foreach (coInvexField invexField in subPart.invexFields)
                        {
                            await busAsyncNew.CreateInvexSyncReportSubPartUserFieldValue(subpartuservalueidTemel, invexField.fieldname, subPart.SubpartCode, invexField.fieldvalue).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        res = false;
                    }
                }
            }
            else
            {
                res = false;
            }

            if (res)
            {
                await busAsyncNew.UpdateReportVersionNo(reportid, 0);
            }

            return (res, reportid, basvuru_id, uzman);
        }
        static async Task<bool> tapuOperations(JArray tapuList)
        {
            bool res = true;

            try
            {
                Parallel.ForEach(tapuList, async (tapu) =>
                {
                    int tapukod = Convert.ToInt32(Utility.JTokenToString((JObject)tapu, "kod"));

                    string pozisyon_n = Utility.JTokenToString((JObject)tapu, "pozisyon_n");
                    string pozisyon_e = Utility.JTokenToString((JObject)tapu, "pozisyon_e");
                    string malikText = GetMalikText(tapu);

                    string pozisyon = !string.IsNullOrEmpty(pozisyon_e) && !string.IsNullOrEmpty(pozisyon_n) ? pozisyon_n + ";" + pozisyon_e : "";

                    if (!String.IsNullOrEmpty(Utility.JTokenToString((JObject)tapu, "ada"))
                        && !String.IsNullOrEmpty(Utility.JTokenToString((JObject)tapu, "parsel")))
                    {
                        try
                        {
                            string ada = Utility.JTokenToString((JObject)tapu, "ada");
                            string parsel = Utility.JTokenToString((JObject)tapu, "parsel");
                            int basvuru_kod = Convert.ToInt32(Utility.JTokenToString((JObject)tapu, "basvuru_kod"));

                            if (!string.IsNullOrEmpty(ada) && !string.IsNullOrEmpty(parsel))
                            {
                                await busAsyncNew.UpdateReportAdaParselByReportID(basvuru_kod, ada, parsel).ConfigureAwait(false);
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                    }

                    var tapuRes = await subpartOperations((JObject)tapu, true, "tapu_sicil_bilgi", new string[] { "pozisyon_n", "pozisyon_e", "malikler" },
                        new List<string>() { "pozisyon", "maliklist" }, new List<string>() { pozisyon, malikText }, tapukod).ConfigureAwait(false);

                    if (!tapuRes.res)
                    {
                        res = false;
                    }

                    var tapuOzellikRes = await subpartOperations((JObject)tapu, true, "tapu_ozellikleri", null, null, null, tapukod, new List<string>() { "fiilikullanim_niteligi", "fiilikullanim_niteligi_aciklama" });
                    if (!tapuOzellikRes.res)
                    {
                        res = false;
                    }
                });
            }
            catch (Exception ex)
            {
                res = false;
            }

            return res;
        }
        static async Task<(bool res, int reportid, int basvuru_id, string uzman)> tapuOzellikOperations(JObject tapu, JObject tapuozellik)
        {
            int reportid = 0;
            int basvuru_kod = 0;

            bool res = false;
            bool res1 = true;
            bool res2 = true;

            int tapu_kod = 0;
            int tapu_ozellik_kod = 0;


            JObject obj = new JObject();
            if (tapu != null)
            {
                foreach (JProperty prop in tapu.Properties())
                {
                    //obj[prop.Name] = tapu[prop];
                    obj.Add(prop.Name, tapu[prop.Name]);
                }
            }

            if (tapuozellik != null)
            {
                foreach (JProperty prop in tapuozellik.Properties())
                {
                    if (obj.ContainsKey(prop.Name))
                    {
                        if ((obj[prop.Name] == null || obj[prop.Name].ToString() == ""))
                        {
                            obj[prop.Name] = tapuozellik[prop.Name];
                        }
                    }
                    else
                    {
                        obj.Add(prop.Name, tapuozellik[prop.Name]);
                    }
                }
            }

            try
            {
                if (tapu != null)
                {
                    int tapukod = Convert.ToInt32(Utility.JTokenToString(tapu, "kod"));

                    string pozisyon_n = Utility.JTokenToString(tapu, "pozisyon_n");
                    string pozisyon_e = Utility.JTokenToString(tapu, "pozisyon_e");
                    string malikText = GetMalikText(tapu);

                    string pozisyon = !string.IsNullOrEmpty(pozisyon_e) && !string.IsNullOrEmpty(pozisyon_n) ? pozisyon_n + ";" + pozisyon_e : "";


                    var tapuRes = await subpartOperations(obj, true, "tapu_sicil_bilgi,tapu_ozellikleri", new string[] { "kod", "basvuru_kod", "pozisyon_n", "pozisyon_e", "malikler" },
                        new List<string>() { "pozisyon", "maliklist" }, new List<string>() { pozisyon, malikText }, tapukod).ConfigureAwait(false);

                    res1 = tapuRes.res;
                    reportid = tapuRes.reportid;
                    basvuru_kod = tapuRes.basvuru_id;

                    string ada = Utility.JTokenToString(tapu, "ada");
                    string parsel = Utility.JTokenToString(tapu, "parsel");


                    if (!string.IsNullOrEmpty(ada) && !string.IsNullOrEmpty(parsel))
                    {
                        await busAsyncNew.UpdateReportAdaParselByReportID(basvuru_kod, ada, parsel).ConfigureAwait(false);
                    }
                }

                if (tapuozellik != null)
                {
                    int tapukod = 0;
                    if (!string.IsNullOrEmpty(Utility.JTokenToString(tapuozellik, "tapu_kod")))
                    {
                        tapukod = Convert.ToInt32(Utility.JTokenToString(tapuozellik, "tapu_kod"));
                    }
                    else if (!string.IsNullOrEmpty(Utility.JTokenToString(tapuozellik, "kod")))
                    {
                        tapukod = Convert.ToInt32(Utility.JTokenToString(tapuozellik, "kod"));
                    }

                    var tapuOzellikRes = await subpartOperations(obj, true, "tapu_sicil_bilgi,tapu_ozellikleri", new string[] { "tapu_kod" },
                        null, null, tapukod).ConfigureAwait(false);


                    res2 = tapuOzellikRes.res;
                    if (reportid == 0)
                    {
                        reportid = tapuOzellikRes.reportid;
                    }
                    if (basvuru_kod == 0)
                    {
                        basvuru_kod = tapuOzellikRes.basvuru_id;
                    }
                }

            }
            catch (Exception ex)
            {
                res = false;
            }

            res = res1 && res2;

            if (res)
            {
                await busAsyncNew.UpdateReportVersionNo(reportid, 0);
            }

            return (res, reportid, basvuru_kod, "");
        }

        static string GetMalikText(JToken data)
        {
            string malikText = "";
            if (data["malikler"] != null && data["malikler"].ToString().Length > 0)
            {
                try
                {
                    JObject malikler = (JObject)data["malikler"];

                    if (malikler.ContainsKey("malik") && malikler["malik"] != null &&
                        malikler.ContainsKey("malik_hisse_pay") && malikler["malik_hisse_pay"] != null &&
                        malikler.ContainsKey("malik_hisse_payda") && malikler["malik_hisse_payda"] != null)
                    {
                        JArray malik = (JArray)malikler["malik"];
                        JArray malik_hisse_pay = (JArray)malikler["malik_hisse_pay"];
                        JArray malik_hisse_payda = (JArray)malikler["malik_hisse_payda"];

                        for (int i = 0; i < malik.Count; i++)
                        {
                            if (i != 0)
                            {
                                malikText += ",";
                            }

                            malikText += malik[i].ToString();
                            if (malik_hisse_pay.Count > i)
                            {
                                malikText += "-" + malik_hisse_pay[i];
                            }

                            if (malik_hisse_payda.Count > i)
                            {
                                malikText += "-" + malik_hisse_payda[i];
                            }
                        }
                    }
                }
                catch
                {

                }
            }

            return malikText;
        }
        static async Task<(bool res, int reportid, int basvuru_id, string uzman)> anaTasinmazOperations(JObject anaTasinmaz)
        {
            //anatasinmaz normalde multiple olmamasina ragmen true dedik cunku buna ait bir kod da geliyor.
            return await subpartOperations(anaTasinmaz, true, "tapu_anatasinmaz_vasfi", null, null, null, 0).ConfigureAwait(false);
        }
        static async Task<(bool res, int reportid, int basvuru_id, string uzman)> degerlemeOperations(JObject degerleme)
        {
            int tapukod = Convert.ToInt32(Utility.JTokenToString(degerleme, "kod"));

            return await subpartOperations(degerleme, true, "tapu_degerleme", null, null, null, tapukod).ConfigureAwait(false);
        }
        static async Task<(bool res, int reportid, int basvuru_id, string uzman)> emsalOperations(JObject emsal)
        {

            int reportid = 0;
            int basvuru_kod = 0;

            bool res = false;


            try
            {
                string pozisyon_n = Utility.JTokenToString(emsal, "pozisyon_n");
                string pozisyon_e = Utility.JTokenToString(emsal, "pozisyon_e");

                string tel1 = Utility.JTokenToString(emsal, "tel1");
                string tel2 = Utility.JTokenToString(emsal, "tel2");

                string tel = !string.IsNullOrEmpty(tel1) && !string.IsNullOrEmpty(tel2) ? tel1 + tel2 : "";

                string pozisyon = !string.IsNullOrEmpty(pozisyon_e) && !string.IsNullOrEmpty(pozisyon_n) ? pozisyon_n + ";" + pozisyon_e : "";

                var emsalRes = await subpartOperations(emsal, true, "emsal", new string[] { "kod", "basvuru_kod", "pozisyon_n", "pozisyon_e", "tel1",
                    "tel2", "tel12", "tel22", "tel13", "tel23", "tapu_kod", "google_adres", "tarih"},
                    new List<string>() { "pozisyon", "tel" }, new List<string>() { pozisyon, tel }, 0).ConfigureAwait(false);

                res = emsalRes.res;
                reportid = emsalRes.reportid;
                basvuru_kod = emsalRes.basvuru_id;

            }
            catch (Exception ex)
            {
                res = false;
            }

            if (res)
            {
                await busAsyncNew.UpdateReportVersionNo(reportid, 0);
            }

            return (res, reportid, basvuru_kod, "");
        }
        static async Task<(bool res, int reportid, int basvuru_id, string uzman)> subpartOperations(JObject subpartObject, bool ismultiplesubpart, string subpartlist,
            string[] excludedfiels, List<string> addedKeys, List<string> addedValues, int tapukod, List<string> onlyKeys = null)
        {
            bool res = true;
            int reportid = 0;
            int basvuru_kod = 0;

            try
            {

                List<string> excludedfielsList = new List<string>();
                if (excludedfiels != null)
                {
                    excludedfielsList.AddRange(excludedfiels);
                }
                excludedfielsList.AddRange(new string[] { "kod", "basvuru_kod" });


                int kod = 0;
                if (ismultiplesubpart)
                {
                    kod = Convert.ToInt32(Utility.JTokenToString(subpartObject, "kod"));
                }

                basvuru_kod = Convert.ToInt32(Utility.JTokenToString(subpartObject, "basvuru_kod"));
                reportid = await busAsyncNew.GetReportIDByBasvuruKod(basvuru_kod).ConfigureAwait(false);

                List<coInvexSyncFieldSubPartCodes> subPartCodes = await busAsyncNew.GetTemplateSubPartFieldCodes(subpartlist).ConfigureAwait(false);

                List<coInvexSubPartSync> subPartList = new List<coInvexSubPartSync>();

                foreach (coInvexSyncFieldSubPartCodes subPartCode in subPartCodes)
                {
                    if (!subPartList.Where(x => x.SubpartCode == subPartCode.SubPartSysCode).Any())
                    {
                        coInvexSubPartSync invexSubPartSync = new coInvexSubPartSync();
                        invexSubPartSync.SubpartCode = subPartCode.SubPartSysCode;

                        invexSubPartSync.invexFields = new List<coInvexField>();

                        List<coInvexSyncFieldSubPartCodes> subPartCodeList = subPartCodes.Where(x => x.SubPartSysCode == subPartCode.SubPartSysCode).ToList();

                        foreach (coInvexSyncFieldSubPartCodes subPartCodeLocal in subPartCodeList)
                        {
                            if (subpartObject.ContainsKey(subPartCodeLocal.FieldSysCode) && (onlyKeys == null || onlyKeys.Contains(subPartCodeLocal.FieldSysCode))
                                && !excludedfielsList.Contains(subPartCodeLocal.FieldSysCode))
                            {
                                string val = Utility.JTokenToString(subpartObject, subPartCodeLocal.FieldSysCode);
                                invexSubPartSync.invexFields.Add(new coInvexField() { fieldname = subPartCodeLocal.FieldSysCode, fieldvalue = val, fieldtype = subPartCodeLocal.FieldType });
                            }
                            else
                            {
                                if (subpartObject != null)
                                {

                                }
                            }
                        }

                        subPartList.Add(invexSubPartSync);
                    }
                }

                foreach (coInvexSubPartSync subPart in subPartList)
                {
                    if (subPart.SubpartCode != "TAPUFIZIKI")
                    {
                        kod = tapukod;
                    }
                    int subpartuservalueidTemel = busAsyncNew.CreateInvexSyncReportSubPartUserValue(reportid, subPart.SubpartCode, 0, kod, tapukod, basvuru_kod).Result;
                    if (subpartuservalueidTemel > 0)
                    {
                        foreach (coInvexField invexField in subPart.invexFields)
                        {
                            if (invexField.fieldtype == (int)Statics.Enums.FieldType.FieldTypeCheckbox)
                            {

                            }
                            await busAsyncNew.CreateInvexSyncReportSubPartUserFieldValue(subpartuservalueidTemel, invexField.fieldname, subPart.SubpartCode, invexField.fieldvalue).ConfigureAwait(false);
                        }
                    }
                    else
                    {
                        res = false;
                    }
                }

                /*
                int subpartid = await busAsyncNew.CreateInvexSyncReportSubPartUserValue(reportid, subpartcode, 0, kod, tapukod).ConfigureAwait(false);

                Dictionary<string, object> dictObj = subpartObject.ToObject<Dictionary<string, object>>();
                if (addedKeys != null && addedValues != null && addedKeys.Count == addedValues.Count)
                {
                    for (int i = 0; i < addedKeys.Count; i++)
                    {
                        dictObj.Add(addedKeys[i], addedValues[i]);
                    }
                }


                if (subpartid > 0)
                {
                    foreach (string key in dictObj.Keys)
                    {
                        if (onlyKeys == null || onlyKeys.Contains(key))
                        {
                            if (!excludedfielsList.Contains(key) && dictObj[key] != null)
                            {
                                await busAsyncNew.CreateInvexSyncReportSubPartUserFieldValue(subpartid, key, subpartcode, dictObj[key].ToString()).ConfigureAwait(false);
                            }
                        }
                    }
                }
                else
                {
                    res = false;
                }

            }
            catch (Exception ex)
            {
                res = false;
            }

            if (res)
            {
                await busAsyncNew.UpdateReportVersionNo(reportid, 0);
            }

            return (res, reportid, basvuru_kod, "");
        }*/
        #endregion processParts

    }
}

        