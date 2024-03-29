using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebClient
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            builder.Services.AddHttpClient("UnauthenticatedHttpClient", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress));
            builder.Services.AddHttpClient("WebAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
               .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();
            // Supply HttpClient instances that include access tokens when making requests to the server project
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("WebAPI"));

            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Services.AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.Authority = "https://localhost:44328/";
                    options.ProviderOptions.ClientId = "BlazorClient";
                    options.ProviderOptions.ResponseType = "code";
                    options.ProviderOptions.DefaultScopes.Add("API");
                    options.AuthenticationPaths.LogOutCallbackPath = "/";
                    options.AuthenticationPaths.LogOutPath = "https://localhost:44328/logout";
                    options.AuthenticationPaths.RemoteProfilePath = "https://localhost:44328/profile";
                    options.AuthenticationPaths.RemoteRegisterPath = "https://localhost:44328/login";
                });
            }
            if (builder.HostEnvironment.IsProduction())
            {
                builder.Services.AddOidcAuthentication(options =>
                {
                    options.ProviderOptions.Authority = "https://teamturtles.azurewebsites.net/";
                    options.ProviderOptions.ClientId = "BlazorClient";
                    options.ProviderOptions.ResponseType = "code";
                    options.ProviderOptions.DefaultScopes.Add("API");
                    options.AuthenticationPaths.LogOutCallbackPath = "/";
                    options.AuthenticationPaths.LogOutPath = "https://teamturtles.azurewebsites.net/logout";
                    options.AuthenticationPaths.RemoteProfilePath = "https://teamturtles.azurewebsites.net/profile";
                    options.AuthenticationPaths.RemoteRegisterPath = "https://teamturtles.azurewebsites.net/login";
                });
            }

            await builder.Build().RunAsync();
        }
    }
}
