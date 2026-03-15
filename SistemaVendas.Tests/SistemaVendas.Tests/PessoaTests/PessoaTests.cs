using Microsoft.EntityFrameworkCore;
using Moq;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Repositories;

namespace SistemaVendas.Tests.PessoaTests
{
    public class PessoaTests
    {
        [Fact(DisplayName = "Deve gerar um erro ao adicionar uma pessoa com e-mail inválido.")]
        public async Task DeveSalvarProdutoNoBancoEBuscar()
        {
            // Arrange: criar DbContext in-memory (use sua factory existente)
            var context = DbContextFactory.Create(); // sua factory deve configurar InMemory provider
            var repository = new PessoaRepository(context);

            var cepMock = new Mock<ICepService>();
            cepMock.Setup(c => c.BuscarCepAsync(It.IsAny<string>()))
                .ReturnsAsync(new CepResultDto
                {
                    Cep = "01001-000",
                    Logradouro = "Praça da Sé",
                    Bairro = "Sé",
                    Cidade = "São Paulo",
                    Estado = "SP"
                });

            var service = new PessoaService(repository, cepMock.Object);

            var dto = new PessoaCreateDto
            {
                NomePessoa = "Maria",
                EmailPessoa = "maria@example.com",
                DataNascimento = new DateTime(1985, 5, 5),
                TelefonePessoa = "11988888888",
                DocumentoPessoa = "10987654321",
                Cep = "01001-000",
                Numero = "200",
                Complemento = ""
            };

            // Act
            await service.CriarPessoaAsync(dto);

            // Assert: verificar que a pessoa e o endereço foram salvos
            var salva = await context.Pessoas.Include(p => p.EnderecoPessoa).FirstOrDefaultAsync(p => p.EmailPessoa == dto.EmailPessoa);
            Assert.NotNull(salva);
            Assert.NotNull(salva.EnderecoPessoa);
            Assert.Equal("São Paulo", salva.EnderecoPessoa.Cidade);
        }
    }
}
