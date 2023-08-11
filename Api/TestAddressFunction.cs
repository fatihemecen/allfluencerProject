using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AdminPanel;
using Api;
using BlazorApp.Api;
using BlazorApp.Api.Filters;
using BlazorApp.Client;
using BlazorApp.Client.Data;
using BlazorApp.Client.Service;
using BlazorApp.Shared;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.WebJobs;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Objects;
using Util;
using static Util.Statics.Enums;

namespace ApiIsolated
{
    public class FnGetEmployees
    {
        mnGlobal mnGlobal;
        public FnGetEmployees(mnGlobal global)
        {
           
            mnGlobal = global;

        }

        /* [Function("TestAddress")]
         public async Task<List<TestAddress>> Run([HttpTrigger(AuthorizationLevel.Function, "get" , Route = "address")] HttpRequestData req)
         {
             var _address = new List<TestAddress>();
             try
             {
                 string connectionString = "Server=tcp:srvallfluencer.database.windows.net,1433;Initial Catalog=allfluencer;Persist Security Info=False;User ID=allfluenceradmin;Password=Evilsmurff1;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;";
                 SqlConnection connection = new SqlConnection((connectionString));

                     connection.Open();
                     var query = "SELECT * FROM [SalesLT].[Address]";
                     SqlCommand command = new SqlCommand(query, connection);
                     var reader = await command.ExecuteReaderAsync();
                     while (reader.Read())
                     {
                         TestAddress employee = new TestAddress()
                         {
                             AddresID = Convert.ToInt32(reader["AddressID"]),
                             AddressLine1 = Convert.ToString(reader["AddressLine1"]),
                             AddressLine2 = Convert.ToString(reader["AddressLine2"]),
                             City = Convert.ToString(reader["City"]),
                             StateProvince = Convert.ToString(reader["StateProvince"])
                         };
                         _address.Add(employee);
                     }

             }
             catch (Exception e)
             {
                 _logger.LogError(e.ToString());
             }

             return _address;
         }*/

        [Function("getUserList")]
        [FunctionTokenFilter]
        [Obsolete]
        public async Task<HttpResponseData> GetUserList(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {


            List<coUser> list = await mnGlobal.busAsyncNew.GetUserList();
            var response = req.CreateResponse(HttpStatusCode.OK);
            response.WriteAsJsonAsync(list);
            
            return response;
        }
        [Function("CreateUser")]
        [FunctionTokenFilter]
        [Obsolete]
        public async Task<IActionResult> CreateUser(
      [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
      ILogger log)
        {

            var content = await new StreamReader(req.Body).ReadToEndAsync();


            coUser myClass = JsonConvert.DeserializeObject<coUser>(content);

   
                                   if ((new Regex(@"^[A-Za-z][A-Za-z0-9]{5,13}$")).Match(myClass.UserName).Success)
                                   {
                                       if ((new Regex(@"^\p{L}{2,}$")).Match(myClass.FirstName).Success)
                                       {
                                           if ((new Regex(@"^\p{L}{2,}$")).Match(myClass.LastName).Success)
                                           {
                                               if (GeneralUtility.IsValidEmail(myClass.UserEmail))
                                               {
                                                   if ((new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?.!@$%^&*-]).{8,}$")).Match(myClass.UserPassword).Success)
                                                   {
                                                       bool chkRes = await mnGlobal.busAsyncNew.CheckUser(myClass.UserEmail, myClass.UserName);
                                                        if (!chkRes)
                                                        {
                                                            string res = await mnGlobal.busAsyncNew.CreateUser(myClass.UserName, myClass.FirstName, myClass.LastName, myClass.UserEmail, myClass.UserPassword, 1);
                                                            Console.WriteLine(res);
                                                            if (!string.IsNullOrEmpty(res))
                                                            {

                                                            var response = req.CreateResponse(HttpStatusCode.OK);
                                                            await response.WriteAsJsonAsync(myClass);
                                                            return new OkObjectResult(res);
                                                            }
                                                            else
                                                            {
                                                            return new BadRequestObjectResult(((int)UserRegisterResult.ErrorOccured).ToString());
                                                            }
                                                        }
                                                        else
                                                        {
                                                        Console.WriteLine("Bu kullanýcý var");
                                                        return new BadRequestObjectResult((chkRes).ToString());
                                                        }
                                                   }

                                               }
                                           }
                                       }
                                   }
        return new BadRequestObjectResult(((int)UserRegisterResult.DataHasProblem).ToString());
        }

        
        [Function("CheckPreToken")]
        [FunctionTokenFilter]
        [Obsolete]
        public async Task<IActionResult> CheckPreTokenFunction(
           [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            /*if (!FunctionHelper.CheckHeaderAccepted(req))
            {
                return new UnauthorizedResult();
            }*/


            string jwt = FunctionHelper.GetTokenHeader(req);

            return new OkObjectResult(jwt);
        }

        [Function("CreatePreToken")]
        [FunctionPreTokenFilter]
        [Obsolete]
        public async Task<IActionResult> CreatePreToken(
        [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req)
        {
            /*if (!FunctionHelper.CheckHeaderAccepted(req))
            {
                return new UnauthorizedResult();
            }*/

            coJWTToken token = new coJWTToken();
            string jwt = FunctionHelper.GenerateTokenJWT(token);

            return new OkObjectResult(jwt);
        }
        [Function("LoginUser")]
        [FunctionTokenFilter]
        [Obsolete]
        public async Task<IActionResult> LoginUser(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequestData req,
            ILogger log)
        {
            /* if (!FunctionHelper.CheckHeaderAccepted(req))
             {
                 return new UnauthorizedResult();
             }*/

            var content = await new StreamReader(req.Body).ReadToEndAsync();


            coUser myClass = JsonConvert.DeserializeObject<coUser>(content);



            string jwtToken = await mnGlobal.busAsyncNew.LoginUser(myClass.UserEmail, myClass.UserPassword);
            if (!string.IsNullOrEmpty(jwtToken))
            {
                return new OkObjectResult(jwtToken);
            }


            return new BadRequestObjectResult(((int)UserRegisterResult.DataHasProblem).ToString());

        }

    }
        
}



