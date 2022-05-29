using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;
using TodoList.Api.ApiModels;
using TodoList.Infrastructure.Data.Models;
using TodoList.Service.Services;

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

        private static string ToResponseMessage(HttpStatusCode httpStatusCode) => httpStatusCode switch
        {
            HttpStatusCode.NotFound => "Todo item not found",
            HttpStatusCode.Conflict => "Description already exists",
            _=> "Unmapped error code"
        };

        // GET api/todoitems
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
                return ResponseExtensions<object>.FailureResponse(HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        // GET api/todoitems/{guid}
        [HttpGet("{id}")]
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

        // PUT api/todoitems
        [HttpPut]
        public async Task<IActionResult> PutTodoItem([FromBody] TodoItem todoItem)
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

        // POST api/todoitems
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromBody] TodoItem todoItem)
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

        // DELETE api/todoitems/{guid}
        [HttpDelete("{id}")]
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
