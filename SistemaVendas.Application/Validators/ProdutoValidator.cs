using FluentValidation;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Application.Validators
{
    public class ProdutoValidator : AbstractValidator<ProdutoCriarDto>
    {
        public ProdutoValidator() 
        {
            RuleFor(produto => produto.TituloProduto).NotEmpty().WithMessage("O título é obrigatório.")
                .MaximumLength(150).WithMessage("O título não pode ser maior que 150 caracteres.");

            RuleFor(p => p.DescricaoProduto)
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres.");

            RuleFor(p => p.PrecoProduto)
                .GreaterThanOrEqualTo(0)
                .WithMessage("O preço não pode ser negativo.");

            RuleFor(p => p.CodigoProduto)
                .NotEmpty().WithMessage("O código do produto é obrigatório.")
                .MaximumLength(20).WithMessage("O código deve ter no máximo 20 caracteres.");

            RuleFor(p => p.CodigoProduto)
                .Matches("^[a-zA-Z0-9]+$")
                .WithMessage("O código deve conter apenas letras e números.");
        }
    }
}
