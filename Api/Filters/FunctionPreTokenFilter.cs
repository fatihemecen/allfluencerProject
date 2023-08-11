using BlazorApp.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Util;

namespace BlazorApp.Api.Filters
{
    [Obsolete]
    public class FunctionPreTokenFilter : FunctionInvocationFilterAttribute
    {
        public FunctionPreTokenFilter()
        {

        }

        public override Task OnExecutingAsync(FunctionExecutingContext executingContext, CancellationToken cancellationToken)
        {
            FunctionHelper.FilterCheck(executingContext, true);

            return base.OnExecutingAsync(executingContext, cancellationToken);
        }
    }
}
