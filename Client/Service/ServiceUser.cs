using Api;
using BlazorApp.Shared;
using Newtonsoft.Json;
using Objects;
using Util;
using static Util.Statics.Enums;

namespace BlazorApp.Client.Service
{
    public class ServiceUser
    {
        private mnServiceManager mnService { get; set; }
        public ServiceUser(mnServiceManager _mnService)
        {
            mnService = _mnService;
        }
        public async Task<List<coUser>> GetUserList()
        {
            Dictionary<string, string> pars = new Dictionary<string, string>();

            List<coUser> res = await mnService.PostAfterTokenRequest<List<coUser>>(Statics.MethodNames.srvGetUserList, pars);
            return res;
        }

        public async Task<string> Register(coUser user)
        {
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("UserName", user.UserName);
            pars.Add("FirstName", user.FirstName);
            pars.Add("LastName", user.LastName);
            pars.Add("UserEmail", user.UserEmail);
            pars.Add("UserPassword", user.UserPassword);
            pars.Add("UserRole", Convert.ToString(user.UserRole));

            string res = await mnService.PostAfterTokenRequest<string>(Statics.MethodNames.srvCreateUser, pars);
            return res;

            /*HttpResponseMessage resp = await mnService.PostAfterTokenRequest(Statics.MethodNames.srvRegister, pars);

            if (resp.IsSuccessStatusCode)
            {
                string jwtToken = resp.Content.ReadAsStringAsync().Result;
                return jwtToken;
            }
            else
            {
                string resCode = resp.Content.ReadAsStringAsync().Result;
                return resCode;
            }*/
        }

        /*public async Task<ApiResponse<int>> CreateUser(string userName ,string userFirstName, string userLastName, string userPassword, int userType, string userEmail, DateTime dateTime, string userGuid)
        {
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("userName", userFirstName);
            pars.Add("userFirstName", userFirstName);
            pars.Add("userLastName", userLastName);
            pars.Add("Password", userPassword);
            pars.Add("userType", userType.ToString());
            pars.Add("userEmail", userEmail);
            pars.Add("registerTime", dateTime.ToString());
            pars.Add("userGuid", userGuid);
            ApiResponse<int> resp = await mnService.GetMethod<ApiResponse<int>>("CreateUser", pars);

            return resp;
        }*/
    }
}
