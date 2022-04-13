using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
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
            var results = await _todoItemsService.GetTodoItemsList();
            return Ok(results);
        }

        // GET: api/TodoItems/...
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTodoItem(Guid id)
        {
            var result = await _todoItemsService.GetTodoItemById(id);

            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // PUT: api/TodoItems/... 
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodoItem(Guid id, TodoItem todoItem)
        {
            //**NEEDS TO BE REFACTORED**

            //if (id != todoItem.Id)
            //{
            //    return BadRequest();
            //}

            ////_context.Entry(todoItem).State = EntityState.Modified;

            //try
            //{
            //    await _context.SaveChangesAsync();
            //}
            //catch (DbUpdateConcurrencyException)
            //{
            //    if (!TodoItemIdExists(id))
            //    {
            //        return NotFound();
            //    }
            //    else
            //    {
            //        throw;
            //    }
            //}

            return Ok("To Implement");
        }

        // POST: api/TodoItems 
        [HttpPost]
        public async Task<IActionResult> PostTodoItem([ModelBinder(BinderType = typeof(TodoItemModelBinder))] TodoItem todoItem)
        {
            if (_todoItemsService.TodoItemDescriptionExists(todoItem.Description))
                return BadRequest("Description already exists");

            var success = await _todoItemsService.CreateTodoItem(todoItem);

            return success ?
                CreatedAtAction(nameof(GetTodoItem), todoItem) :
                StatusCode(500);
        }
    }
}
