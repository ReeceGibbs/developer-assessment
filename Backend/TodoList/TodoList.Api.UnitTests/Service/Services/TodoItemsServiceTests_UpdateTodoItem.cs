using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Common.Exceptions;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Infrastructure.Data.Models;
using TodoList.Service.Services;
using Xunit;

namespace TodoList.UnitTests.Service.Services
{
    public class TodoItemsServiceTests_UpdateTodoItem : TodoItemsServiceTests_Base
    {
        public TodoItemsServiceTests_UpdateTodoItem()
            : base()
        {
        }

        [Fact]
        public async Task UpdateTodoItem_Should_Succeed_And_Return_The_Correct_TodoItem_When_TodoItem_Exists_And_Another_Incomplete_Description_Does_Not_Exist()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItemId = _mockTodoItems.First().Id;
            var mockTodoItem = new TodoItem
            {
                Id = mockTodoItemId,
                Description = "Test 0",
                IsCompleted = true
            };

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.UpdateTodoItem(mockTodoItemId, mockTodoItem);
            }

            // Assert
            result.Should().BeEquivalentTo(mockTodoItem, "because we updated an existing TodoItem that contained a description that did not conflict with that of another incomplete TodoItem");
        }

        [Fact]
        public void UpdateTodoItem_Should_Fail_And_Throw_NotFoundException_When_TodoItem_Does_Not_Exist()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItemId = Guid.NewGuid();
            var mockTodoItem = new TodoItem
            {
                Id = mockTodoItemId,
                Description = "Test 0",
                IsCompleted = true
            };

            // Act
            var action = async () =>
            {
                using (var context = new TodoContext(_contextOptions))
                {
                    var todoItemsService = new TodoItemsService(context);
                    result = await todoItemsService.UpdateTodoItem(mockTodoItemId, mockTodoItem);
                }
            };

            // Assert
            action.Should().ThrowAsync<NotFoundException>("because we are trying to update a TodoItem that does not exist");
            result.Should().NotBeEquivalentTo(mockTodoItem, "because the UpdateTodoItem call failed");
        }

        [Fact]
        public void UpdateTodoItem_Should_Fail_And_Throw_DescriptionExistsException_When_Another_Incomplete_Description_Exists()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItemId = _mockTodoItems.First().Id;
            var mockTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 2",
                IsCompleted = false
            };

            // Act
            var action = async () =>
            {
                using (var context = new TodoContext(_contextOptions))
                {
                    context.TodoItems.Add(mockTodoItem);
                    context.SaveChanges();

                    var todoItemsService = new TodoItemsService(context);
                    result = await todoItemsService.UpdateTodoItem(mockTodoItemId, mockTodoItem);
                }
            };

            // Assert
            action.Should().ThrowAsync<DescriptionExistsException>("because we are trying to set the description of a TodoItem to that of one that conflicts with an existing incomplete TodoItem");
            result.Should().NotBeEquivalentTo(mockTodoItem, "because the UpdateTodoItem call failed");
        }
    }
}
