using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using TodoList.Api.ModelBinders;
using TodoList.Api.Models;
using TodoList.Api.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsService _todoItemsService;
        private readonly ILogger<TodoItemsController> _logger;

        public TodoItemsController(ITodoItemsService todoItemsService, ILogger<TodoItemsController> logger)
        {
            _todoItemsService = todoItemsService;
            _logger = logger;
        }

        [HttpGet]
        [SwaggerResponse(200, null, typeof(Response<TodoItem>), "application/json")]
        [SwaggerOperation(Summary = "TodoItems: Get List", Description = "Returns a list of all incomplete todo items")]
        public async Task<IActionResult> GetTodoItems()
        {
            try
            {
                var results = await _todoItemsService.GetTodoItemsList();
                return ResponseExtensions<List<TodoItem>>.SuccessResponse(HttpStatusCode.OK, results);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        [SwaggerResponse(200, null, typeof(Response<TodoItem>), "application/json")]
        [SwaggerResponse(404, "Todo item not found", typeof(Response<TodoItem>), "application/json")]
        [SwaggerOperation(Summary = "TodoItems: Get Item", Description = "Returns a todo item by id")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            try
            {
                if (!_todoItemsService.TodoItemIdExists(id))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.NotFound, "Todo item not found");

                var result = await _todoItemsService.GetTodoItemById(id);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut(Name = "TodoItemPut")]
        [SwaggerResponse(200, null, typeof(Response<TodoItem>), "application/json")]
        [SwaggerResponse(404, "Todo item not found", typeof(Response<TodoItem>), "application/json")]
        [SwaggerResponse(409, "Description already exists", typeof(Response<TodoItem>), "application/json")]
        [SwaggerOperation(Summary = "TodoItems: Update Item", Description = "Updates an existing todo item")]
        public async Task<IActionResult> PutTodoItem([ModelBinder(BinderType = typeof(TodoItemModelBinder))] TodoItem todoItem)
        {
            try
            {
                if (!_todoItemsService.TodoItemIdExists(todoItem.Id))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.NotFound, "Todo item not found");

                if (!todoItem.IsCompleted && _todoItemsService.TodoItemDescriptionExists(todoItem.Description))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.Conflict, "Description already exists");

                var result = await _todoItemsService.UpdateTodoItem(todoItem);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPost(Name = "TodoItemPost")]
        [SwaggerResponse(201, null, typeof(Response<TodoItem>), "application/json")]
        [SwaggerResponse(409, "Description already exists", typeof(Response<TodoItem>), "application/json")]
        [SwaggerOperation(Summary = "TodoItems: Create Item", Description = "Creates a new todo item")]
        public async Task<IActionResult> PostTodoItem([ModelBinder(BinderType = typeof(TodoItemModelBinder))] TodoItem todoItem)
        {
            try
            {
                if (_todoItemsService.TodoItemDescriptionExists(todoItem.Description))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.Conflict, "Description already exists");

                var result = await _todoItemsService.CreateTodoItem(todoItem);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(409, "Description already exists", typeof(Response<TodoItem>), "application/json")]
        [SwaggerOperation(Summary = "TodoItems: Delete Item", Description = "Deletes an existing todo item by id")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            try
            {
                if (!_todoItemsService.TodoItemIdExists(id))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.NotFound, "Todo item not found");

                var result = await _todoItemsService.DeleteTodoItem(id);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.NoContent, null);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
