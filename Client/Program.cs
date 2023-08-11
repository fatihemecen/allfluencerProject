using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BlazorApp.Client;
using Api;
using Blazored.LocalStorage;
using BlazorApp.Client.Service;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.Configuration["API_Prefix"] ?? builder.HostEnvironment.BaseAddress) });
builder.Services.AddScoped<ServiceUser>();
builder.Services.AddScoped<mnUserManager>();
builder.Services.AddScoped<mnTokenManager>();
builder.Services.AddScoped<mnServiceManager>();



await builder.Build().RunAsync();
