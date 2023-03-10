using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;

namespace Backend.Libs.Domain.Extensions;

public static class WebApplicationExtensions
{
    public static WebApplication UseDomainLibs(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();

        app.UseCors(x =>
        {
            x.AllowAnyOrigin();
            x.AllowAnyHeader();
            x.AllowAnyMethod();
        });

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        app.UseRequesterIpMiddleware();
        app.UseRequesterCultureMiddleware();

        return app;
    }
}