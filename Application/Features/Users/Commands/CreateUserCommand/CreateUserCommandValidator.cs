using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Users.Commands.CreateUserCommand
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(p => p.UserName)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(50).WithMessage("{PropertyName} no debe exceder de {MaxLength} carácteres.");

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(100).WithMessage("{PropertyName} no debe exceder de {MaxLength} carácteres.");

            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .EmailAddress().WithMessage("{PropertyName} no tiene un formato válido.")
                .MaximumLength(150).WithMessage("{PropertyName} no debe exceder de {MaxLength} carácteres.");
        }
    }
}
