using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using TodoList.Api.Context;
using TodoList.Api.Models;
using TodoList.Api.Services;
using Microsoft.EntityFrameworkCore;
using System.Data.Entity.Infrastructure;
using Microsoft.Extensions.DependencyInjection;

namespace TodoList.Api.UnitTests.Services
{
    public class TodoItemsService_Test
    {
        private readonly DbContextOptions<TodoContext> _contextOptions;
        private readonly List<TodoItem> _mockTodoItems;

        public TodoItemsService_Test()
        {
            var serviceProvider = new ServiceCollection()
                .AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            _contextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TodoItemsDB")
                .UseInternalServiceProvider(serviceProvider).Options;

            using (var context = new TodoContext(_contextOptions))
            {
                _mockTodoItems = new List<TodoItem>
                {
                    new TodoItem
                    {
                        Id = new Guid("fede7bcd-20a0-4c7a-8077-219dbd0118f4"),
                        Description = "Test 0",
                        IsCompleted = false
                    },
                    new TodoItem
                    {
                        Id = new Guid("de30013b-96c0-477a-b704-2d4977637d14"),
                        Description = "Test 1",
                        IsCompleted = true
                    },
                };

                context.TodoItems.AddRange(_mockTodoItems);
                context.SaveChangesAsync();
            }
        }

        [Fact]
        public async Task GetTodoItemsList()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsService todoItemsService = new TodoItemsService(context);
                var todoItems = await todoItemsService.GetTodoItemsList();

                Assert.Single(todoItems);
            }
        }

        [Fact]
        public async Task GetTodoItemById()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsService todoItemsService = new TodoItemsService(context);
                var todoItem = await todoItemsService.GetTodoItemById(_mockTodoItems[0].Id);

                Assert.Equal(_mockTodoItems[0].Id, todoItem.Id);
            }
        }

        [Fact]
        public async Task CreateTodoItem()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                Assert.Equal(2, context.TodoItems.Count());

                TodoItemsService todoItemsService = new TodoItemsService(context);
                var todoItem = await todoItemsService.CreateTodoItem(new TodoItem());

                Assert.Equal(3, context.TodoItems.Count());
            }
        }

        [Fact]
        public async Task UpdateTodoItem()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                Assert.False(context.TodoItems.AsNoTracking().FirstOrDefault(x => x.Id == _mockTodoItems[0].Id).IsCompleted);

                TodoItemsService todoItemsService = new TodoItemsService(context);
                var todoItem = await todoItemsService.UpdateTodoItem(
                    new TodoItem
                    {
                        Id = _mockTodoItems[0].Id,
                        Description = "Test 0",
                        IsCompleted = true
                    });

                Assert.True(context.TodoItems.AsNoTracking().FirstOrDefault(x => x.Id == _mockTodoItems[0].Id).IsCompleted);
            }
        }

        [Fact]
        public async Task DeleteTodoItem()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                Assert.NotNull(context.TodoItems.AsNoTracking().FirstOrDefault(x => x.Id == _mockTodoItems[0].Id));

                TodoItemsService todoItemsService = new TodoItemsService(context);
                var todoItem = await todoItemsService.DeleteTodoItem(_mockTodoItems[0].Id);

                Assert.Null(context.TodoItems.AsNoTracking().FirstOrDefault(x => x.Id == _mockTodoItems[0].Id));
            }
        }

        [Fact]
        public void TodoItemIdExists()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsService todoItemsService = new TodoItemsService(context);
                Assert.True(todoItemsService.TodoItemIdExists(_mockTodoItems[0].Id));
            }
        }

        [Fact]
        public void TodoItemDescriptionExists()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                TodoItemsService todoItemsService = new TodoItemsService(context);
                Assert.True(todoItemsService.TodoItemDescriptionExists(_mockTodoItems[0].Description));
            }
        }
    }
}
