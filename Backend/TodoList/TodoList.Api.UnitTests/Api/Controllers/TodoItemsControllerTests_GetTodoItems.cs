using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Models.Common;
using TodoList.Infrastructure.Data.Models;
using Xunit;

namespace TodoList.UnitTests.Api.Controllers
{
    public class TodoItemsControllerTests_GetTodoItems : TodoItemsControllerTests_Base
    {
        public TodoItemsControllerTests_GetTodoItems()
            : base()
        {
        }

        [Fact]
        public async Task GetTodoItems_Should_Return_OK_And_Valid_ResponseObject_On_Success()
        {
            // Arrange
            var mockTodoItemsList = new List<TodoItem>
            {
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Test 0",
                    IsCompleted = false
                },
                new TodoItem
                {
                    Id = Guid.NewGuid(),
                    Description = "Test 1",
                    IsCompleted = true
                }
            };

            var expectedResult = new ResponseObject<List<TodoItem>>
            {
                Success = true,
                Data = mockTodoItemsList
            };

            _todoItemsService.Setup(x => x.GetTodoItemsList()).Returns(Task.FromResult(mockTodoItemsList));

            // Act
            var result = (ObjectResult)await _todoItemsController.GetTodoItems();

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
