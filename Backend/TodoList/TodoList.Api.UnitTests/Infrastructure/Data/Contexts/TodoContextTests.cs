using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Infrastructure.Data.Models;
using Xunit;

namespace TodoList.UnitTests.Infrastructure.Data.Contexts
{
    public class TodoContextTests
    {
        private readonly DbContextOptions<TodoContext> _contextOptions;
        private readonly List<TodoItem> _mockTodoItems;

        public TodoContextTests()
        {
            var serviceProvider = new ServiceCollection()
               .AddEntityFrameworkInMemoryDatabase()
               .BuildServiceProvider();

            _contextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TodoItemsDB")
                .UseInternalServiceProvider(serviceProvider).Options;

            using (var context = new TodoContext(_contextOptions))
            {
                _mockTodoItems = new List<TodoItem>()
                {
                    new TodoItem()
                    {
                        Id = Guid.NewGuid(),
                        Description = "Test 0",
                        IsCompleted = false
                    },
                    new TodoItem()
                    {
                        Id = Guid.NewGuid(),
                        Description = "Test 1",
                        IsCompleted = true
                    },
                };

                context.TodoItems.AddRange(_mockTodoItems);
                context.SaveChangesAsync();
            }
        }

        [Fact]
        public void Add_Should_Successfully_Add_A_TodoItem_To_The_DBSet()
        {
            // Arrange
            var countResult = 0;
            var todoItemResult = new TodoItem();
            var mockTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 2",
                IsCompleted = false
            };

            // Act

            using (var context = new TodoContext(_contextOptions))
            {
                context.Add(mockTodoItem);
                context.SaveChanges();

                countResult = context.TodoItems.Count();
                todoItemResult = context.TodoItems.Last();
            }

            // Assert
            countResult.Should().Be(_mockTodoItems.Count + 1, "because we added exactly one TodoItem to our DBSet of TodoItems");
            todoItemResult.Should().BeEquivalentTo(mockTodoItem, "because the item we just added should match the item in the DBSet");
        }

        [Fact]
        public void Update_Should_Successfully_Update_An_Existing_TodoItem_In_The_DBSet()
        {
            // Arrange
            var todoItemResult = new TodoItem();
            var updatedTodoItemDescription = "Updated";

            var mockTodoItem = _mockTodoItems.First();
            mockTodoItem.Description = updatedTodoItemDescription;

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                context.Update(mockTodoItem);
                context.SaveChanges();

                todoItemResult = context.TodoItems.Find(mockTodoItem.Id);
            }

            // Assert
            todoItemResult.Description.Should().Be(updatedTodoItemDescription, "because we updated an existing TodoItem with a valid description");
        }

        [Fact]
        public void Delete_Should_Successfully_Remove_An_Existing_TodoItem_In_The_DBSet()
        {
            // Arrange
            var countResult = 0;
            var todoItemResult = new TodoItem();
            var mockTodoItem = _mockTodoItems.First();

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                context.Delete(mockTodoItem);
                context.SaveChanges();

                countResult = context.TodoItems.Count();
                todoItemResult = context.TodoItems.Find(mockTodoItem.Id);
            }

            // Assert
            countResult.Should().Be(_mockTodoItems.Count - 1, "because we removed exactly one existing TodoItem from the DBSet");
            todoItemResult.Should().BeNull("because the TodoItem we deleted should no longer exist in the DBSet");
        }
    }
}
