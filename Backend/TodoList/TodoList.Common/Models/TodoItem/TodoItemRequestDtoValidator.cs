using FluentValidation;

namespace TodoList.Common.Models.TodoItem
{
    public class TodoItemRequestDtoValidator : AbstractValidator<TodoItemRequestDto>
    {
        public TodoItemRequestDtoValidator()
        {
            RuleFor(todoItem => todoItem.Description).NotEmpty();
        }
    }
}
