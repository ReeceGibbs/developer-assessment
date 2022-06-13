using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Net;
using TodoList.Common.Builders;
using TodoList.Common.Exceptions;
using TodoList.Common.Models.Common;

namespace TodoList.Api.Filters
{
    // referenced from jason taylor's clean architecture example https://github.com/jasontaylordev/CleanArchitecture/blob/main/src/WebUI/Filters/ApiExceptionFilterAttribute.cs
    public class ApiExceptionFilterAttribute: ExceptionFilterAttribute
    {
        private readonly IDictionary<Type, Action<ExceptionContext>> _exceptionHandlers;

        public ApiExceptionFilterAttribute()
        {
            _exceptionHandlers = new Dictionary<Type, Action<ExceptionContext>>
            {
                { typeof(ValidationException), HandleValidationException },
                { typeof(NotFoundException), HandleNotFoundException },
                { typeof(UnauthorizedAccessException), HandleUnauthorizedAccessException },
                { typeof(DescriptionExistsException), HandleDescriptionExistsException }
            };
        }

        public override void OnException(ExceptionContext context)
        {
            HandleException(context);

            base.OnException(context);
        }

        private void HandleException(ExceptionContext context)
        {
            Type type = context.Exception.GetType();
            if (_exceptionHandlers.ContainsKey(type))
            {
                _exceptionHandlers[type].Invoke(context);
                return;
            }

            if (!context.ModelState.IsValid)
            {
                HandleInvalidModelStateException(context);
                return;
            }
        }

        private void HandleValidationException(ExceptionContext context)
        {
            var exception = (ValidationException)context.Exception;

            var details = new ValidationProblemDetails(exception.Errors)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = ResponseBuilder.Build(HttpStatusCode.BadRequest, details, false);
            context.ExceptionHandled = true;
        }

        private void HandleInvalidModelStateException(ExceptionContext context)
        {
            var details = new ValidationProblemDetails(context.ModelState)
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
            };

            context.Result = ResponseBuilder.Build(HttpStatusCode.BadRequest, details, false);
            context.ExceptionHandled = true;
        }

        private void HandleNotFoundException(ExceptionContext context)
        {
            var exception = (NotFoundException)context.Exception;

            var details = new ProblemDetails
            {
                Detail = exception.Message,
                Title = "The specified resource was not found.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            };

            context.Result = ResponseBuilder.Build(HttpStatusCode.NotFound, details, false);
            context.ExceptionHandled = true;
        }

        private void HandleUnauthorizedAccessException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status401Unauthorized,
                Title = "Unauthorized",
                Type = "https://tools.ietf.org/html/rfc7235#section-3.1"
            };

            context.Result = ResponseBuilder.Build(HttpStatusCode.Unauthorized, details, false);

            context.ExceptionHandled = true;
        }

        private void HandleDescriptionExistsException(ExceptionContext context)
        {
            var details = new ProblemDetails
            {
                Status = StatusCodes.Status409Conflict,
                Title = "Description exists",
                Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.8"
            };

            context.Result = ResponseBuilder.Build(HttpStatusCode.Conflict, details, false);

            context.ExceptionHandled = true;
        }
    }
}
