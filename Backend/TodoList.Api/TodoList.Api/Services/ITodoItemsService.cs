using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Services
{
    public interface ITodoItemsService
    {
        Task<List<TodoItem>> GetTodoItemsList();
        Task<TodoItem> GetTodoItemById(Guid id);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItem(TodoItem todoItem);
        Task<Guid> DeleteTodoItem(Guid id);
        bool TodoItemIdExists(Guid id);
        bool TodoItemDescriptionExists(string description);
    }
}
