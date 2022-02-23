using AspNetCoreRateLimit;
using HotelListing.Core.Configurations;
using HotelListing.Data;
using HotelListing.Core.Repository;
using HotelListing.Core.Auth;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using HotelListing.Core;
using Microsoft.Extensions.Logging;

namespace HotelListing
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // defining the database connection to be instantiate on startup and available globaly
            services.AddDbContext<DatabaseContext>(options =>
            {
                options.UseSqlServer(Configuration.GetConnectionString("sqlConnection"));
            });


            services.AddMemoryCache();

            // configure caching
            //services.AddResponseCaching(); // was moved to ConfigureHttpCacheHeaders
            services.ConfigureHttpCacheHeaders();

            services.ConfigureRateLimiting();
            services.AddHttpContextAccessor();

            // configure auth
            services.AddAuthentication();

            // configure Identity
            services.ConfigureIdentity();

            // Setting JWT validation
            services.ConfigureJWT(Configuration);

            services.AddCors(o =>
            {
                o.AddPolicy("CorsPolicy_AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            // initialise the AutoMapper calss
            services.AddAutoMapper(typeof(MapperInitialiser));

            // define the controller at startup
            services.AddTransient<IUnitOfWork, UnitOfWork > ();
            // register the interface for auth
            services.AddScoped<IAuthManager, AuthManager>();

            AddSwaggerDoc(services);

            services.AddControllers(config =>
            {
                config.CacheProfiles.Add("120SecondsDuration", new CacheProfile
                {
                    Duration = 120
                });
            }).AddNewtonsoftJson(op => 
                op.SerializerSettings.ReferenceLoopHandling = 
                    Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            // register API Version support
            services.ConfigureVersioning();
        }

        private void AddSwaggerDoc(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // add authentication section.
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = @"JWT Auth header using the Bearer schema. 
                                    Enter 'Bearer' [space] and then your token. 
                                    Example: 'Bearer 123sfsdf3fe",
                    Name = "Authorization", 
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme {
                            Reference = new OpenApiReference{
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "AOuth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header
                        },
                        new List<string> ()
                    }
                });

                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HotelListing", Version = "v1" });
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        // Register middleware 
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment() || env.IsProduction())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
//                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HotelListing v1"));
                app.UseSwaggerUI(c =>
                {
                    string swaggerJsonBasePath = string.IsNullOrWhiteSpace(c.RoutePrefix) ? "." : "..";
                    c.SwaggerEndpoint($"{swaggerJsonBasePath}/swagger/v1/swagger.json", "HotelListing v1");
                });
            }

            app.UseHttpsRedirection();
            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseCors("CorsPolicy_AllowAll");

            app.UseResponseCaching(); // register the middleare
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
