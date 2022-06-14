using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net;
using TodoList.Common.Builders;
using TodoList.Common.Exceptions;

namespace TodoList.Api.Filters
{
    public class ApiAuthFilterAttribute : Attribute, IAuthorizationFilter
    {
        private readonly string _apiKey = "API_KEY";

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var apiKey = appSettings.GetValue<string>(_apiKey);

            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKey, out var requestApiKey) || apiKey != requestApiKey)
            {
                throw new UnauthorizedAccessException();
            }
        }
    }
}
