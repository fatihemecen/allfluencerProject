using BlazorApp.Shared;
using Newtonsoft.Json;
using Util;

namespace BlazorApp.Client.Service
{
    public class ServiceAddress
    {
        private mnServiceManager mnService { get; set; }
        public ServiceAddress(mnServiceManager _mnService)
        {
            mnService = _mnService;
        }
        public async Task<List<TestAddress>> GetAddressAll(long lastupdatetick)
        {
            Dictionary<string, string> pars = new Dictionary<string, string>();
            pars.Add("lastupdatetick", lastupdatetick.ToString());

            List<TestAddress> res = await mnService.PostAfterTokenRequest<List<TestAddress>>(Statics.MethodNames.srvGetAddressAll, pars);
            return res;
        }
    }
}
