using System.Text.Json.Serialization;
using Backend.Libs.Caching.Extensions;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Plugins.Domain.Extensions;
using Backend.Plugins.gRPC.Extensions;
using Backend.Plugins.Messaging.Extensions;
using Backend.Plugins.Messaging.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Api;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }
    
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddOptions();
        services.AddHttpContextAccessor();
        services.AddJwtAuthentication();
        services.AddPermissionBasedAuthorization<PermissionType, RoleType>();

        services.AddEndpointsApiExplorer();
        services.TryAddRabbitMqClientFactory(Configuration);
        services.TryAddRedisKeyValueStorage();

        services.TryAddRabbitMqProducer<TestMessage>();

        services.AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });

        services.AddAuthSwagger("Backend.Api");
        services.AddGrpcDatabaseServices();
        services.AddCryptographyLibs();
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseCors(s =>
        {
            s.AllowAnyHeader();
            s.AllowAnyMethod();
            s.AllowAnyOrigin();
        });
        
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseSwagger();
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend.Api v1"));
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}