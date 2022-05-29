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
        Task<List<TodoItem>> GetTodoItemsList();
        Task<TodoItem> GetTodoItemById(Guid id);
        Task<TodoItem> CreateTodoItem(TodoItem todoItem);
        Task<TodoItem> UpdateTodoItem(TodoItem todoItem);
        Task<Guid> DeleteTodoItem(Guid id);
        Task<bool> TodoItemIdExists(Guid id);
        Task<bool> TodoItemDescriptionExists(string description);
    }
}
