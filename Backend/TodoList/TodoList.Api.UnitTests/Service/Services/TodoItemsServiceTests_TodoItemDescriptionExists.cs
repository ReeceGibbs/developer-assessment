using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Service.Services;
using Xunit;

namespace TodoList.UnitTests.Service.Services
{
    public class TodoItemsServiceTests_TodoItemDescriptionExists : TodoItemsServiceTests_Base
    {
        public TodoItemsServiceTests_TodoItemDescriptionExists()
            : base()
        {
        }

        [Fact]
        public async Task TodoItemDescriptionExists_Should_Return_False_When_A_Completed_TodoItem_Contains_The_Description_Passed()
        {
            // Arrange
            var result = new bool();
            var mockDescription = _mockTodoItems.ElementAt(1).Description;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.TodoItemDescriptionExists(mockDescription);
            }

            // Assert
            result.Should().BeFalse("because the description we are looking for is only found in completed TodoItems");
        }

        [Fact]
        public async Task TodoItemDescriptionExists_Should_Return_True_When_An_Incomplete_TodoItem_Contains_The_Description_Passed()
        {
            // Arrange
            var result = new bool();
            var mockDescription = _mockTodoItems.First().Description;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.TodoItemDescriptionExists(mockDescription);
            }

            // Assert
            result.Should().BeTrue("because the description we are looking for is found in an incomplete TodoItem");
        }

        [Fact]
        public async Task TodoItemDescriptionExists_Should_Return_False_When_The_Same_TodoItem_Contains_The_Description_Passed()
        {
            // Arrange
            var result = new bool();
            var mockTodoItemId = _mockTodoItems.ElementAt(1).Id;
            var mockDescription = _mockTodoItems.ElementAt(1).Description;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.TodoItemDescriptionExists(mockDescription, mockTodoItemId);
            }

            // Assert
            result.Should().BeFalse("because the description we are looking for is only contained in the TodoItem associated with the Guid we provided as an argument");
        }

        [Fact]
        public async Task TodoItemDescriptionExists_Should_Return_True_When_Another_Incomplete_TodoItem_Contains_The_Description_Passed()
        {
            // Arrange
            var result = new bool();
            var mockDescription = _mockTodoItems.First().Description;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.TodoItemDescriptionExists(mockDescription, Guid.NewGuid());
            }

            // Assert
            result.Should().BeTrue("because the description we are looking for is contained in a TodoItem associated with a Guid other than the one we provided as an argument");
        }
    }
}
