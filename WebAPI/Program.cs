using Azure.Identity;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureAppConfiguration((hostingContext, config) =>
                    {
                        config.AddAzureKeyVault(new Uri("https://teamturtlekeyvault.vault.azure.net/"),
                            new DefaultAzureCredential(new DefaultAzureCredentialOptions { ManagedIdentityClientId = "439c3c1e-b179-447d-88c6-d7b6e8a08232" }));
                    });
                    webBuilder.UseStartup<Startup>();
                });
    }
}
