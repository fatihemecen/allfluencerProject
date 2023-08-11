using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Util;

namespace Objects
{
    public class ApiResponse<T>
    {
        public string Success { get; set; }
        public string ErrorMessage { get; set; }
        public string ErrorCode { get; set; }
        public T ReturnObject { get; set; }
        public string DateTick { get; set; }
        public string Token { get; set; }
        public object Tag { get; set; }

        public ApiResponse()
        {
            DateTick = DateTime.UtcNow.AddMinutes(-1).Ticks.ToString();
        }
        public static ApiResponse<string> CreateInvalidCallResponse()
        {
            ApiResponse<string> resp = new ApiResponse<string>();
            resp.Success = "0";
            resp.ErrorMessage = Statics.ErrorCodes.FailedCall;

            return resp;
        }

        public static ApiResponse<T> CreateSuccessResponse(T result)
        {
            ApiResponse<T> resp = new ApiResponse<T>();
            resp.Success = "1";
            resp.ErrorMessage = String.Empty;
            resp.ReturnObject = result;

            return resp;
        }

        public static ApiResponse<string> CreateFailedResponse(string errorcode, string errormessage)
        {
            ApiResponse<string> resp = new ApiResponse<string>();
            resp.Success = "0";
            resp.ErrorMessage = errormessage;
            resp.ErrorCode = errorcode;

            return resp;
        }

        public static ApiResponse<string> CreateResponseByResult(bool res)
        {
            ApiResponse<string> resp = new ApiResponse<string>();
            if (res)
            {
                resp.Success = "1";
            }
            else
            {
                resp.Success = "0";
            }
            resp.ErrorMessage = String.Empty;
            resp.ReturnObject = null;
            return resp;
        }
    }
}
