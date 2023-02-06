using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Backend.Libs.Domain.Middlewares;

public class RequesterCultureMiddleware
{
    private readonly RequestDelegate _next;

    public RequesterCultureMiddleware(RequestDelegate next)
    {
        _next = next; 
    }

    public async Task InvokeAsync(HttpContext context)
    {
        string currentCultureIso = "en-US";

        if (context.Request.Headers.ContainsKey("Accept-Language") && context.Request.Headers["Accept-Language"].Any())
        {
            currentCultureIso = context.Request.Headers["Accept-Language"].First()!.Split(',').First();
        }
        
        CultureInfo culture = new CultureInfo(currentCultureIso);

        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;
        
        await _next(context);
    }
}