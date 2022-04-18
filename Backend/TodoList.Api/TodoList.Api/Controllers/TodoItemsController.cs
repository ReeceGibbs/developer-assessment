using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
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

        // GET: api/TodoItems
        [HttpGet]
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

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
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

        // PUT: api/TodoItems/... 
        [HttpPut]
        public async Task<IActionResult> PutTodoItem(TodoItem todoItem)
        {
            try
            {
                if (!_todoItemsService.TodoItemIdExists(todoItem.Id))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.NotFound, "Todo item not found");

                if (_todoItemsService.TodoItemDescriptionExists(todoItem.Description))
                    return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.Conflict, "Description already exists");

                var result = await _todoItemsService.UpdateTodoItem(todoItem);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<TodoItem>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // POST: api/TodoItems 
        [HttpPost]
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

        // DELETE: api/TodoItems/{id}
        [HttpDelete("{id}")]
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
