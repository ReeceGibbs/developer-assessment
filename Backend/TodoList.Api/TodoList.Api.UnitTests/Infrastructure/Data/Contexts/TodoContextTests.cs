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

        public TodoContextTests()
        {
            var serviceProvider = new ServiceCollection()
               .AddEntityFrameworkInMemoryDatabase()
               .BuildServiceProvider();

            _contextOptions = new DbContextOptionsBuilder<TodoContext>()
                .UseInMemoryDatabase("TodoItemsDB")
                .UseInternalServiceProvider(serviceProvider).Options;
        }

        [Fact]
        public async Task SaveChangesAsync()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                Assert.Empty(context.TodoItems);

                context.Add(new TodoItem());
                await context.SaveChangesAsync();

                Assert.Single(context.TodoItems);
            }
        }

        [Fact]
        public async Task AddAsync()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                Assert.Empty(context.TodoItems);

                await context.AddAsync(new TodoItem());
                await context.SaveChangesAsync();

                Assert.Single(context.TodoItems);
            }
        }

        [Fact]
        public async Task Update()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                var mockGuid = new Guid("69802775-0810-498e-b7b2-818c16885f27");
                var mockTodoItem = new TodoItem()
                {
                    Id = mockGuid,
                    Description = "Test 0",
                    IsCompleted = false
                };

                context.Add(mockTodoItem);

                await context.SaveChangesAsync();

                Assert.Single(context.TodoItems);

                mockTodoItem.IsCompleted = true;

                context.Update(mockTodoItem);
                await context.SaveChangesAsync();

                Assert.True(context.TodoItems.Find(mockGuid).IsCompleted);
            }
        }

        [Fact]
        public async Task DeleteAndSaveAsync()
        {
            using (var context = new TodoContext(_contextOptions))
            {
                var mockGuid = new Guid("69802775-0810-498e-b7b2-818c16885f27");
                var mockTodoItem = new TodoItem()
                {
                    Id = mockGuid,
                    Description = "Test 0",
                    IsCompleted = false
                };

                context.Add(mockTodoItem);

                await context.SaveChangesAsync();

                Assert.Single(context.TodoItems);

                context.Delete(mockTodoItem);
                await context.SaveChangesAsync();

                Assert.Empty(context.TodoItems);
            }
        }
    }
}
