using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Repositories;

namespace SistemaVendas.Tests.PedidoTests
{
    public class PedidoServiceTests
    {
        [Fact(DisplayName = "Confirmar pedido delivery deve gerar delivery pendente.")]
        public async Task ConfirmarPedidoAsync_Delivery_DeveGerarDelivery()
        {
            using var context = DbContextFactory.Create();

            var cliente = new Cliente("Maria", "11999999999", "12345678900");
            cliente.DefinirEnderecos(new[]
            {
                new ClienteEndereco
                {
                    Cep = "01001-000",
                    Logradouro = "Praca da Se",
                    Bairro = "Se",
                    Cidade = "Sao Paulo",
                    Estado = "SP",
                    Numero = "100"
                }
            });

            var produto = new Produto("Agua", "Garrafa", 10.50m, 20, "AGUA1");

            await context.Clientes.AddAsync(cliente);
            await context.Produtos.AddAsync(produto);
            await context.SaveChangesAsync();

            var service = new PedidoService(
                new PedidoRepository(context),
                new ClienteRepository(context),
                new ProdutoRepository(context),
                new DeliveryRepository(context),
                new PedidoCriarValidator());

            var pedido = await service.CriarRascunhoAsync(new PedidoCriarDto
            {
                ClienteId = cliente.ClienteId,
                Tipo = TipoPedido.Delivery,
                Itens =
                {
                    new PedidoItemCriarDto
                    {
                        ProdutoId = produto.ProdutoId,
                        Quantidade = 2
                    }
                }
            }, Guid.NewGuid());

            var pedidoConfirmado = await service.ConfirmarPedidoAsync(pedido.PedidoId);

            Assert.Equal(StatusPedido.Confirmado, pedidoConfirmado.Status);
            Assert.NotNull(pedidoConfirmado.DeliveryId);
            Assert.Single(context.Deliveries);
            Assert.Equal(StatusDelivery.Pendente, context.Deliveries.First().Status);
        }

        [Fact(DisplayName = "Confirmar pedido retirada nao deve gerar delivery.")]
        public async Task ConfirmarPedidoAsync_Retirada_NaoDeveGerarDelivery()
        {
            using var context = DbContextFactory.Create();

            var cliente = new Cliente("Carlos", "11988888888", "98765432100");
            var produto = new Produto("Suco", "Laranja", 8.90m, 10, "SUCO1");

            await context.Clientes.AddAsync(cliente);
            await context.Produtos.AddAsync(produto);
            await context.SaveChangesAsync();

            var service = new PedidoService(
                new PedidoRepository(context),
                new ClienteRepository(context),
                new ProdutoRepository(context),
                new DeliveryRepository(context),
                new PedidoCriarValidator());

            var pedido = await service.CriarRascunhoAsync(new PedidoCriarDto
            {
                ClienteId = cliente.ClienteId,
                Tipo = TipoPedido.Retirada,
                Itens =
                {
                    new PedidoItemCriarDto
                    {
                        ProdutoId = produto.ProdutoId,
                        Quantidade = 1
                    }
                }
            }, Guid.NewGuid());

            var pedidoConfirmado = await service.ConfirmarPedidoAsync(pedido.PedidoId);

            Assert.Equal(StatusPedido.Confirmado, pedidoConfirmado.Status);
            Assert.Null(pedidoConfirmado.DeliveryId);
            Assert.Empty(context.Deliveries);
        }
    }
}
