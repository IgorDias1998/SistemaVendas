using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class UsuarioCriarValidator : AbstractValidator<UsuarioCriarDto>
    {
        public UsuarioCriarValidator()
        {
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O nome do usuario e obrigatorio.")
                .MaximumLength(200).WithMessage("O nome do usuario deve ter no maximo 200 caracteres.");

            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O e-mail e obrigatorio.")
                .EmailAddress().WithMessage("O e-mail informado e invalido.")
                .MaximumLength(200).WithMessage("O e-mail deve ter no maximo 200 caracteres.");

            RuleFor(x => x.Senha)
                .NotEmpty().WithMessage("A senha e obrigatoria.")
                .MinimumLength(6).WithMessage("A senha deve ter ao menos 6 caracteres.");

            RuleFor(x => x.Role)
                .IsInEnum().WithMessage("O papel do usuario informado e invalido.");
        }
    }
}
