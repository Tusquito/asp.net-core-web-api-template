using System.Text.Json.Serialization;
using Backend.Libs.Caching.Extensions;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Database.Account;
using Backend.Libs.Domain.Enums;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Messaging.Extensions;
using Backend.Libs.Messaging.Messages;
using Backend.Plugins.Domain.Extensions;
using Backend.Plugins.gRPC.Extensions;

namespace Backend.Api.Tests;

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

        services.AddAuthedSwagger(typeof(Startup).Namespace!);
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