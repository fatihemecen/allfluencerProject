
using BlazorApp.Client;
using BlazorApp.Shared;
using Blazored.LocalStorage;
using Newtonsoft.Json;
using Objects;
using System.Net;
using System.Net.Http.Json;
using System.Security.Authentication;
using System.Text;
using Util;
using static Util.Statics.Enums;

namespace AdminPanel
{
    public class mnServiceManager
    {
        private HttpClient Http { get; set; }
        private mnTokenManager TokenManager { get; set; }
        private mnUserManager UserManager { get; set; }
        private ILocalStorageService LocalStorage { get; set; }
        //coJWTToken jWTToken = new coJWTToken();
        //string sessionkey = "";

        public mnServiceManager(HttpClient _http, ILocalStorageService _localStorage, mnTokenManager _tokenManager, mnUserManager userManager)
        {
            Http = _http;
            LocalStorage = _localStorage;
            TokenManager = _tokenManager;
            UserManager = userManager;
        }
        async Task<string> createGetUrl(string methodName, Dictionary<string, string> pars, bool addsessionkey)
        {
            string callurl = "api/" + methodName;

            string allparams = "";

            if (pars != null)
            {
                callurl += "?";

                if (pars != null)
                {
                    List<string> keys = pars.Keys.ToList();
                    keys = keys.OrderBy(x => x).ToList();

                    foreach (string item in keys)
                    {
                        string val = pars[item];

                        callurl += "&" + item + "=" + val;

                        if (val != null)
                        {
                            allparams += val.ToString();
                        }
                        else
                        {
                            allparams += "";
                        }
                    }

                    callurl += "&key=" + SessionManager.CreateKey(allparams);
                }
            }

            //callurl += allparams;

            if (addsessionkey)
            {
                string sessionKey = await GetToken();
                callurl += "&sessionkey=" + sessionKey;
            }

            return callurl;
        }

        public async Task<T> GetMethod<T>(string methodName, Dictionary<string, string> pars, bool addsessionkey = true)
        {
            try
            {
                string callurl = await createGetUrl(methodName, pars, addsessionkey).ConfigureAwait(false);

                /*Http.DefaultRequestHeaders.Clear();
                if (addJwt)
                {
                    Http.DefaultRequestHeaders.Add(Statics.SystemKeys.skTokenHeader, jWTToken.JWTContent);
                }*/

                const SslProtocols _Tls12 = (SslProtocols)0x00000C00;
                const SecurityProtocolType Tls12 = (SecurityProtocolType)_Tls12;
                ServicePointManager.SecurityProtocol = Tls12;

                T retVal = await Http.GetFromJsonAsync<T>(callurl);

                return retVal;
            }
            catch (Exception ex)
            {

            }

            return default;
        }
        public async Task<HttpResponseMessage> PostRequest(Dictionary<string, string> pars, string methodName, bool isPreToken)
        {
            foreach (string item in pars.Keys)
            {
                pars[item] = Base32.EncodeAsBase32String(pars[item]);
            }

            pars.Add(Statics.SystemKeys.skTempParam, Guid.NewGuid().ToString());
            pars.Add(Statics.SystemKeys.skServiceCallDateTick, DateTime.UtcNow.Ticks.ToString());

            string key = GeneralUtility.CreateKey(pars);
            pars.Add(Statics.SystemKeys.skKey, key);

            string json = JsonConvert.SerializeObject(pars);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            Http.DefaultRequestHeaders.Clear();
            if (!isPreToken)
            {
                string token = await GetToken();
                Http.DefaultRequestHeaders.Add(Statics.SystemKeys.skTokenHeader, token);
            }

            HttpResponseMessage httpResponseMessage;
            try
            {
                httpResponseMessage = await Http.PostAsync("api/" + methodName, data).ConfigureAwait(false);

            }
            catch (Exception ex)
            {
                httpResponseMessage = new HttpResponseMessage(System.Net.HttpStatusCode.NotFound);
            }

            return httpResponseMessage;
        }
        public async Task<HttpResponseMessage> PostPreTokenRequest(string methodName, Dictionary<string, string> pars)
        {
            HttpResponseMessage httpResponseMessage = await PostRequest(pars, methodName, true).ConfigureAwait(false);

            return httpResponseMessage;
        }
        public async Task<HttpResponseMessage> PostPreTokenRequest(string methodName)
        {
            HttpResponseMessage httpResponseMessage = await PostRequest(new Dictionary<string, string>(), methodName, true).ConfigureAwait(false);

            return httpResponseMessage;
        }
        public async Task<HttpResponseMessage> PostAfterTokenRequest(string methodName, Dictionary<string, string> pars)
        {
            HttpResponseMessage httpResponseMessage = await PostRequest(pars, methodName, false).ConfigureAwait(false);
            return httpResponseMessage;
        }

        public async Task<T> PostAfterTokenRequest<T>(string methodName, Dictionary<string, string> pars)
        {
            HttpResponseMessage httpResponseMessage = await PostRequest(pars, methodName, false).ConfigureAwait(false);
            if (httpResponseMessage.IsSuccessStatusCode)
            {
                try
                {
                    T res = JsonConvert.DeserializeObject<T>(await httpResponseMessage.Content.ReadAsStringAsync());
                    return res;
                }
                catch
                {

                }
            }

            return default;
        }
        public async Task<HttpResponseMessage> PostAfterTokenRequest(string methodName)
        {
            HttpResponseMessage httpResponseMessage = await PostRequest(new Dictionary<string, string>(), methodName, false).ConfigureAwait(false);

            return httpResponseMessage;
        }

        #region Token

        public async void setToken(string jwtToken)
        {
            coJWTToken token = new coJWTToken(jwtToken);
            TokenManager.Token = token;
            UserManager.user = new coUser(token);

            await LocalStorage.SetItemAsync(Statics.SystemKeys.skJwt, jwtToken);
        }
        public async void removeToken()
        {
            TokenManager.Token = null;
            UserManager.user = new coUser();

            await LocalStorage.RemoveItemAsync(Statics.SystemKeys.skJwt);
        }
        public async Task<string> GetToken()
        {
            TokenCheckStatus status = await TokenManager.PrepareToken();
            HttpResponseMessage resp = null;

            string retVal = "";

            ///token zaten ayaga kaldirilmis ise herhangi bir isleme gerek yok
            if (status != TokenCheckStatus.AlreadyCreated)
            {
                ///zzz token localden ayaga kaldirilmis, check edilecek
                if (status == TokenCheckStatus.CreatedFromLocal)
                {
                    resp = await CheckToken();
                    if (resp == null || !resp.IsSuccessStatusCode)
                    {
                        resp = await CreatePreToken();
                    }
                }
                ///zzz henuz localde bile token yok, generate edilecek
                else if (status == TokenCheckStatus.NotExist)
                {
                    resp = await CreatePreToken();
                }

                if (resp != null && resp.IsSuccessStatusCode)
                {
                    string jwt = resp.Content.ReadAsStringAsync().Result;
                    setToken(jwt);


                    retVal = jwt;
                }
            }
            else
            {
                retVal = TokenManager.Token.JWTContent;
            }

            return retVal;
        }

        public async Task<HttpResponseMessage> CheckToken()
        {
            HttpResponseMessage resp = await PostAfterTokenRequest(Statics.MethodNames.srvCheckPreToken);
            return resp;
        }
        public async Task<HttpResponseMessage> CreatePreToken()
        {
            HttpResponseMessage resp = await PostPreTokenRequest(Statics.MethodNames.srvCreatePreToken);
            return resp;
        }

        #endregion Token
    }
}
