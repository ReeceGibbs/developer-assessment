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
    public class TodoItemsServiceTests_GetTodoItemsList : TodoItemsServiceTests_Base
    {
        public TodoItemsServiceTests_GetTodoItemsList() :
            base()
        {
        }

        [Fact]
        public async Task GetTodoItemsList_Should_Return_Valid_List_Of_TodoItems_When_They_Exist()
        {
            // Arrange
            var result = new List<TodoItem>();

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.GetTodoItemsList();
            }

            // Assert
            result.Should().HaveCount(2, "because we started with 2 TodoItems in the DBSet");
            result.First().Should().BeEquivalentTo(_mockTodoItems.First());
        }

        [Fact]
        public async Task GetTodoItemsList_Should_Return_Empty_List_When_No_TodoItems_Exist()
        {
            // Arrange
            var result = new List<TodoItem>();

            // Act
            using (var context = new TodoContext(_contextOptions))
            {
                context.TodoItems.RemoveRange(context.TodoItems);
                context.SaveChanges();

                var todoItemsService = new TodoItemsService(context);
                result = await todoItemsService.GetTodoItemsList();
            }

            // Assert
            result.Should().BeEmpty("because there are currently no items in the TodoItems DBSet");
        }
    }
}
