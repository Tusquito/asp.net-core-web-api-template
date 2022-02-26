using System;
using System.Threading.Tasks;
using Backend.Plugins.Database.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Hosting;

namespace Backend.Api.Database;

public class Program
{
    public static async Task Main(string[] args)
    {
        MapsterMapperRules.InitMappingRules();
        IHost web = CreateHostBuilder(args).Build();

        await web.StartAsync();
        await web.WaitForShutdownAsync();
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        IHostBuilder host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.ConfigureKestrel(s =>
                {
                    s.ListenAnyIP(short.Parse(Environment.GetEnvironmentVariable("DATABASE_SERVER_PORT") ?? "19999"), 
                        options => { options.Protocols = HttpProtocols.Http2; });
                });
                webBuilder.UseStartup<Startup>();
            });
        return host;
    }
}