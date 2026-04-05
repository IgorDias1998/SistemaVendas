using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class ClienteCreateValidator : AbstractValidator<ClienteCreateDto>
    {
        public ClienteCreateValidator()
        {
            RuleFor(c => c.Nome)
                .NotEmpty().WithMessage("O nome do cliente é obrigatório.")
                .MaximumLength(200).WithMessage("O nome do cliente deve ter no máximo 200 caracteres.");

            RuleFor(c => c.Telefone)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Matches(@"^\d{10,11}$").WithMessage("O telefone deve conter 10 ou 11 números.");

            RuleFor(c => c.Documento)
                .MaximumLength(20).WithMessage("O documento deve ter no máximo 20 caracteres.");

            RuleFor(c => c.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{8}$").WithMessage("O CEP deve conter 8 números.");

            RuleFor(c => c.Numero)
                .NotEmpty().WithMessage("O número do endereço é obrigatório.");
        }
    }
}
