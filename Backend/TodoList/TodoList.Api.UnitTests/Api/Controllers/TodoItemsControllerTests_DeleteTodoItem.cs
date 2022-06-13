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
using Xunit;

namespace TodoList.UnitTests.Api.Controllers
{
    public class TodoItemsControllerTests_DeleteTodoItem : TodoItemsControllerTests_Base
    {
        public TodoItemsControllerTests_DeleteTodoItem()
            : base()
        {
        }

        [Fact]
        public async Task DeleteTodoItem_Should_Return_OK_And_Valid_ResponseObject_On_Success()
        {
            // Arrange
            var mockTodoItemGuid = Guid.NewGuid();

            var expectedResult = new ResponseObject<Guid>
            {
                Success = true,
                Data = mockTodoItemGuid
            };

            _todoItemsService.Setup(x => x.DeleteTodoItem(It.IsAny<Guid>())).Returns(Task.FromResult(mockTodoItemGuid));

            // Act
            var result = (ObjectResult)await _todoItemsController.DeleteTodoItem(Guid.NewGuid());

            // Assert
            result.StatusCode.Should().Be((int)HttpStatusCode.OK);
            result.Value.Should().BeEquivalentTo(expectedResult);
        }
    }
}
