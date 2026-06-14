using FluentValidation;
using SimpleToDo.Application.Features.Todos.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace SimpleToDo.Application.Validations
{
    public class UpdateTodoCommandValidator : AbstractValidator<UpdateTodoCommand>
    {
        public UpdateTodoCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("Id is Reqired.");
            RuleFor(x => x.Status).NotEmpty().WithMessage("Status is Required");
        }
    }
}
