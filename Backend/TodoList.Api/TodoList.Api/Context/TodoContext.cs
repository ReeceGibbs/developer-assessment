using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using TodoList.Api.Models;

namespace TodoList.Api.Context
{
    public class TodoContext : DbContext, ITodoContext
    {
        public TodoContext(DbContextOptions<TodoContext> options) : base(options) { }
        public DbSet<TodoItem> TodoItems { get; set; }

        Task<int> ITodoContext.SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        void ITodoContext.Update(TodoItem todoItem)
        {
            base.Entry(todoItem).State = EntityState.Modified;
        }

        async void ITodoContext.Delete(Guid id)
        {
            var todoItem = await TodoItems.FindAsync(id);
            base.Entry(todoItem).State = EntityState.Deleted;
        }
    }
}
