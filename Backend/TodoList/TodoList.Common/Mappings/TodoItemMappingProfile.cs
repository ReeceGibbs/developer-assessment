using AutoMapper;
using TodoList.Common.Models.TodoItem;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Common.Mappings
{
    public class TodoItemMappingProfile : Profile
    {
        public TodoItemMappingProfile()
        {
            CreateMap<TodoItemRequestDto, TodoItem>();
            CreateMap<TodoItem, TodoItemRequestDto>();
        }
    }
}
