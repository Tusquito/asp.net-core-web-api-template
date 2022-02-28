using System.Text.Json.Serialization;
using Backend.Api.Authentication.Services.Account;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.gRPC.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;

namespace Backend.Api.Authentication
{
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

            services.AddEndpointsApiExplorer();

            services.AddControllers()
                .AddJsonOptions(x =>
                {
                    x.JsonSerializerOptions.Converters.Add(
                        new JsonStringEnumConverter());
                });

            services.AddAuthSwagger("Backend.Api.Authentication");
            services.AddGrpcDatabaseServerClients();

            services.TryAddTransient<IUserAuthenticationService, UserAuthenticationService>();

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
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Backend.Api.Authentication v1"));
            }

            app.UseHttpsRedirection();
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}