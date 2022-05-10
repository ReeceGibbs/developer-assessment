using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Middleware
{
    public class RequestAuthMiddleware
    {
        private readonly RequestDelegate _next;
        const string APIKEY = "API_KEY";

        public RequestAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            if (!httpContext.Request.Headers.TryGetValue(APIKEY, out var requestApiKey))
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(ResponseExtensions<object>.FailureResponse(HttpStatusCode.Unauthorized, "Authentication failed: Missing key"));

                return;
            }

            var appSettings = httpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(APIKEY);

            if (apiKey != requestApiKey)
            {
                httpContext.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                await httpContext.Response.WriteAsJsonAsync(ResponseExtensions<object>.FailureResponse(HttpStatusCode.Unauthorized, "Authentication failed: Invalid key"));

                return;
            }

            await _next(httpContext);
        }
    }
}
