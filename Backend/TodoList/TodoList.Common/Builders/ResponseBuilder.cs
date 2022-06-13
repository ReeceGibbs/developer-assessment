using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Models.Common;

namespace TodoList.Common.Builders
{
    public static class ResponseBuilder
    {
        public static ObjectResult Build<T>(HttpStatusCode statusCode, T data, bool success)
        {
            var response = new ResponseObject<T>();
            response.Success = success;
            response.Data = data;

            return new ObjectResult(response) { StatusCode = (int)statusCode };
        }
    }
}
