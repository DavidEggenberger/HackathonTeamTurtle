using Azure;
using Azure.AI.TextAnalytics;
using Domain;
using IdentityServer4.Models;
using Infrastructure.Persistance;
using Microsoft.AspNetCore.ApiAuthorization.IdentityServer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAPI.Hubs;

namespace WebAPI
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private readonly IWebHostEnvironment webHostEnvironment;
        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            this.webHostEnvironment = webHostEnvironment;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddSignalR();
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.AllowAnyOrigin()
                          .AllowAnyMethod()
                          .AllowAnyHeader()
                          .Build();
                });
            });
            services.AddScoped<TextAnalyticsClient>(sp => new TextAnalyticsClient(new Uri(Configuration["AzureKeyVaultCognitiveServicesSEndpoint"]), new AzureKeyCredential(Configuration["AzureKeyVaultCognitiveServicesKey"])));

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(Configuration["AzureKeyVaultSQLConnection"]);
            });

            #region Authenentication Setup
            services.AddAuthentication(options =>
            {
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                options.DefaultChallengeScheme = IdentityServerJwtConstants.IdentityServerJwtBearerScheme;
                options.DefaultAuthenticateScheme = "ApplicationDefinedAuthentication";
            })
               .AddIdentityServerJwt()
               .AddGoogle(options =>
               {
                   options.ClientId = Configuration["AzureKeyVaultGoogleClientId"];
                   options.ClientSecret = Configuration["AzureKeyVaultGoogleClientSecret"];
               })
               .AddDiscord(options => 
               {
                   options.ClientId = Configuration["AzureKeyVaultDiscordClientId"];
                   options.ClientSecret = Configuration["AzureKeyVaultDiscordClientSecret"];
               })
               .AddGitHub(options => 
               {
                   options.ClientId = Configuration["AzureKeyVaultGitHubClientId"];
                   options.ClientSecret = Configuration["AzureKeyVaultGitHubClientSecret"];
               })
               .AddPolicyScheme("ApplicationDefinedAuthentication", null, options =>
               {
                   options.ForwardDefaultSelector = (context) =>
                   {
                       if (context.Request.Path.StartsWithSegments(new PathString("/api"), StringComparison.OrdinalIgnoreCase))
                           return IdentityServerJwtConstants.IdentityServerJwtScheme;
                       else
                           return IdentityConstants.ApplicationScheme;
                   };
               })
               .AddIdentityCookies(options =>
               {
               });
            services.ConfigureApplicationCookie(config =>
            {
                config.LoginPath = "/Login";
                config.LogoutPath = "/Logout";
            });

            var identityService = services.AddIdentityCore<ApplicationUser>(options =>
            {
                options.User.AllowedUserNameCharacters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+/ ";
                options.User.RequireUniqueEmail = false;
                options.Tokens.AuthenticatorIssuer = "TeamTurtle";
                options.Stores.MaxLengthForKeys = 128;
            })
                .AddDefaultTokenProviders()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            identityService.AddSignInManager();
            #endregion
            #region IdentityServer Registration
            if (webHostEnvironment.IsDevelopment())
            {
                services.AddIdentityServer()
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                {
                    options.Clients.Add(new Client
                    {
                        ClientId = "BlazorClient",
                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        RequireClientSecret = false,
                        AllowedScopes = new List<string>
                        {
                            "openid",
                            "profile",
                            "API"
                        },
                        RedirectUris = { "https://localhost:44328/authentication/login-callback" },
                        PostLogoutRedirectUris = { "https://localhost:44328" },
                        FrontChannelLogoutUri = "https://localhost:44328"
                    });
                    options.ApiResources = new ApiResourceCollection
                    {
                        new ApiResource
                        {
                            Name = "API",
                            Scopes = new List<string> {"API"}
                        }
                    };
                    var cert = options.SigningCredential = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["AzureKeyVaultSecretKey"])), SecurityAlgorithms.HmacSha256);
                });
            }
            if (webHostEnvironment.IsProduction())
            {
                services.AddIdentityServer(options =>
                {

                })
                .AddApiAuthorization<ApplicationUser, ApplicationDbContext>(options =>
                {
                    options.Clients.Add(new Client
                    {
                        ClientId = "BlazorClient",
                        AllowedGrantTypes = GrantTypes.Code,
                        RequirePkce = true,
                        RequireClientSecret = false,
                        AllowedScopes = new List<string>
                        {
                            "openid",
                            "profile",
                            "API"
                        },
                        RedirectUris =
                            {
                                "https://teamturtles.azurewebsites.net/authentication/login-callback"
                            },
                        PostLogoutRedirectUris =
                            {
                                "https://teamturtles.azurewebsites.net"
                            },
                        FrontChannelLogoutUri = "https://teamturtles.azurewebsites.net"
                    });
                    options.ApiResources = new ApiResourceCollection
                    {
                        new ApiResource
                        {
                            Name = "API",
                            Scopes = new List<string> {"API"}
                        }
                    };
                    var cert = options.SigningCredential = new SigningCredentials(new SymmetricSecurityKey(Encoding.ASCII.GetBytes(Configuration["SigningKey"])), SecurityAlgorithms.HmacSha256);
                });
            }
            #endregion
            services.Configure<JwtBearerOptions>(IdentityServerJwtConstants.IdentityServerJwtBearerScheme, options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidAudience = "API"
                };
                var originalOnMessageReceived = options.Events.OnMessageReceived;
                options.Events.OnMessageReceived = async context =>
                {
                    await originalOnMessageReceived(context);

                    if (string.IsNullOrEmpty(context.Token))
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;

                        if (!string.IsNullOrEmpty(accessToken) &&
                            path.Value.EndsWith("hub"))
                        {
                            context.Token = accessToken;
                        }
                    }
                };
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors();

            app.UseIdentityServer();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<UserHub>("/hub");
                endpoints.MapControllers();
                endpoints.MapRazorPages();
                endpoints.MapFallbackToFile("index.html");
            });
        }
    }
}
