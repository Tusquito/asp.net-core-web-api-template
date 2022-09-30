using Backend.Libs.gRPC.Account;
using Backend.Plugins.Database.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProtoBuf.Grpc.Server;

namespace Backend.Api.Database;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    private IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {
        services.AddCors();

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
        app.UseCors(s =>
        {
            s.AllowAnyHeader();
            s.AllowAnyMethod();
            s.AllowAnyOrigin();
        });

        app.UseDeveloperExceptionPage();
        
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapGrpcService<GrpcAccountService>();
        });
    }
}