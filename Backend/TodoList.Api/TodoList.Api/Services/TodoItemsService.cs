using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoList.Api.Context;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public class TodoItemsService : ITodoItemsService
    {
        private readonly ITodoContext _context;

        public TodoItemsService(ITodoContext context)
        {
            _context = context;
        }

        public async Task<List<TodoItem>> GetTodoItemsList()
        {
            return await _context.TodoItems.Where(x => !x.IsCompleted).ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemById(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<bool> CreateTodoItem(TodoItem item)
        {
            try
            {
                _context.TodoItems.Add(item);
                await _context.SaveChangesAsync();

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.ToLowerInvariant() == description.ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
