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
                HasHeaderRecord = true,
                IgnoreBlankLines = true,
                MissingFieldFound = null,
                HeaderValidated = null
            };

            using var csv = new CsvReader(reader, config);

            var produtos = new List<Produto>();

            while (await csv.ReadAsync())
            {
                try
                {
                    var dto = csv.GetRecord<ProdutoCriarDto>();

                    // Ignora linha vazia
                    if (string.IsNullOrWhiteSpace(dto.TituloProduto))
                        continue;

                    // Validação FluentValidation
                    var validation = await _validator.ValidateAsync(dto);

                    if (!validation.IsValid)
                        continue;

                    var produto = new Produto(
                        dto.TituloProduto,
                        dto.DescricaoProduto,
                        dto.PrecoProduto,
                        dto.EstoqueProduto,
                        dto.CodigoProduto
                    );

                    produtos.Add(produto);
                }
                catch
                {
                    // Ignora linha inválida
                    continue;
                }
            }

            if (produtos.Any())
            {
                await _repository.AdicionarListaAsync(produtos);
            }
        }
    }
}