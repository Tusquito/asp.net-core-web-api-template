using System.Text.Json.Serialization;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.gRPC.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Backend.Api;

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
        services.AddOptions();
        services.AddHttpContextAccessor();

        services.AddJwtAuthentication();

        services.AddControllers()
            .AddJsonOptions(x =>
            {
                x.JsonSerializerOptions.Converters.Add(
                    new JsonStringEnumConverter());
            });

        services.AddAuthSwagger();
        services.AddGrpcDatabaseServerClients();

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

        app.UseDeveloperExceptionPage();
        app.UseSwagger();
        app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend.Api v1"));

        app.UseHttpsRedirection();
        app.UseRouting();
        app.UseAuthorization();
        app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
    }
}