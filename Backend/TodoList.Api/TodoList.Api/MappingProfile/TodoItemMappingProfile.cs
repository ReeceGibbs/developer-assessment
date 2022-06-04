using AutoMapper;
using TodoList.Api.ApiModels;
using TodoList.Infrastructure.Data.Models;

namespace TodoList.Api.MappingProfile
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
