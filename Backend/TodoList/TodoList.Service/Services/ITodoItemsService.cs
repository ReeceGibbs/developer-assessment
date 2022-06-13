using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Service.Services
{
    public interface ITodoItemsService
    {
        ValueTask<TodoItem> GetTodoItemById(Guid id);
        Task<List<TodoItem>> GetTodoItemsList();
        Task<bool> TodoItemDescriptionExists(string description, Guid? todoItemId);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItem(Guid id, TodoItem todoItem);
        Task<Guid> DeleteTodoItem(Guid id);
    }
}
