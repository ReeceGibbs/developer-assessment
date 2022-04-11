using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
    }
}
