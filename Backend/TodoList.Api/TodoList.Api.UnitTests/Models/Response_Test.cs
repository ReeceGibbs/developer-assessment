using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.Models;
using Xunit;

namespace TodoList.Api.UnitTests.Models
{
    public class Response_Test
    {
        [Fact]
        public void SuccessResponse()
        {
            var mockStatusCode = HttpStatusCode.OK;
            var mockData = new TodoItem()
            {
                Id = Guid.NewGuid(),
                Description = "Test 0",
                IsCompleted = false
            };

            var expectedValue = new Response<TodoItem>()
            {
                Success = true,
                Data = mockData,
                Error = null
            };

            var response = ResponseExtensions<TodoItem>.SuccessResponse(mockStatusCode, mockData);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedValue),
                JsonConvert.SerializeObject(response.Value));
        }

        [Fact]
        public void FailureResponse()
        {
            var mockStatusCode = HttpStatusCode.InternalServerError;
            var mockErrorMessage = "Database access error";

            var expectedValue = new Response<TodoItem>()
            {
                Success = false,
                Data = null,
                Error = new KeyValuePair<string, string>(mockStatusCode.ToString(), mockErrorMessage)
            };

            var response = ResponseExtensions<TodoItem>.FailureResponse(mockStatusCode, mockErrorMessage);

            Assert.Equal(500, response.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedValue),
                JsonConvert.SerializeObject(response.Value));
        }
    }
}
