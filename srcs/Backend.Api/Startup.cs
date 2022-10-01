using System.Text.Json.Serialization;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Redis.Extensions;
using Backend.Plugins.Domain.Extensions;
using Backend.Plugins.gRPC.Extensions;
using Backend.Plugins.RabbitMQ.Extensions;
using Backend.Plugins.RabbitMQ.Messages;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Api;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();
        services.AddOptions();
        services.AddHttpContextAccessor();
        services.AddJwtAuthentication();

        services.AddEndpointsApiExplorer();
        services.AddRabbitMqClientFactoryFromEnv();
        services.TryAddRedisKeyValueStorage();

        services.AddRabbitMqProducer<TestMessage>();

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
        app.UseAuthorization();
        app.UseAuthentication();
        
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });
    }
}