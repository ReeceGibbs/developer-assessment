using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace TodoList.Api.Context
{
    public interface ITodoContext : IDisposable
    {
        DbSet<TodoItem> TodoItems { get; set; }
        Task<int> SaveChangesAsync();
    }
}
