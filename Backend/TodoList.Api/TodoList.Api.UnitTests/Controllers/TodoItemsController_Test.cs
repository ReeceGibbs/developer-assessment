using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Api.Controllers;
using TodoList.Api.Models;
using TodoList.Api.Services;
using Xunit;

namespace TodoList.Api.UnitTests.Controllers
{
    public class TodoItemsController_Test
    {
        private readonly Mock<ITodoItemsService> _todoItemsService;
        private readonly TodoItemsController _todoItemsController;

        public TodoItemsController_Test()
        {
            _todoItemsService = new Mock<ITodoItemsService>();
            _todoItemsController = new TodoItemsController(_todoItemsService.Object);
        }

        [Fact]
        public async Task GetTodoItems_success()
        {
            var mockTodoItems = new List<TodoItem>
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

            _todoItemsService.Setup(s => s.GetTodoItemsList()).Returns(Task.FromResult(mockTodoItems));

            var expectedResult = new Response<List<TodoItem>>
            {
                Success = true,
                Data = mockTodoItems
            };

            var result = (ObjectResult)await _todoItemsController.GetTodoItems();

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task GetTodoItems_server_error()
        {
            _todoItemsService.Setup(s => s.GetTodoItemsList()).Throws(new Exception("Database access error"));

            var expectedResult = new Response<List<TodoItem>>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("InternalServerError", "Database access error")
            };

            var result = (ObjectResult)await _todoItemsController.GetTodoItems();

            Assert.Equal(500, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task GetTodoItem_success()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(true);

            var mockTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 0",
                IsCompleted = false
            };

            _todoItemsService.Setup(s => s.GetTodoItemById(It.IsAny<Guid>())).Returns(Task.FromResult(mockTodoItem));

            var expectedResult = new Response<TodoItem>
            {
                Success = true,
                Data = mockTodoItem
            };

            var result = (ObjectResult)await _todoItemsController.GetTodoItem(Guid.NewGuid());

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task GetTodoItem_not_found()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(false);

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("NotFound", "Todo item not found")
            };

            var result = (ObjectResult)await _todoItemsController.GetTodoItem(Guid.NewGuid());

            Assert.Equal(404, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task GetTodoItem_server_error()
        {

        }
    }
}
