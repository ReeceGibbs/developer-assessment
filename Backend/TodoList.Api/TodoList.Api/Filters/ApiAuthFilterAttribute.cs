using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace TodoList.Api.Filters
{
    public class ApiAuthFilterAttribute : IActionFilter
    {
        private readonly string _apiKey = "API_KEY";

        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKey, out var requestApiKey))
            {
                throw new UnauthorizedAccessException("Missing API key");
            }

            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(_apiKey);

            if (apiKey != requestApiKey)
            {
                throw new UnauthorizedAccessException("Invalid API key");
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
        }
    }
}
