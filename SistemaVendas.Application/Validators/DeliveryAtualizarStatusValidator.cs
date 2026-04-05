using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class DeliveryAtualizarStatusValidator : AbstractValidator<DeliveryAtualizarStatusDto>
    {
        public DeliveryAtualizarStatusValidator()
        {
            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("O status de delivery informado e invalido.");
        }
    }
}
