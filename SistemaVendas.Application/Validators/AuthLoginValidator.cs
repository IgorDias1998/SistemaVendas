using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class AuthLoginValidator : AbstractValidator<AuthLoginDto>
    {
        public AuthLoginValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail e obrigatorio.")
                .EmailAddress().WithMessage("O e-mail informado e invalido.")
                .MaximumLength(200).WithMessage("O e-mail deve ter no maximo 200 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha e obrigatoria.");
        }
    }
}
