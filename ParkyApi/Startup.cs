using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using ParkyApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ParkyApi.Models.Repository.IRepository;
using ParkyApi.Models.Repository;
using AutoMapper;
using ParkyApi.Models.Mapper;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using ParkyApi.Configurations;
using Swashbuckle.AspNetCore.SwaggerGen;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ParkyApi
{
    public enum DataBaseNames
    {
        ParkyDb
    }


    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors();

            services.AddAutoMapper(typeof(ParkyMappings));

            services.AddControllers();

            #region Interface Services
            //Repository
            services.AddScoped<INationalParkRepository, NationalParkRepository>();
            services.AddScoped<ITrailRepository, TrailRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ITokenRepository, TokenRepository>();
            #endregion

            #region DB
            ////DbConnection
            services.AddDbContext<ParkyDbContext>(options =>
            options.UseSqlServer(_config.GetConnectionString(DataBaseNames.ParkyDb.ToString())));
            #endregion

            #region Swagger
            //Swagger 
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("NationalParks",
                    new OpenApiInfo
                    {
                        Title = "ParkyApi NP",
                        Version = "v1",
                        Description = "Demo api",
                        Contact = new OpenApiContact()
                        {
                            Email = "AramYako@hotmail.com",
                            Name = "Aram Yako"
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "MIT Linces"
                        }
                    });

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please insert JWT with Bearer into field",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name ="Bearer",
                            In =ParameterLocation.Header
                        },
                        new string[] { }
                    }
                });
            });


            //services.AddSwaggerGen(c =>
            //{
            //    c.SwaggerDoc("ParkyTrail", new OpenApiInfo { Title = "ParkyApi Trails", Version = "v1" });
            //});

            //Version
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true; //DefaultApiVersion is used to set the default version to API
                options.DefaultApiVersion = new ApiVersion(1, 0); //This flag AssumeDefaultVersionWhenUnspecified flag is used to set the default version when the client has not specified any versions.
                options.ReportApiVersions = true;   //To return the API version in response header
            });

            #endregion

            #region IOptions
            services.Configure<AppSettings>(_config.GetSection(AppSettings.AppSettingsSection));
            #endregion

            #region JWT 
            var jwtTokenConfig = _config.GetSection(AppSettings.AppSettingsSection).Get<AppSettings>();

            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.Audience = string.Empty;
                    options.Authority = string.Empty;
                    options.AutomaticRefreshInterval = TimeSpan.FromDays(1);
                    options.RequireHttpsMetadata = false; //Set to true in development mode 
                    options.SaveToken = true;

                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtTokenConfig.Secret)),
                        ValidateIssuerSigningKey = true,
                        ValidateLifetime = true,
                        ValidateIssuer = false, //Set domain during production
                        ValidateAudience = false,  //Set domain during production
                        ClockSkew = TimeSpan.FromMinutes(1)

                        //ValidIssuer = jwtTokenConfig.Issuer,
                        //ValidAudience = jwtTokenConfig.Audience,

                    };

                });
            #endregion
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env /*ApiVersionDescriptionProvider provider*/)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();

                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/NationalParks/swagger.json", "My API V1");
                    //options.SwaggerEndpoint("/swagger/ParkyTrail/swagger.json", "Parky Api Trails v1");
                });
            }

            app.UseRouting();

            //Authentication //JWT
            app.UseAuthentication();

            app.UseCors(x =>
                x.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader()
            );

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
