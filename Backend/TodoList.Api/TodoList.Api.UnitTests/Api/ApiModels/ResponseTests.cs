using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.ApiModels;
using TodoList.Infrastructure.Data.Models;
using Xunit;

namespace TodoList.UnitTests.Api.ApiModels
{
    public class ResponseTests
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

            var expectedValue = new TodoItemResponseDto<TodoItem>()
            {
                Success = true,
                Data = mockData,
                Error = null
            };

            var response = TodoItemResponseBuilder.SuccessResponse(mockStatusCode, mockData);

            Assert.Equal(200, response.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedValue),
                JsonConvert.SerializeObject(response.Value));
        }

        [Fact]
        public void FailureResponse()
        {
            var mockStatusCode = HttpStatusCode.InternalServerError;
            var mockErrorMessage = "Database access error";

            var expectedValue = new TodoItemResponseDto<TodoItem>()
            {
                Success = false,
                Data = null,
                Error = new KeyValuePair<string, string>(mockStatusCode.ToString(), mockErrorMessage)
            };

            var response = TodoItemResponseBuilder.FailureResponse(mockStatusCode, mockErrorMessage);

            Assert.Equal(500, response.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedValue),
                JsonConvert.SerializeObject(response.Value));
        }
    }
}
