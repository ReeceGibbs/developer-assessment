using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Infrastructure.Data.Models;
using TodoList.Service.Services;
using Xunit;

namespace TodoList.UnitTests.Service.Services
{
    public class TodoItemsServiceTests_GetTodoItemById : TodoItemsServiceTests_Base
    {
        public TodoItemsServiceTests_GetTodoItemById()
            : base()
        {
        }

        [Fact]
        public async Task GetTodoItemById_Should_Return_Valid_TodoItem_Given_A_Valid_Guid()
        {
            // Arrange
            var result = new TodoItem();
            var expectedResult = _mockTodoItems.First();
            var mockTodoItemGuid = _mockTodoItems.First().Id;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.GetTodoItemById(mockTodoItemGuid);
            }

            // Assert
            result.Should().BeEquivalentTo(expectedResult, "because we have passed the Guid of a TodoItem that exists in the TodoItems DBSet");
        }

        [Fact]
        public async Task GetTodoItemById_Should_Return_Null_Given_An_Invalid_Guid()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItemGuid = Guid.NewGuid();

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.GetTodoItemById(mockTodoItemGuid);
            }

            // Assert
            result.Should().BeNull("because we have passed the Guid of a TodoItem that does not exists in the TodoItems DBSet");
        }
    }
}
