using FluentValidation;
using SimpleToDo.Application.Features.Todos.Commands;

namespace SimpleToDo.Application.Validations
{
    public class CreateTodoCommandValidator : AbstractValidator<CreateTodoCommand>
    {
        public CreateTodoCommandValidator()
        {
            RuleFor(t => t.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");
            RuleFor(t => t.Description)
                .NotEmpty().WithMessage("Description is required.")
                .MaximumLength(100).WithMessage("Description cannot exceed 100 characters.");
            RuleFor(t => t.FileName)
                .MaximumLength(100).WithMessage("FileName must be less than or equal 100.")
                .When(x => x.FileName != null);
            RuleFor(t => t.FilePath)
                .MaximumLength(100).WithMessage("FilePath must be less than or equal 100.")
                .When(x => x.FilePath != null);
            RuleFor(t => t.ProjectId)
                .GreaterThan(0);
        }
    }
}
