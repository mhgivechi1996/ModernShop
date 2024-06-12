using Catalog.Domain.Core.AggregatesModel.FeatureAggregate;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Text;

namespace Catalog.API.Configurations
{
    public static class MainSetup
    {
        public static void RegisterDI(this IServiceCollection services, IConfiguration configuration)
        {
            DatabaseSetup.AddDatabaseSetup(services, configuration);
            RepositoriesSetup.AddRepositorySetup(services);
            ServicesSetup.AddServicesSetup(services);

            #region Add Authorize Button in Swagger

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "ModernShop Catalog API",
                    Description = "Catalog App API - Version01",
                    TermsOfService = new Uri("https://ModernShop.ir/"),
                    License = new OpenApiLicense
                    {
                        Name = "ModernShop",
                        Url = new Uri("https://ModernShop.ir/"),
                    }
                });

                var security = new Dictionary<string, IEnumerable<string>>
                {
                    {"Bearer", new string[] { }},
                };

                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "Bearer"
                    },
                    Scheme = "Bearer",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    new string[] { }
                }
                });

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
            });
            #endregion

            #region Add Check JWT Bearer Token

            var tokenTimeOut = configuration.GetValue<int>("Configs:TokenTimeOut");
            var tokenKey = Encoding.UTF8.GetBytes(configuration.GetValue<string>("Configs:TokenKey"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                      .AddJwtBearer(x =>
                      {
                          x.RequireHttpsMetadata = false;
                          x.SaveToken = true;
                          x.TokenValidationParameters = new TokenValidationParameters
                          {
                              ClockSkew = TimeSpan.FromMinutes(tokenTimeOut),
                              ValidateLifetime = true,
                              ValidateIssuerSigningKey = true,
                              IssuerSigningKey = new SymmetricSecurityKey(tokenKey),
                              ValidateIssuer = false,
                              ValidateAudience = false
                          };
                      });


            #endregion

        }
    }
}
