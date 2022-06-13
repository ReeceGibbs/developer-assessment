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
    public class TodoItemsServiceTests_CreateTodoItem : TodoItemsServiceTests_Base
    {
        public TodoItemsServiceTests_CreateTodoItem()
            : base()
        {
        }

        [Fact]
        public async Task CreateTodoItem_Should_Succeed_And_Return_The_Correct_TodoItem_When_Incomplete_Description_Does_Not_Exist()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItem = new TodoItem
            {
                Description = "Test 2",
                IsCompleted = false
            };

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.CreateTodoItem(mockTodoItem);
            }

            // Assert
            result.Should().BeEquivalentTo(mockTodoItem, "because we have created a TodoItem that does not contain a description that conflicts with the description of an existing, incomplete TodoItem");
        }

        [Fact]
        public async Task CreateTodoItem_Should_Succeed_And_Return_The_Correct_TodoItem_When_Complete_Description_Exists()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItem = new TodoItem
            {
                Description = "Test 1",
                IsCompleted = false
            };

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.CreateTodoItem(mockTodoItem);
            }

            // Assert
            result.Should().BeEquivalentTo(mockTodoItem, "because we have created a TodoItem that contains a description that only conflicts with that of a previously completed TodoItem");
        }

        [Fact]
        public void CreateTodoItem_Should_Fail_And_Throw_DescriptionExistsException_When_Incomplete_Description_Exists()
        {
            // Arrange
            var result = new TodoItem();
            var mockTodoItem = new TodoItem
            {
                Description = "Test 0",
                IsCompleted = false
            };

            // Act
            Func<Task> action = async () =>
            {
                using (var context = new TodoContext(_contextOptions))
                {
                    var todoItemsService = new TodoItemsService(context);
                    result = await todoItemsService.CreateTodoItem(mockTodoItem);
                }
            };

            // Assert
            action.Should().ThrowAsync<DescriptionExistsException>("because we are trying to create a TodoItem that contains a description that conflicts with that of an incomplete TodoItem");
            result.Should().NotBeEquivalentTo(mockTodoItem, "because the CreateTodoItem call failed");
        }
    }
}
