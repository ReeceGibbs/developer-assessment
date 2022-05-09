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
            return await _context.TodoItems.ToListAsync();
        }

        public async Task<TodoItem> GetTodoItemById(Guid id)
        {
            return await _context.TodoItems.FindAsync(id);
        }

        public async Task<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            todoItem.Description = todoItem.Description?.Trim();
            await _context.AddAndSaveAsync(todoItem);

            return todoItem;
        }

        public async Task<TodoItem> UpdateTodoItem(TodoItem todoItem)
        {
            todoItem.Description = todoItem.Description?.Trim();
            await _context.UpdateAndSaveAsync(todoItem);

            return todoItem;
        }

        public async Task<Guid> DeleteTodoItem(Guid id)
        {
            await _context.DeleteAndSaveAsync(id);
            return id;
        }

        public async Task<bool> TodoItemIdExists(Guid id)
        {
            return await _context.TodoItems.AnyAsync(x => x.Id == id);
        }

        public async Task<bool> TodoItemDescriptionExists(string description)
        {
            return await _context.TodoItems
                   .AnyAsync(x => x.Description.Trim().ToLowerInvariant() == description.Trim().ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
