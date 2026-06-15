using FluentValidation;
using SimpleToDo.Application.Features.Queries.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Validations
{
    public class CreateQueryCommandValidator:AbstractValidator<CreateQueryCommand>
    {
        public CreateQueryCommandValidator()
        {
            RuleFor(x => x.Body)
                .NotEmpty().WithMessage("Query is equired.")
                .MaximumLength(500).WithMessage("Query cannot exceed 100 characters.");
            RuleFor(x => x.UserId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Invalid user id.");
            RuleFor(x=> x.TodoId)
                .NotEmpty()
                .GreaterThan(0).WithMessage("Invalid todo id.");
            RuleFor(t => t.FileName)
                .MaximumLength(100).WithMessage("FileName must be less than or equal 100.")
                .When(x => x.FileName != null);
            RuleFor(t => t.FilePath)
                .MaximumLength(100).WithMessage("FilePath must be less than or equal 100.")
                .When(x => x.FilePath != null);

        }
    }
}
