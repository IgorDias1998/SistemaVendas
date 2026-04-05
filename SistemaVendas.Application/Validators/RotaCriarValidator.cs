using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class RotaCriarValidator : AbstractValidator<RotaCriarDto>
    {
        public RotaCriarValidator()
        {
            RuleFor(x => x.CriadoPeloUsuarioId)
                .NotEmpty().WithMessage("O usuario criador da rota e obrigatorio.");

            RuleFor(x => x.DeliveryIds)
                .NotEmpty().WithMessage("A rota deve possuir ao menos uma delivery.");

            RuleFor(x => x.DeliveryIds)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("A lista de deliveries da rota nao pode conter itens duplicados.");
        }
    }
}
