using BlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Primitives;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace BlazorApp.Api
{
    public class FunctionHelper
    {
        public static bool CheckHeaderAccepted(HttpRequest req)
        {
            try
            {
                StringValues authorizationStatuesValues;
                req.Headers.TryGetValue(Statics.SystemKeys.skAuthorizationStatus, out authorizationStatuesValues);

                if (authorizationStatuesValues.Count > 0 && Convert.ToInt32(authorizationStatuesValues.FirstOrDefault()).Equals((int)HttpStatusCode.Accepted))
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

            }

            return false;
        }
        public static coJWTToken GetJWTFromRequest(HttpRequest req)
        {
            try
            {
                StringValues authorizationStatuesValues;
                req.Headers.TryGetValue(Statics.SystemKeys.skTokenHeader, out authorizationStatuesValues);

                if (authorizationStatuesValues.Count > 0)
                {
                    string jwt = authorizationStatuesValues.First();
                    return new coJWTToken(jwt);
                }
            }
            catch (Exception ex)
            {

            }

            return null;
        }
        public static bool CheckJWTEmail(HttpRequest req, string email)
        {
            coJWTToken jWTToken = GetJWTFromRequest(req);
            if (jWTToken != null && jWTToken.Email != null && jWTToken.Email == email)
            {
                return true;
            }

            return false;
        }
        public static string GenerateTokenJWT(coJWTToken token)
        {
            return GenerateTokenJWT(token.UserName, token.FirstName, token.LastName, token.Email,token.UserRole);
        }
        public static string GenerateTokenJWT(coUser user)
        {
            return GenerateTokenJWT(user.UserName, user.FirstName, user.LastName, user.UserEmail,
                user.UserRole);
        }
        public static string GenerateTokenJWT(string username, string firstname, string lastname,
            string email,  long userrole)
        {
            SymmetricSecurityKey mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(ApiStatics.FunctionStrings.JWTSecret));

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim("UserName", username),
                    new Claim("FirstName", firstname),
                    new Claim("LastName", lastname),
                    new Claim("Email", email),
                    new Claim("UserRole", userrole.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(ApiStatics.FunctionStrings.JWTExpireDay),
                Issuer = ApiStatics.FunctionStrings.JWTIssuer,
                Audience = ApiStatics.FunctionStrings.JWTIssuer,
                SigningCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature)
            };

            SecurityToken jwtToken = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(jwtToken);
        }
        public static string GetTokenHeader(HttpRequest req)
        {
            string jwt = req.Headers[Statics.SystemKeys.skTokenHeader];

            return jwt;
        }
        [Obsolete]
        public static void FilterCheck(FunctionExecutingContext executingContext, bool isPreToken)
        {
            var workItem = executingContext.Arguments.First().Value as HttpRequest;
            var syncIOFeature = workItem.HttpContext.Features.Get<IHttpBodyControlFeature>();
            if (syncIOFeature != null)
            {
                syncIOFeature.AllowSynchronousIO = true;
            }

            bool isSuccess = false;

            try
            {
                workItem.EnableBuffering();

                var jsonContent = "";
                using (StreamReader reader = new StreamReader(workItem.Body, Encoding.UTF8, true, 1024, true))
                {
                    jsonContent = reader.ReadToEnd();
                }

                string jwt = workItem.Headers[Statics.SystemKeys.skTokenHeader];
                //coToken token = JsonConvert.DeserializeObject<coToken>(jsonToken);

                if (isPreToken || (!isPreToken && TokenUtility.ValidateJWT(jwt)))
                {
                    workItem.Body.Position = 0;

                    Dictionary<string, string> values = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

                    if (values != null)
                    {
                        List<string> allparams = new List<string>();
                        List<string> keys = values.Keys.ToList();
                        keys = keys.OrderBy(x => x).ToList();
                        string key = "";
                        string calldatetick = "";

                        foreach (string item in keys)
                        {
                            string val = values[item];

                            if (item.ToLower() != Statics.SystemKeys.skKey)
                            {
                                if (val != null)
                                {
                                    allparams.Add(val.ToString());
                                }
                                else
                                {
                                    allparams.Add("");
                                }
                            }
                            else if (item.ToLower() == Statics.SystemKeys.skKey)
                            {
                                key = val;
                            }

                            if (item.ToLower() == Statics.SystemKeys.skServiceCallDateTick)
                            {
                                calldatetick = val;
                            }
                        }

                        DateTime date = new DateTime(Int64.Parse(calldatetick));

                        if (TokenUtility.CompareKeys(allparams, key.ToString())
                            && allparams.Count > 0
                            && DateTime.UtcNow.AddMinutes(-10) < date)
                        {
                            isSuccess = true;
                        }
                    }
                }


                if (isSuccess)
                {
                    workItem.Headers.Add(Statics.SystemKeys.skAuthorizationStatus, Convert.ToInt32(HttpStatusCode.Accepted).ToString());
                }
                else
                {
                    workItem.Headers.Add(Statics.SystemKeys.skAuthorizationStatus, Convert.ToInt32(HttpStatusCode.Unauthorized).ToString());
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [Obsolete]
        public static Dictionary<string, string> GetParams(HttpRequest req)
        {
            try
            {
                var jsonContent = "";
                using (StreamReader reader = new StreamReader(req.Body, Encoding.UTF8, true, 1024, true))
                {
                    jsonContent = reader.ReadToEnd();
                }

                Dictionary<string, string> pars = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonContent);

                Dictionary<string, string> retVal = new Dictionary<string, string>();

                if (pars != null)
                {
                    foreach (string item in pars.Keys)
                    {
                        retVal.Add(item, pars[item]);
                    }
                }

                retVal.Remove(Statics.SystemKeys.skTempParam);
                retVal.Remove(Statics.SystemKeys.skServiceCallDateTick);
                retVal.Remove(Statics.SystemKeys.skKey);

                foreach (string item in retVal.Keys)
                {
                    retVal[item] = Base32.DecodeString(retVal[item]);
                }

                return retVal;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
