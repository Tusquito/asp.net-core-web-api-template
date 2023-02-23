using System.Text.Json.Serialization;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Redis.Extensions;
using Backend.Plugins.Domain.Extensions;
using Backend.Plugins.gRPC.Extensions;
using Backend.Plugins.RabbitMQ.Extensions;
using Backend.Plugins.RabbitMQ.Messages;

namespace Backend.Api.Tests;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddOptions();
        services.AddHttpContextAccessor();
        services.AddJwtAuthentication();
        services.AddPermissionBasedAuthorization<PermissionType, RoleType>();

        services.AddEndpointsApiExplorer();
        services.AddRabbitMqClientFactoryFromEnv();
        services.TryAddRedisKeyValueStorage();

        services.AddRabbitMqPublisher<TestMessage>();

        services.AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });

        services.AddAuthSwagger(typeof(Startup).Namespace!);
        services.AddGrpcDatabaseServices();
        services.AddCryptographyLibs();
        services.AddDomainLibs();
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
            app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend.Api.Tests v1"));
        }
        
        app.UseHttpsRedirection();
        app.UseRouting();
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}