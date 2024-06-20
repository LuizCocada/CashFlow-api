using System.Globalization;

namespace CashFlow.Api.Middleware;

public class CultureMiddleware
{
    private readonly RequestDelegate _next;

    public CultureMiddleware( RequestDelegate next )
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context )
    {
        var supportedLanguages = CultureInfo.GetCultures(CultureTypes.AllCultures).ToList();

        var RequestedCulture = context.Request.Headers.AcceptLanguage.FirstOrDefault();



        var cultureInfo = new CultureInfo("en");

        if(string.IsNullOrWhiteSpace(RequestedCulture) == false && supportedLanguages.Exists(language => language.Name.Equals(RequestedCulture)))
        {
            cultureInfo = new CultureInfo(RequestedCulture);
        }

        CultureInfo.CurrentCulture = cultureInfo;
        CultureInfo.CurrentUICulture = cultureInfo;

        await _next(context);
    }
}
