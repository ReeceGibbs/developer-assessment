using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Builders;
using TodoList.Common.Exceptions;

namespace TodoList.Common.Behaviours
{
    public class ExceptionHandlingBehaviour
    {
        private readonly RequestDelegate _next;
        private readonly JsonSerializerSettings _jsonSerializerSettings;
        private readonly IDictionary<Type, Func<HttpContext, Exception, Task>> _exceptionHandlers;

        public ExceptionHandlingBehaviour(RequestDelegate next)
        {
            _next = next;
            _exceptionHandlers = new Dictionary<Type, Func<HttpContext, Exception, Task>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(DescriptionExistsException), HandleDescriptionExistsException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException }
            };

            _jsonSerializerSettings = new JsonSerializerSettings();
            _jsonSerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                HandleException(context, exception);
            }
        }

        private void HandleException(HttpContext context, Exception exception)
        {
            Type type = exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                context.Response.ContentType = "application/json";
                _exceptionHandlers[type].Invoke(context, exception);
                return;
            }
        }

        private async Task HandleValidationException(HttpContext context, Exception exception)
        {
            var validationException = (ValidationException)exception;

            var details = new ValidationProblemDetails(validationException.Errors)
            {
                Status = (int)HttpStatusCode.BadRequest,
                Title = "Validation exception",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            var result = JsonConvert.SerializeObject(ResponseBuilder.Build(HttpStatusCode.BadRequest, details, false).Value, _jsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }

        private async Task HandleUnauthorizedAccessException(HttpContext context, Exception exception)
        {
            var details = new ProblemDetails
            {
                Status = (int)HttpStatusCode.Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };

            var result = JsonConvert.SerializeObject(ResponseBuilder.Build(HttpStatusCode.Unauthorized, details, false).Value, _jsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }

        private async Task HandleNotFoundException(HttpContext context, Exception exception)
        {
            var notFoundException = (NotFoundException)exception;

            var details = new ProblemDetails
            {
                Status = (int)HttpStatusCode.NotFound,
                Title = "The specified resource was not found",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                Detail = notFoundException.Message,
            };

            var result = JsonConvert.SerializeObject(ResponseBuilder.Build(HttpStatusCode.NotFound, details, false).Value, _jsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }

        private async Task HandleDescriptionExistsException(HttpContext context, Exception exception)
        {
            var details = new ProblemDetails
            {
                Status = (int)HttpStatusCode.Conflict,
                Title = "Description exists",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8"
            };

            var result = JsonConvert.SerializeObject(ResponseBuilder.Build(HttpStatusCode.Conflict, details, false).Value, _jsonSerializerSettings);
            await context.Response.WriteAsync(result);
        }
    }
}
