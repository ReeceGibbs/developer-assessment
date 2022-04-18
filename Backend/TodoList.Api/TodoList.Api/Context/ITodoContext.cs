using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Context
{
    public interface ITodoContext : IDisposable
    {
        DbSet<TodoItem> TodoItems { get; set; }
        Task<int> SaveChangesAsync();
        void Update(TodoItem todoItem);
        void Delete(Guid id);
    }
}
