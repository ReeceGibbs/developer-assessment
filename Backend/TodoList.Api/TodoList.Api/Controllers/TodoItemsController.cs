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

        public TodoItemsController(ITodoItemsService todoItemsService)
        {
            _todoItemsService = todoItemsService;
        }

        public static string ToResponseMessage(HttpStatusCode httpStatusCode) => httpStatusCode switch
        {
            HttpStatusCode.NotFound => "Todo item not found",
            HttpStatusCode.Conflict => "Description already exists",
            _=> "Unmapped error code"
        };

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
                return ResponseExtensions<object>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
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
                if (!await _todoItemsService.TodoItemIdExists(id))
                    return ResponseExtensions<object>.FailureResponse(HttpStatusCode.NotFound, ToResponseMessage(HttpStatusCode.NotFound));

                var result = await _todoItemsService.GetTodoItemById(id);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<object>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
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
                if (!await _todoItemsService.TodoItemIdExists(todoItem.Id))
                    return ResponseExtensions<object>.FailureResponse(HttpStatusCode.NotFound, ToResponseMessage(HttpStatusCode.NotFound));

                if (!todoItem.IsCompleted && await _todoItemsService.TodoItemDescriptionExists(todoItem.Description))
                    return ResponseExtensions<object>.FailureResponse(HttpStatusCode.Conflict, ToResponseMessage(HttpStatusCode.Conflict));

                var result = await _todoItemsService.UpdateTodoItem(todoItem);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.OK, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<object>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
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
                if (await _todoItemsService.TodoItemDescriptionExists(todoItem.Description))
                    return ResponseExtensions<object>.FailureResponse(HttpStatusCode.Conflict, ToResponseMessage(HttpStatusCode.Conflict));

                var result = await _todoItemsService.CreateTodoItem(todoItem);
                return ResponseExtensions<TodoItem>.SuccessResponse(HttpStatusCode.Created, result);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<object>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [SwaggerResponse(204)]
        [SwaggerResponse(404, "Todo item not found", typeof(Response<TodoItem>), "application/json")]
        [SwaggerOperation(Summary = "TodoItems: Delete Item", Description = "Deletes an existing todo item by id")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            try
            {
                if (!await _todoItemsService.TodoItemIdExists(id))
                    return ResponseExtensions<object>.FailureResponse(HttpStatusCode.NotFound, ToResponseMessage(HttpStatusCode.NotFound));

                var result = await _todoItemsService.DeleteTodoItem(id);
                return ResponseExtensions<object>.SuccessResponse(HttpStatusCode.NoContent, null);
            }
            catch (Exception ex)
            {
                return ResponseExtensions<object>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }
    }
}
