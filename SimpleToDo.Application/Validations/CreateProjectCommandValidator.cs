using FluentValidation;
using SimpleToDo.Application.Features.Projects.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Validations
{
    public class CreateProjectCommandValidator:AbstractValidator<CreateProjectCommand>
    {
        public CreateProjectCommandValidator() 
        {
            RuleFor(c => c.Name)
                .NotEmpty().WithMessage("Name is required.")
                .MaximumLength(100).WithMessage("Name cannot exceed 100 characters.");
        }
    }
}
