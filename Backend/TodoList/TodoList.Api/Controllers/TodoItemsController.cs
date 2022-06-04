using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;
using TodoList.Common.Models.TodoItem;
using TodoList.Infrastructure.Data.Models;
using TodoList.Service.Services;

namespace TodoList.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly ITodoItemsService _todoItemsService;
        private readonly IMapper _mapper;

        public TodoItemsController(ITodoItemsService todoItemsService, IMapper mapper)
        {
            _todoItemsService = todoItemsService;
            _mapper = mapper;
        }

        // GET api/todoitems
        [HttpGet]
        public async Task<IActionResult> GetTodoItems()
        {
            var results = await _todoItemsService.GetTodoItemsList();
            return TodoItemResponseBuilder.SuccessResponse(HttpStatusCode.OK, results);
        }

        // PUT api/todoitems
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, [FromBody] TodoItemRequestDto todoItemRequestDto)
        {
            var result = await _todoItemsService.UpdateTodoItem(id, _mapper.Map<TodoItem>(todoItemRequestDto));
            return TodoItemResponseBuilder.SuccessResponse(HttpStatusCode.OK, result);
        }

        // POST api/todoitems
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([FromBody] TodoItemRequestDto todoItemRequestDto)
        {
            var result = await _todoItemsService.CreateTodoItem(_mapper.Map<TodoItem>(todoItemRequestDto));
            return TodoItemResponseBuilder.SuccessResponse(HttpStatusCode.Created, result);
        }

        // DELETE api/todoitems/{guid}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(Guid id)
        {
            var result = await _todoItemsService.DeleteTodoItem(id);
            return TodoItemResponseBuilder.SuccessResponse(HttpStatusCode.OK, result);
        }
    }
}
