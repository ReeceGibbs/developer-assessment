using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Infrastructure.Data.Contexts
{
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public DbSet<TodoItem> TodoItems { get; set; }

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public void Add(TodoItem todoItem)
        {
            TodoItems.Add(todoItem);
        }

        public void Update(TodoItem todoItem)
        {
            base.Entry(todoItem).State = EntityState.Modified;
        }

        public void Delete(TodoItem todoItem)
        {
            base.Entry(todoItem).State = EntityState.Deleted;
        }
    }
}
