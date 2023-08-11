using Microsoft.Azure.WebJobs.Host;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Util;

namespace BlazorApp.Api.Filters
{
    [Obsolete]
    public class FunctionAuthorizeAttribute: FunctionInvocationFilterAttribute
    {
        public FunctionAuthorizeAttribute()
        {
        }

        public override Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            var workItem = executingContext.Arguments.First().Value as HttpRequestMessage;
            //ValidationPackage validationPackage = new ValidationPackage();
            bool res = false;
            AuthenticationHeaderValue jwtInput = workItem.Headers.Authorization;
            if (jwtInput != null)
            {
                String jwt = "";
                if (jwtInput.ToString().StartsWith("Bearer ", StringComparison.OrdinalIgnoreCase))
                {
                    jwt = jwtInput.ToString().Substring("Bearer ".Length).Trim();
                }
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                try
                {
                    //validationPackage = ExtractClaims(jwt, handler);
                    res = ExtractClaims(jwt, handler);
                }
                catch (Exception ex)
                {
                    
                }
            }
            if (res/*!validationPackage.ValidToken*/)
            {
                workItem.Headers.Add(Statics.SystemKeys.skAuthorizationStatus, Convert.ToInt32(HttpStatusCode.Unauthorized).ToString());
            }
            else
            {
                workItem.Headers.Add(Statics.SystemKeys.skAuthorizationStatus, Convert.ToInt32(HttpStatusCode.Accepted).ToString());
            }

            return base.OnExecutingAsync(executingContext, cancellationToken);
        }
        public static bool ExtractClaims(string jwt, JwtSecurityTokenHandler handler)
        {
            //ValidationPackage validationPackage = new ValidationPackage();
            //validationPackage.Token = jwt;
            var token = handler.ReadJwtToken(jwt);
            //validationPackage.Scope = "user_impersonation";
            try
            {

                var claims = token.Claims;
                foreach (Claim c in claims)
                {
                    /*switch (c.Type)
                    {
                        case "sub":
                        case "upn":
                            if (c.Value.Contains('@'))
                                validationPackage.PrincipalName = c.Value;
                            break;
                        case "Firstname":
                            validationPackage.FirstName = c.Value;
                            break;
                        case "Lastname":
                            validationPackage.LastName = c.Value;
                            break;
                        case "client_id":
                        case "aud":
                            validationPackage.AppID = c.Value;
                            break;
                        case "iat":
                            validationPackage.IssuedAt = Convert.ToInt64(c.Value);
                            break;
                        case "exp":
                            validationPackage.ExpiresAt = Convert.ToInt64(c.Value);
                            break;
                        case "scp":
                            validationPackage.Scope = c.Value;
                            break;
                    }*/
                }
            }
            catch (Exception e)
            {
                return false;
                //validationPackage.ValidToken = false;
            }
            var currentTimestamp = (long)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds;

            /*if ((validationPackage.ExpiresAt - currentTimestamp) > 0)
                validationPackage.ValidToken = true;
            return validationPackage;*/

            return true;
        }
    }
}
