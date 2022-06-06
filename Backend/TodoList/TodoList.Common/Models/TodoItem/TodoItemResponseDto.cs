using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Common.Models.TodoItem
{
    public class TodoItemResponseDto<T>
    {
        public bool Success { get; set; }
        public T? Data { get; set; }
        public KeyValuePair<string, string>? Error { get; set; }
    }

    public static class TodoItemResponseBuilder
    {
        public static ObjectResult SuccessResponse<T>(HttpStatusCode statusCode, T data)
        {
            var response = new TodoItemResponseDto<T>();
            response.Success = true;
            response.Data = data;

            return new ObjectResult(response) { StatusCode = (int)statusCode };
        }

        public static ObjectResult FailureResponse(HttpStatusCode statusCode, string errorMessage)
        {
            var response = new TodoItemResponseDto<object>();
            response.Success = false;
            response.Error = new KeyValuePair<string, string>(statusCode.ToString(), errorMessage);

            return new ObjectResult(response) { StatusCode = (int)statusCode };
        }
    }
}
