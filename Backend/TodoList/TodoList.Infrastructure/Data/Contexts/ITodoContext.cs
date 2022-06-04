using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Data.Contexts
{
    public interface ITodoContext : IDisposable
    {
        DbSet<TodoItem> TodoItems { get; set; }
        Task<int> SaveChangesAsync();
        void Add(TodoItem todoItem);
        void Update(TodoItem todoItem);
        void Delete(TodoItem todoItem);
    }
}
