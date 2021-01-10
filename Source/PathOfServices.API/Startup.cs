using System;
using System.IO;
using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
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

            if (WebHostEnvironment.IsDevelopment())
            {
                services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.AllowAnyOrigin()));
            }
            else
            {
                services.AddCors(cors => cors.AddDefaultPolicy(policy => policy.WithOrigins(config.AllowedOrigins)));
            }

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

            services.AddTransient<IDBMigrator, DBMigrator>();

            services.AddControllers();
            services.AddSwaggerGen(gen =>
            {
                gen.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Path of Services API",
                    Description = "Open API for Path of Services",
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
            });
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
                app.UseCors(cors => cors.AllowAnyOrigin());
            }
            else
            {
                app.UseCors(cors => cors.WithOrigins(config.AllowedOrigins));
            }

            app.UseSwagger();

            app.UseSwaggerUI(swagger =>
            {
                swagger.SwaggerEndpoint("/swagger/v1/swagger.json", "Path of Services API");
                swagger.RoutePrefix = "";
            });

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
