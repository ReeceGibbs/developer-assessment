using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TodoList.Common.Models.TodoItem
{
    public class TodoItemRequestDto
    {
        public string? Description { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
