using System.Threading.Tasks;
using Backend.Plugins.Database.Mapping;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Backend.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        MapsterMapperRules.InitMappingRules();
        IHost web = CreateHostBuilder(args).Build();

        await web.StartAsync();
        await web.WaitForShutdownAsync();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
}