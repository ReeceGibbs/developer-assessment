using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Models.Common;
using TodoList.Common.Models.TodoItem;
using TodoList.Infrastructure.Data.Models;
using Xunit;

namespace TodoList.UnitTests.Api.Controllers
{
    public class TodoItemsControllerTests_PostTodoItem : TodoItemsControllerTests_Base
    {
        public TodoItemsControllerTests_PostTodoItem()
            : base()
        {
        }

        [Fact]
        public async Task PostTodoItem_Should_Return_OK_And_Valid_ResponseObject_On_Success()
        {
            // Arrange
            var mockTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 0",
                IsCompleted = false
            };

            var expectedResult = new ResponseObject<TodoItem>
            {
                Success = true,
                Data = mockTodoItem
            };

            _todoItemsService.Setup(x => x.CreateTodoItem(It.IsAny<TodoItem>())).Returns(Task.FromResult(mockTodoItem));

            // Act
            var result = (ObjectResult)await _todoItemsController.PostTodoItem(new TodoItemRequestDto());

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.Created);
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
