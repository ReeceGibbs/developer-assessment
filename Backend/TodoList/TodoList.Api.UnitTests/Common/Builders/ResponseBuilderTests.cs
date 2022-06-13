using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Builders;
using TodoList.Common.Models.Common;
using TodoList.Infrastructure.Data.Models;
using Xunit;

namespace TodoList.UnitTests.Common.Builders
{
    public class ResponseBuilderTests
    {
        [Fact]
        public void ResponseBuilder_Should_Return_Valid_Response_Data_On_Success()
        {
            // Arrange
            var mockStatusCode = HttpStatusCode.OK;
            var mockData = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 0",
                IsCompleted = false
            };

            var expectedResult = new ResponseObject<TodoItem>
            {
                Success = true,
                Data = mockData
            };

            // Act
            var response = ResponseBuilder.Build(mockStatusCode, mockData, true);

            // Assert
            response.StatusCode.Should().Be((int)mockStatusCode);
            response.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Fact]
        public void ResponseBuilder_Should_Return_Valid_ErrorDetails_On_Failure()
        {
            // Arrange
            var mockStatusCode = HttpStatusCode.NotFound;
            var mockErrorDetails = new ProblemDetails
            {
                Detail = "NotFound",
                Title = "The specified resource was not found.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4",
            };

            var expectedResult = new ResponseObject<ProblemDetails> 
            {
                Success = false,
                Data = mockErrorDetails
            };

            // Act
            var response = ResponseBuilder.Build(mockStatusCode, mockErrorDetails, false);

            // Assert
            response.StatusCode.Should().Be((int)mockStatusCode);
            response.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
