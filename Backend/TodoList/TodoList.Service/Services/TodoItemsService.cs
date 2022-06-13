using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Common.Exceptions;
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

        public Task<List<TodoItem>> GetTodoItemsList() => _context.TodoItems.ToListAsync();

        public ValueTask<TodoItem> GetTodoItemById(Guid id) => _context.TodoItems.FindAsync(id);

        public Task<bool> TodoItemDescriptionExists(string description, Guid? todoItemId = null) => 
            _context.TodoItems.AnyAsync(x => x.Description.Trim().ToLowerInvariant() == description.Trim().ToLowerInvariant() && !x.IsCompleted && x.Id != todoItemId);

        public async Task<TodoItem> CreateTodoItem(TodoItem newTodoItem)
        {
            if (await TodoItemDescriptionExists(newTodoItem.Description))
            {
                throw new DescriptionExistsException(newTodoItem.Description);
            }

            _context.Add(newTodoItem);
            await _context.SaveChangesAsync();

            return newTodoItem;
        }

        public async Task<TodoItem> UpdateTodoItem(Guid id, TodoItem updatedTodoItem)
        {
            var todoItem = await GetTodoItemById(id);

            if (todoItem == null)
            {
                throw new NotFoundException("TodoItem", id);
            }

            if (await TodoItemDescriptionExists(updatedTodoItem.Description, todoItem.Id))
            {
                throw new DescriptionExistsException(updatedTodoItem.Description);
            }

            todoItem.Description = updatedTodoItem.Description;
            todoItem.IsCompleted = updatedTodoItem.IsCompleted;

            _context.Update(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<Guid> DeleteTodoItem(Guid id)
        {
            var todoItem = await GetTodoItemById(id);

            if (todoItem == null)
            {
                throw new NotFoundException("TodoItem", id);
            }

            _context.Delete(todoItem);
            await _context.SaveChangesAsync();

            return id;
        }
    }
}
