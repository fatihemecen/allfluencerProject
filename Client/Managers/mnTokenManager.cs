using BlazorApp.Shared;
using Blazored.LocalStorage;
using Util;
using static Util.Statics.Enums;

namespace BlazorApp.Client
{
    public class mnTokenManager
    {
        public coJWTToken Token { get; set; }
        private ILocalStorageService LocalStorage { get; set; }
        public mnTokenManager(ILocalStorageService _localStorage)
        {
            LocalStorage = _localStorage;
        }
        public async Task<TokenCheckStatus> PrepareToken()
        {
            if (Token == null)
            {
                bool hasToken = await LocalStorage.ContainKeyAsync(Statics.SystemKeys.skJwt);
                if (hasToken)
                {
                    string jwt = await LocalStorage.GetItemAsync<string>(Statics.SystemKeys.skJwt);
                    Token = new coJWTToken(jwt);
                    return TokenCheckStatus.CreatedFromLocal;
                }
                else
                {
                    return TokenCheckStatus.NotExist;
                }
            }
            else
            {
                return TokenCheckStatus.AlreadyCreated;
            }
        }
    }
}
