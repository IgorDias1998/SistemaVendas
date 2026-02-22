using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class PessoaCreateValidator : AbstractValidator<PessoaCreateDto>
    {
        public PessoaCreateValidator() 
        {
            RuleFor(p => p.NomePessoa)
                .NotEmpty().WithMessage("O nome é obrigatório.")
                .MaximumLength(200).WithMessage("O nome deve ter no máximo 150 caracteres.");

            RuleFor(p => p.EmailPessoa)
                .NotEmpty().WithMessage("O e-mail é obrigatório.")
                .EmailAddress().WithMessage("E-mail inválido.");

            RuleFor(p => p.DataNascimento)
                .LessThan(DateTime.Today)
                .WithMessage("A data de nascimento deve ser anterior à data atual.");

            RuleFor(p => p.TelefonePessoa)
                .NotEmpty().WithMessage("O telefone é obrigatório.")
                .Matches(@"^\d{10,11}$")
                .WithMessage("O telefone deve conter 10 ou 11 números.");

            RuleFor(p => p.DocumentoPessoa)
                .NotEmpty().WithMessage("O documento é obrigatório.")
                .Matches(@"^\d{11}$")
                .WithMessage("O documento deve conter 11 números.");

            RuleFor(p => p.Cep)
                .NotEmpty().WithMessage("O CEP é obrigatório.")
                .Matches(@"^\d{8}$")
                .WithMessage("O CEP deve conter 8 números.");

            RuleFor(p => p.Numero)
                .NotEmpty().WithMessage("O número do endereço é obrigatório.");
        }
    }
}
