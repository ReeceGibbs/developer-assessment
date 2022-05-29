using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Net;

namespace TodoList.Api.ApiModels
{
    public class Response<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public KeyValuePair<string, string>? Error { get; set; }
    }

    public static class ResponseExtensions<T>
    {
        public static ObjectResult SuccessResponse(HttpStatusCode statusCode, T data)
        {
            Response<T> response = new Response<T>();
            response.Success = true;
            response.Data = data;

            return new ObjectResult(response) { StatusCode = (int)statusCode };
        }

        public static ObjectResult FailureResponse(HttpStatusCode statusCode, string errorMessage)
        {
            Response<T> response = new Response<T>();
            response.Success = false;
            response.Error = new KeyValuePair<string, string>(statusCode.ToString(), errorMessage);

            return new ObjectResult(response) { StatusCode = (int)statusCode };
        }
    }
}
