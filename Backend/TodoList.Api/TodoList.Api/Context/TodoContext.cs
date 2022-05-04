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

        public Task<int> SaveChangesAsync()
        {
            return base.SaveChangesAsync();
        }

        public async Task AddAndSaveAsync(TodoItem todoItem)
        {
            TodoItems.Add(todoItem);
            await SaveChangesAsync();
        }

        public async Task UpdateAndSaveAsync(TodoItem todoItem)
        {
            base.Entry(todoItem).State = EntityState.Modified;
            await SaveChangesAsync();
        }

        public async Task DeleteAndSaveAsync(Guid id)
        {
            var todoItem = await TodoItems.FindAsync(id);
            base.Entry(todoItem).State = EntityState.Deleted;
            await SaveChangesAsync();
        }
    }
}
