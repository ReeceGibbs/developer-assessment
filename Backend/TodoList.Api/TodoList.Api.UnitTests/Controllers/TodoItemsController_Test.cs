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
                        Id = Guid.NewGuid(),
                        Description = "Test 0",
                        IsCompleted = false
                    },
                    new TodoItem
                    {
                        Id = Guid.NewGuid(),
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
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));

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
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(false));

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
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.GetTodoItemById(It.IsAny<Guid>())).Throws(new Exception("Database access error"));

            var expectedResult = new Response<List<TodoItem>>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("InternalServerError", "Database access error")
            };

            var result = (ObjectResult)await (_todoItemsController.GetTodoItem(Guid.NewGuid()));

            Assert.Equal(500, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PutTodoItem_success()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.TodoItemDescriptionExists(It.IsAny<string>())).Returns(Task.FromResult(false));

            var mockTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 0",
                IsCompleted = false,
            };

            _todoItemsService.Setup(s => s.UpdateTodoItem(It.IsAny<TodoItem>())).Returns(Task.FromResult(mockTodoItem));

            var expectedResult = new Response<TodoItem>
            {
                Success = true,
                Data = mockTodoItem
            };

            var result = (ObjectResult)await _todoItemsController.PutTodoItem(mockTodoItem);

            Assert.Equal(200, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PutTodoItem_not_found()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("NotFound", "Todo item not found")
            };

            var result = (ObjectResult)await _todoItemsController.PutTodoItem(new TodoItem());

            Assert.Equal(404, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PutTodoItem_conflict()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.TodoItemDescriptionExists(It.IsAny<string>())).Returns(Task.FromResult(true));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("Conflict", "Description already exists")
            };

            var result = (ObjectResult)await _todoItemsController.PutTodoItem(new TodoItem());

            Assert.Equal(409, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PutTodoItem_server_error()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.TodoItemDescriptionExists(It.IsAny<string>())).Returns(Task.FromResult(false));

            _todoItemsService.Setup(s => s.UpdateTodoItem(It.IsAny<TodoItem>())).Throws(new Exception("Database access error"));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("InternalServerError", "Database access error")
            };

            var result = (ObjectResult)await _todoItemsController.PutTodoItem(new TodoItem());

            Assert.Equal(500, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PostTodoItem_success()
        {
            _todoItemsService.Setup(s => s.TodoItemDescriptionExists(It.IsAny<string>())).Returns(Task.FromResult(false));

            var mockTodoItem = new TodoItem
            {
                Id = Guid.NewGuid(),
                Description = "Test 0",
                IsCompleted = false,
            };

            _todoItemsService.Setup(s => s.CreateTodoItem(It.IsAny<TodoItem>())).Returns(Task.FromResult(mockTodoItem));

            var expectedResult = new Response<TodoItem>
            {
                Success = true,
                Data = mockTodoItem
            };

            var result = (ObjectResult)await _todoItemsController.PostTodoItem(mockTodoItem);

            Assert.Equal(201, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PostTodoItem_conflict()
        {
            _todoItemsService.Setup(s => s.TodoItemDescriptionExists(It.IsAny<string>())).Returns(Task.FromResult(true));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("Conflict", "Description already exists")
            };

            var result = (ObjectResult)await _todoItemsController.PostTodoItem(new TodoItem());

            Assert.Equal(409, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task PostTodoItem_server_error()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.TodoItemDescriptionExists(It.IsAny<string>())).Returns(Task.FromResult(false));

            _todoItemsService.Setup(s => s.CreateTodoItem(It.IsAny<TodoItem>())).Throws(new Exception("Database access error"));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("InternalServerError", "Database access error")
            };

            var result = (ObjectResult)await _todoItemsController.PostTodoItem(new TodoItem());

            Assert.Equal(500, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task DeleteTodoItem_success()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.DeleteTodoItem(It.IsAny<Guid>())).Returns(Task.FromResult(Guid.NewGuid()));

            var expectedResult = new Response<TodoItem>
            {
                Success = true,
                Data = null
            };

            var result = (ObjectResult)await _todoItemsController.DeleteTodoItem(Guid.NewGuid());

            Assert.Equal(204, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task DeleteTodoItem_not_found()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(false));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("NotFound", "Todo item not found")
            };

            var result = (ObjectResult)await _todoItemsController.DeleteTodoItem(Guid.NewGuid());

            Assert.Equal(404, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }

        [Fact]
        public async Task DeleteTodoItem_server_error()
        {
            _todoItemsService.Setup(s => s.TodoItemIdExists(It.IsAny<Guid>())).Returns(Task.FromResult(true));
            _todoItemsService.Setup(s => s.DeleteTodoItem(It.IsAny<Guid>())).Throws(new Exception("Database access error"));

            var expectedResult = new Response<TodoItem>
            {
                Success = false,
                Error = new KeyValuePair<string, string>("InternalServerError", "Database access error")
            };

            var result = (ObjectResult)await _todoItemsController.DeleteTodoItem(Guid.NewGuid());

            Assert.Equal(500, result.StatusCode);
            Assert.Equal(JsonConvert.SerializeObject(expectedResult),
                JsonConvert.SerializeObject(result.Value));
        }
    }
}
