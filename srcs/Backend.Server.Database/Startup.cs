using Backend.Plugins.Database.Extensions;
using Backend.Plugins.gRPC.Account;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Server;

namespace Backend.Server.Database;

public class Startup
{
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddControllers();

        services.AddBackendDatabasePlugin();
        
        services.AddCodeFirstGrpc(config =>
        {
            config.MaxReceiveMessageSize = null;
            config.MaxSendMessageSize = null;
            config.EnableDetailedErrors = true;
        });
    }
    
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GrpcAccountService>();
        });
    }
}