using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Exceptions;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Service.Services;
using Xunit;

namespace TodoList.UnitTests.Service.Services
{
    public class TodoItemsServiceTests_DeleteTodoItem : TodoItemsServiceTests_Base
    {
        public TodoItemsServiceTests_DeleteTodoItem()
            : base()
        {
        }

        [Fact]
        public async Task DeleteTodoItem_Should_Succeed_And_Return_The_Correct_Guid_When_TodoItem_Exists()
        {
            // Arrange
            var result = new Guid();
            var mockTodoItemId = _mockTodoItems.First().Id;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.DeleteTodoItem(mockTodoItemId);
            }

            // Assert
            result.Should().Be(mockTodoItemId, "because we deleted an existing TodoItem");
        }

        [Fact]
        public void DeleteTodoItem_Should_Fail_And_Throw_NotFoundException_When_TodoItem_Does_Not_Exist()
        {
            // Arrange
            var result = new Guid();
            var mockTodoItemId = Guid.NewGuid();

            // Act
            var action = async () =>
            {
                using (var context = new TodoContext(_contextOptions))
                {
                    var todoItemsService = new TodoItemsService(context);
                    result = await todoItemsService.DeleteTodoItem(mockTodoItemId);
                }
            };

            //Assert
            action.Should().ThrowAsync<NotFoundException>("because we tried to delete a TodoItem that does not exist");
            result.Should().NotBe(mockTodoItemId, "because the DeleteTodoItem call failed");
        }
    }
}
