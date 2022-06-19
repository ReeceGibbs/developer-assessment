using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.UnitTests.Service.Services
{
    public class TodoItemsServiceTests_Base
    {
        protected readonly DbContextOptions<TodoContext> _contextOptions;
        protected readonly List<TodoItem> _mockTodoItems;

        public TodoItemsServiceTests_Base()
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
    }
}
