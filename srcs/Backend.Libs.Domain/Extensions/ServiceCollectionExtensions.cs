using System.Text;
using System.Text.Json.Serialization;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Options;
using Backend.Libs.Domain.Services.Cryptography;
using Backend.Libs.Domain.Services.Cryptography.Abstractions;
using Backend.Libs.Persistence.Enums;
using FluentValidation;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Backend.Libs.Domain.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCryptographyLibs(this IServiceCollection services)
    {
        // BcryptPasswordHasher
        services.AddTransient<IPasswordHasherService, BCryptPasswordHasherService>();
        return services;
    }
    
    
    public static IServiceCollection AddDomainLibs(this IServiceCollection services, string apiName, string version = "v1")
    {
        services.AddCors();
        services.AddHttpContextAccessor();
        services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

        services.AddCryptographyLibs();
        services.AddAuthedSwagger(apiName, version);
        services.AddJwtAuthentication();
        services.AddValidatorsFromAssemblyContaining(typeof(FluentValidationOptions<>));
        return services;
    }

    public static IServiceCollection AddAuthedSwagger(this IServiceCollection services, string apiName, string version = "v1")
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = apiName,
                Version = version
            });
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "Please insert JWT with Bearer into field",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = JwtBearerDefaults.AuthenticationScheme
            });
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services)
    {
        services.AddAuthentication(s =>
        {
            s.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            s.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(s =>
        {
            s.RequireHttpsMetadata = false;
            s.SaveToken = true;
            s.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                    Environment.GetEnvironmentVariable("JWT_SIGNATURE_KEY")?.ToSha512() ?? "123456789".ToSha512())),
                ValidateIssuer = false,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidIssuer = "backend",
                ValidAudience = "backend",
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddPermissionBasedAuthorization<PermissionType, RoleType>();
        
        return services;
    }
}