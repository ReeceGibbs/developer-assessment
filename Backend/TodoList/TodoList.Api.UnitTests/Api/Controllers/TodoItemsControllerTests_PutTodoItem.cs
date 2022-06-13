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
    public class TodoItemsControllerTests_PutTodoItem : TodoItemsControllerTests_Base
    {
        public TodoItemsControllerTests_PutTodoItem()
            : base()
        {
        }

        [Fact]
        public async Task PutTodoItem_Should_Return_OK_And_Valid_ResponseObject_On_Success()
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

            _todoItemsService.Setup(x => x.UpdateTodoItem(It.IsAny<Guid>(), It.IsAny<TodoItem>())).Returns(Task.FromResult(mockTodoItem));

            // Act
            var result = (ObjectResult)await _todoItemsController.PutTodoItem(Guid.NewGuid(), new TodoItemRequestDto());

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
