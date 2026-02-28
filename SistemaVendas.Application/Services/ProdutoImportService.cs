using System.Formats.Asn1;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using FluentValidation;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class ProdutoImportService : IProdutoImportService
    {
        private readonly IProdutoRepository _repository;
        private readonly IValidator<ProdutoCriarDto> _validator;

        public ProdutoImportService(
            IProdutoRepository repository,
            IValidator<ProdutoCriarDto> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public async Task ImportarAsync(Stream stream)
        {
            using var reader = new StreamReader(stream);

            var config = new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                HasHeaderRecord = true
            };

            using var csv = new CsvReader(reader, config);

            var registros = csv.GetRecords<ProdutoCriarDto>().ToList();

            var produtos = new List<Produto>();

            foreach (var dto in registros)
            {
                var validation = await _validator.ValidateAsync(dto);

                if (!validation.IsValid)
                    throw new Exception("Erro de validação no CSV.");

                var produto = new Produto(
                    dto.TituloProduto,
                    dto.DescricaoProduto,
                    dto.PrecoProduto,
                    dto.EstoqueProduto,
                    dto.CodigoProduto
                );

                produtos.Add(produto);
            }

            await _repository.AdicionarListaAsync(produtos);
        }
    }
}