using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BlazorApp.Shared
{
    public class coJWTToken
    {
        public string UserName { get; set; } = "";
        public string FirstName { get; set; } = "";
        public string LastName { get; set; } = "";
        public string Email { get; set; } = "";

        public long UserRole { get; set; } = 0;
        public string JWTContent { get; set; } = "";
        public coJWTToken(string jwt)
        {
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtSecurityToken = handler.ReadJwtToken(jwt);

            UserName = jwtSecurityToken.Claims.First(claim => claim.Type == "UserName").Value;
            FirstName = jwtSecurityToken.Claims.First(claim => claim.Type == "FirstName").Value;
            LastName = jwtSecurityToken.Claims.First(claim => claim.Type == "LastName").Value;
            Email = jwtSecurityToken.Claims.First(claim => claim.Type == "Email").Value;
            UserRole = Int64.Parse(jwtSecurityToken.Claims.First(claim => claim.Type == "UserRole").Value);

            //Claim clmUserState = jwtSecurityToken.Claims.First(claim => claim.Type == "UserState");
            //string userstate = clmUserState != null ? clmUserState.Value : "0";
            //UserState = Int64.Parse(userstate);

            JWTContent = jwt;
        }
        public coJWTToken()
        {
            
        }
    }
}
