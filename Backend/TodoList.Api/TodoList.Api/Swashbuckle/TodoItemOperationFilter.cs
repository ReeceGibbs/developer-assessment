using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Collections.Generic;
using TodoList.Api.Models;

namespace TodoList.Api.Swashbuckle
{
    public class TodoItemOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            if (operation.OperationId == "TodoItemPut" || operation.OperationId == "TodoItemPost")
            {
                operation.Parameters.Clear();

                operation.RequestBody = new OpenApiRequestBody()
                {
                    Content = new Dictionary<string, OpenApiMediaType>()
                    {
                        {
                            "application/json",
                            new OpenApiMediaType
                            {
                                Schema = context.SchemaGenerator.GenerateSchema(typeof(TodoItem), context.SchemaRepository)
                            }
                        }
                    }
                };
            }

            operation.Parameters.Add(new OpenApiParameter
            {
                Name = "ApiKey",
                In = ParameterLocation.Header,
                Description = "Api Key"
            });
        }
    }
}
