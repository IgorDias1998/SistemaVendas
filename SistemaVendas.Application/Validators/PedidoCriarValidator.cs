using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class PedidoCriarValidator : AbstractValidator<PedidoCriarDto>
    {
        public PedidoCriarValidator()
        {
            RuleFor(x => x.ClienteId)
                .NotEmpty().WithMessage("O cliente do pedido e obrigatorio.");

            RuleFor(x => x.Tipo)
                .IsInEnum().WithMessage("O tipo de pedido informado e invalido.");

            RuleFor(x => x.Itens)
                .NotEmpty().WithMessage("O pedido deve possuir ao menos um item.");

            RuleForEach(x => x.Itens)
                .SetValidator(new PedidoItemCriarValidator());
        }
    }
}
