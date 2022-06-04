using FluentValidation;
using TodoList.Api.ApiModels;

namespace TodoList.Api.Validators
{
    public class TodoItemRequestDtoValidator: AbstractValidator<TodoItemRequestDto>
    {
        public TodoItemRequestDtoValidator()
        {
            RuleFor(todoItem => todoItem.Description).NotEmpty();
        }
    }
}
