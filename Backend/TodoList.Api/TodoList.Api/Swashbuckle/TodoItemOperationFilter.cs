using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Api.Swashbuckle
{
    public class TodoItemOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "API_KEY",
                In = ParameterLocation.Header,
                Description = "Api Key"
            });
        }
    }
}
