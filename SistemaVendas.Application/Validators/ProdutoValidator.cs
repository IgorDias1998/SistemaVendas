using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class ProdutoValidator : AbstractValidator<ProdutoCriarDto>
    {
        public ProdutoValidator() 
        {
            RuleFor(produto => produto.TituloProduto).NotEmpty().WithMessage("O título não pode ser vazio.");
        }
    }
}
