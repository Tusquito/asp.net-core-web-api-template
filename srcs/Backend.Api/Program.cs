using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace Backend.Api;

public class Program
{
    public static async Task Main(string[] args)
    {
        PrintHeader();
        IHost web = CreateHostBuilder(args).Build();
        

        await web.StartAsync();
        await web.WaitForShutdownAsync();
    }
    
    private static void PrintHeader()
    {
        const string text = @"
██████╗  █████╗  ██████╗██╗  ██╗███████╗███╗   ██╗██████╗     
██╔══██╗██╔══██╗██╔════╝██║ ██╔╝██╔════╝████╗  ██║██╔══██╗    
██████╔╝███████║██║     █████╔╝ █████╗  ██╔██╗ ██║██║  ██║    
██╔══██╗██╔══██║██║     ██╔═██╗ ██╔══╝  ██║╚██╗██║██║  ██║    
██████╔╝██║  ██║╚██████╗██║  ██╗███████╗██║ ╚████║██████╔╝    
╚═════╝ ╚═╝  ╚═╝ ╚═════╝╚═╝  ╚═╝╚══════╝╚═╝  ╚═══╝╚═════╝     
                                                              
 ██████╗ ██████╗ ██████╗ ███████╗     █████╗ ██████╗ ██╗      
██╔════╝██╔═══██╗██╔══██╗██╔════╝    ██╔══██╗██╔══██╗██║      
██║     ██║   ██║██████╔╝█████╗      ███████║██████╔╝██║      
██║     ██║   ██║██╔══██╗██╔══╝      ██╔══██║██╔═══╝ ██║      
╚██████╗╚██████╔╝██║  ██║███████╗    ██║  ██║██║     ██║      
 ╚═════╝ ╚═════╝ ╚═╝  ╚═╝╚══════╝    ╚═╝  ╚═╝╚═╝     ╚═╝    

                                                                                       
";
        string separator = new string('=', Console.WindowWidth);
        string logo = text.Split('\n').Select(s => string.Format("{0," + (Console.WindowWidth / 2 + s.Length / 2) + "}\n", s))
            .Aggregate("", (current, i) => current + i);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine(separator + logo + $"Version: {Assembly.GetExecutingAssembly().GetName().Version}\n" + separator);
        Console.ForegroundColor = ConsoleColor.White;
    }

    private static IHostBuilder CreateHostBuilder(string[] args) {
        IHostBuilder host = Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
                webBuilder.UseUrls("http://*:6661");
            });
        return host;
    }
}