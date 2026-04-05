using Microsoft.EntityFrameworkCore;
using Moq;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;
using SistemaVendas.Infrastructure.Repositories;

namespace SistemaVendas.Tests.PessoaTests
{
    public class ClienteTests
    {
        [Fact(DisplayName = "Deve salvar um cliente com endereço resolvido pelo CEP.")]
        public async Task DeveSalvarClienteNoBanco()
        {
            var context = DbContextFactory.Create();
            var repository = new ClienteRepository(context);

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

            var service = new ClienteService(repository, cepMock.Object);

            var dto = new ClienteCreateDto
            {
                Nome = "Maria",
                Telefone = "11988888888",
                Documento = "10987654321",
                Cep = "01001000",
                Numero = "200",
                Complemento = ""
            };

            await service.CriarClienteAsync(dto);

            var salvo = await context.Clientes.Include(c => c.Enderecos).FirstOrDefaultAsync(c => c.Documento == dto.Documento);
            Assert.NotNull(salvo);
            Assert.NotNull(salvo.Enderecos.FirstOrDefault());
            Assert.Equal("São Paulo", salvo.Enderecos.First().Cidade);
        }
    }
}
