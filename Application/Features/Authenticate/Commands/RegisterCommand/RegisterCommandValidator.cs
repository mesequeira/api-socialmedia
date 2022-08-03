using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Authenticate.Commands.RegisterCommand
{
    internal class RegisterCommandValidator : AbstractValidator<RegisterCommand>
    {
        public RegisterCommandValidator()
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

            RuleFor(p => p.Password)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(15).WithMessage("{PropertyName} no debe exceder de {MaxLength} carácteres.");

            RuleFor(p => p.ConfirmPassword)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(15).WithMessage("{PropertyName} no debe exceder de {MaxLength} carácteres.")
                .Equal(p => p.Password).WithMessage("{PropertyName} debe ser igual a password.");
        }
    }
}
