using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class PedidoItemCriarValidator : AbstractValidator<PedidoItemCriarDto>
    {
        public PedidoItemCriarValidator()
        {
            RuleFor(x => x.ProdutoId)
                .NotEmpty().WithMessage("O produto do item e obrigatorio.");

            RuleFor(x => x.Quantidade)
                .GreaterThan(0).WithMessage("A quantidade do item deve ser maior que zero.");
        }
    }
}
