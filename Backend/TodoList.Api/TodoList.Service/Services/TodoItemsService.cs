using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Contexts;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Service.Services
{
    public class TodoItemsService : ITodoItemsService
    {
        private readonly ITodoContext _context;

        public TodoItemsService(ITodoContext context)
        {
            _context = context;
        }

        public ValueTask<TodoItem> GetTodoItemById(Guid id) => _context.TodoItems.FindAsync(id);

        public Task<List<TodoItem>> GetTodoItemsList() => _context.TodoItems.ToListAsync();

        public Task<bool> TodoItemDescriptionExists(string description) => _context.TodoItems.AnyAsync(x => x.Description.Trim().ToLowerInvariant() == description.Trim().ToLowerInvariant() && !x.IsCompleted);

        public async Task<TodoItem> CreateTodoItem(TodoItem newTodoItem)
        {
            _context.Add(newTodoItem);
            await _context.SaveChangesAsync();

            return newTodoItem;
        }

        public async Task<TodoItem> UpdateTodoItem(Guid id, TodoItem updatedTodoItem)
        {
            var todoItem = await GetTodoItemById(id);

            todoItem.Description = updatedTodoItem.Description;
            todoItem.IsCompleted = updatedTodoItem.IsCompleted;

            _context.Update(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<Guid> DeleteTodoItem(Guid id)
        {
            var todoItem = await GetTodoItemById(id);

            _context.Delete(todoItem);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
