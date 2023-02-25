using System.Text.Json.Serialization;
using Backend.Libs.Caching.Extensions;
using Backend.Libs.Cryptography.Extensions;
using Backend.Libs.Domain.Extensions;
using Backend.Libs.Mediator.Extensions;
using Backend.Plugins.Domain.Extensions;
using Backend.Plugins.gRPC.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Backend.Api.Authentication
{
    public class Startup
    {
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

            services.AddAuthSwagger(typeof(Startup).Namespace!);
            
            services.AddGrpcDatabaseServices();

            services.AddCryptographyLibs();
            services.AddDomainLibs();
            services.AddMediatorLibs<Startup>();
            
            services.TryAddRedisKeyValueStorage();
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
            app.UseRequesterIpMiddleware();
            app.UseRequesterCultureMiddleware();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}