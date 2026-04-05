using FluentValidation;
using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Validators
{
    public class RotaReordenarParadasValidator : AbstractValidator<RotaReordenarParadasDto>
    {
        public RotaReordenarParadasValidator()
        {
            RuleFor(x => x.AlteradoPeloUsuarioId)
                .NotEmpty().WithMessage("O usuario responsavel pela alteracao e obrigatorio.");

            RuleFor(x => x.ParadaIdsEmOrdem)
                .NotEmpty().WithMessage("A reordenacao deve informar a nova ordem das paradas.");

            RuleFor(x => x.ParadaIdsEmOrdem)
                .Must(ids => ids.Distinct().Count() == ids.Count)
                .WithMessage("A lista de paradas nao pode conter itens duplicados.");
        }
    }
}
