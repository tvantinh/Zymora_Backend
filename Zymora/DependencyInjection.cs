using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Zymora.Models.Settings;
using Zymora.Services.Implementations;
using Zymora.Services.Interfaces;
using Zymora_BE.Contract.Services.IService;
using Zymora_BE.Middleware.Middleware;
using Zymora_BE.Repositories.DataContext;
using Zymora_BE.Services;
using Zymora_BE.Services.Service;
namespace Zymora
{
    public static class DependencyInjection
    {

        public static void AddConfig(this IServiceCollection services, IConfiguration configuration)
        {
            services.ConfigRoute();
            services.AddDatabase(configuration);
            services.AddServicesMain(configuration);
            services.AddInfrastructure(configuration);
            string? secretKey = configuration["JWT:secretKey"];
            if (string.IsNullOrEmpty(secretKey))
                throw new ArgumentNullException(nameof(secretKey), "JWT secret key configuration is missing or empty.");
            services.AddJwtAuthentication(secretKey);
            services.AddSwaggerGen(); 
            
    }
        public static void ConfigRoute(this IServiceCollection services)
        {
            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
            });
        }
        public static void AddDatabase(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<DatabaseContext>(options =>
            {
              options.UseLazyLoadingProxies()
                 .UseSqlServer(
                      configuration.GetConnectionString("ZymoraDb"),
                      sqlOptions =>
                      {
                        sqlOptions.MigrationsAssembly("Zymora");
                        sqlOptions.EnableRetryOnFailure(
                              maxRetryCount: 5,                         
                              maxRetryDelay: TimeSpan.FromSeconds(10), 
                              errorNumbersToAdd: null          
                          );
                 });
            });
            
        }
        public static void AddServicesMain(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<JWTSettings>(configuration.GetSection("JWT")); 
            services.AddSingleton(sp => sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<JWTSettings>>().Value);
            services.AddScoped<IJWTService, JWTService>();
        }
        public static IApplicationBuilder UseAPIResponseWrapperMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ResponseWrapperMiddleware>();
        }

        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, string secretKey)
        {
          services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
              .AddJwtBearer(options =>
              {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                  ValidateIssuer = false,
                  ValidateAudience = false,
                  ValidateLifetime = true,
                  IssuerSigningKey = new SymmetricSecurityKey(
                          Encoding.UTF8.GetBytes(secretKey))
                };
              });

          services.AddAuthorization();

          return services;
        }
        public static void AddSwaggerGen(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Zymora API", Version = "v1" });
                c.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = "bearer"
                });
                c.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
                {
                    {
                        new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                        {
                            Reference = new Microsoft.OpenApi.Models.OpenApiReference
                            {
                                Type=Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                                Id="Bearer"
                            }
                        },
                        new string[]{}
                    }
                });
            });
    }
  }
}