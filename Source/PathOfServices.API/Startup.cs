using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using PathOfServices.API.Hubs;
using PathOfServices.API.Middleware;
using PathOfServices.API.Services.Implementations;
using PathOfServices.API.Swagger;
using PathOfServices.Business;
using PathOfServices.Business.Configuration;
using PathOfServices.Business.Database;
using PathOfServices.Business.Services.Abstractions;
using PathOfServices.Business.Services.Implementations;

namespace PathOfServices.API
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        private IWebHostEnvironment WebHostEnvironment { get; }

        public Startup(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            Configuration = configuration;
            WebHostEnvironment = webHostEnvironment ?? throw new ArgumentNullException(nameof(webHostEnvironment));
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var config = Configuration.Get<PathOfServicesConfig>();

            if (config.ConnectionType == ConnectionType.INVALID)
            {
                Console.Error.WriteLine("Please specify a valid ConnectionType in your AppSettings");
                Environment.Exit(1);
            }

            if (string.IsNullOrEmpty(config.ConnectionString.Trim()))
            {
                Console.Error.WriteLine("Please specify a valid ConnectionString in your AppSettings");
                Environment.Exit(1);
            }

            services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.WithOrigins(config.Origin).AllowCredentials().AllowAnyHeader()));

            services.AddSingleton(config);
            services.AddLogging();
             
            // Business Services
            services
                .AddDbContext<PathOfServicesDbContext>(options =>
                {
                    _ = config.ConnectionType switch
                    {
                        ConnectionType.MSSQL => options.UseSqlServer(config.ConnectionString),
                        ConnectionType.POSTGRES => options.UseNpgsql(config.ConnectionString),
                        _ => throw new ArgumentOutOfRangeException(nameof(config.ConnectionType))
                    };
                });

            services
                .AddIdentityCore<UserEntity>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = false;
                    options.SignIn.RequireConfirmedEmail = false;
                    options.SignIn.RequireConfirmedPhoneNumber = false;
                    options.User.RequireUniqueEmail = false;
                })
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<PathOfServicesDbContext>();

            services.AddTransient<IDBMigrator, DBMigrator>();

            services.AddMemoryCache(mem => mem.SizeLimit = config.MemoryCacheSizeLimitBytes);

            services.AddControllers();

            services.AddSignalR();
            services.AddTransient<ITestEventHandler, TestEventHandler>();

            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Path of Services API",
                    Description =
@"Open API for Path of Services <br />
<a href='/api/oauth/authorize' target='_blank'>Click here to authenticate!</a> (Refresh this window after authenticating)",
                    TermsOfService = new Uri("https://raw.githubusercontent.com/SteffenBlake/PathOfServices/main/LICENSE"),
                    License = new OpenApiLicense
                    {
                        Name = "Use under MIT License",
                        Url = new Uri("https://raw.githubusercontent.com/SteffenBlake/PathOfServices/main/LICENSE")
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                gen.IncludeXmlComments(xmlPath);

                // Auto append "access_token" property to authorize queries
                gen.OperationFilter<AuthAccessTokenOperationFilter>();
            });

            services.AddTransient<OAuthTokenMiddleWare>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            // Auto migrate EFCore Database
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();

            var config = serviceScope.ServiceProvider.GetRequiredService<PathOfServicesConfig>();

            loggerFactory.AddFile(config.Logging.Path, config.Logging.MinimumLevel);

            var migrator = serviceScope.ServiceProvider.GetRequiredService<IDBMigrator>();
            migrator.ExecuteAsync().Wait();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();

            app.UseSwagger();

            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Path of Services API");
                swagger.RoutePrefix = "";
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseMiddleware<OAuthTokenMiddleWare>();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();

                endpoints.MapHub<TestHub>("/hubs" + TestHub.EndPoint);
            });
        }
    }
}
