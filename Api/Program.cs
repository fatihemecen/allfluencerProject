using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Azure.Functions.Worker.Configuration;
using Api;
using Microsoft.Extensions.DependencyInjection;
using BlazorApp.Client.Service;
using Microsoft.AspNetCore.Http;
using BlazorApp.Client;
using Blazored.LocalStorage;
using Microsoft.Extensions.Azure;
using System.Net.Http;
using Microsoft.JSInterop;

namespace ApiIsolated
{
    public class Program
    {
        public static void Main()
        {
            var host = new HostBuilder()
                .ConfigureFunctionsWorkerDefaults().ConfigureServices((context, services) =>
                {
                    ConfigureServices(context.Configuration, services);
                })
                .Build();

            host.Services.GetRequiredService<mnGlobal>();





            host.Run();
        }
        public static void ConfigureServices(IConfiguration configuration,
    IServiceCollection services)
        {

            services.AddSingleton<mnGlobal>();
            services.AddBlazoredLocalStorage();
            services.AddAuthentication();





        }
    }
}