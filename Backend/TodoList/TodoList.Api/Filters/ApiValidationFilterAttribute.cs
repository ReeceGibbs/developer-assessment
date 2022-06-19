using FluentValidation;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Common.Models.TodoItem;
using CustomExceptions = TodoList.Common.Exceptions;

namespace TodoList.Api.Filters
{
    public class ApiValidationFilterAttribute : IAsyncActionFilter
    {
        private readonly IValidatorFactory _validatorFactory;

        public ApiValidationFilterAttribute(IValidatorFactory validatorFactory)
        {
            _validatorFactory = validatorFactory;
        }

        private ValidationContext<T> GetValidationContext<T>(T validationObject) => new ValidationContext<T>(validationObject);

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var incomingObjects = context.ActionArguments.Values;

            foreach (var incomingObject in incomingObjects)
            {
                var validator = _validatorFactory.GetValidator(incomingObject.GetType());

                if (validator != null)
                {
                    var validationContext = GetValidationContext(incomingObject);

                    var result = await validator.ValidateAsync(validationContext);

                    if (result.Errors.Any())
                    {
                        throw new CustomExceptions.ValidationException(result.Errors);
                    }
                }
            }

            await next();
        }
    }
}
