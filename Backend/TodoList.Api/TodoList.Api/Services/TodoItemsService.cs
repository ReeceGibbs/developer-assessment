﻿using Microsoft.EntityFrameworkCore;
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

        public async Task<TodoItem> CreateTodoItem(TodoItem todoItem)
        {
            todoItem.Description = todoItem.Description?.Trim();

            _context.TodoItems.Add(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<TodoItem> UpdateTodoItem(TodoItem todoItem)
        {
            todoItem.Description = todoItem.Description?.Trim();

            _context.Update(todoItem);
            await _context.SaveChangesAsync();

            return todoItem;
        }

        public async Task<Guid> DeleteTodoItem(Guid id)
        {
            _context.Delete(id);
            await _context.SaveChangesAsync();

            return id;
        }

        public bool TodoItemIdExists(Guid id)
        {
            return _context.TodoItems.Any(x => x.Id == id);
        }

        public bool TodoItemDescriptionExists(string description)
        {
            return _context.TodoItems
                   .Any(x => x.Description.Trim().ToLowerInvariant() == description.Trim().ToLowerInvariant() && !x.IsCompleted);
        }
    }
}
