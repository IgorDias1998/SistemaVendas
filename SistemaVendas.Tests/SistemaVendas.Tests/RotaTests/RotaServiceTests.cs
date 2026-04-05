using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Domain.Enums;
using SistemaVendas.Infrastructure.Repositories;

namespace SistemaVendas.Tests.RotaTests
{
    public class RotaServiceTests
    {
        [Fact(DisplayName = "Reordenar rota deve registrar log de auditoria.")]
        public async Task ReordenarParadasAsync_DeveRegistrarLog()
        {
            using var context = DbContextFactory.Create();
            var entregador = new Usuario("Entregador", "entregador@sistema.com", "hash", UserRole.Entregador);
            var criador = new Usuario("Operador", "operador@sistema.com", "hash", UserRole.Operador);
            var cliente = new Cliente("Cliente 1", "11977777777", "12312312312");
            cliente.DefinirEnderecos(new[]
            {
                new ClienteEndereco
                {
                    Cep = "01001-000",
                    Logradouro = "Rua A",
                    Bairro = "Centro",
                    Cidade = "Sao Paulo",
                    Estado = "SP",
                    Numero = "10"
                }
            });

            var endereco = cliente.Enderecos.First();
            var pedido1 = new Pedido { ClienteId = cliente.ClienteId, Cliente = cliente, CriadoPeloUsuarioId = criador.UsuarioId, Tipo = TipoPedido.Delivery, Status = StatusPedido.Confirmado };
            var pedido2 = new Pedido { ClienteId = cliente.ClienteId, Cliente = cliente, CriadoPeloUsuarioId = criador.UsuarioId, Tipo = TipoPedido.Delivery, Status = StatusPedido.Confirmado };
            var delivery1 = new Delivery { Pedido = pedido1, PedidoId = pedido1.PedidoId, ClienteEndereco = endereco, ClienteEnderecoId = endereco.ClienteEnderecoId, Status = StatusDelivery.Pendente };
            var delivery2 = new Delivery { Pedido = pedido2, PedidoId = pedido2.PedidoId, ClienteEndereco = endereco, ClienteEnderecoId = endereco.ClienteEnderecoId, Status = StatusDelivery.Pendente };

            await context.Usuarios.AddRangeAsync(criador, entregador);
            await context.Clientes.AddAsync(cliente);
            await context.Pedidos.AddRangeAsync(pedido1, pedido2);
            await context.Deliveries.AddRangeAsync(delivery1, delivery2);
            await context.SaveChangesAsync();

            var service = new RotaService(
                new RotaRepository(context),
                new DeliveryRepository(context),
                new UsuarioRepository(context),
                new LogMudancaRotaRepository(context),
                new RotaCriarValidator(),
                new RotaReordenarParadasValidator());

            var rota = await service.CriarRotaAsync(new RotaCriarDto
            {
                CriadoPeloUsuarioId = criador.UsuarioId,
                EntregadorId = entregador.UsuarioId,
                DeliveryIds = new List<Guid> { delivery1.DeliveryId, delivery2.DeliveryId }
            });

            var ordemInvertida = rota.Paradas.OrderByDescending(x => x.StopOrder).Select(x => x.ParadaRotaId).ToList();

            await service.ReordenarParadasAsync(rota.RotaId, new RotaReordenarParadasDto
            {
                AlteradoPeloUsuarioId = criador.UsuarioId,
                ParadaIdsEmOrdem = ordemInvertida
            });

            var logs = await context.LogsMudancaRota.Where(x => x.RotaId == rota.RotaId).ToListAsync();
            Assert.Contains(logs, x => x.TipoMudanca == TipoMudancaRota.Reordenar);
        }

        [Fact(DisplayName = "Rotas finalizadas nao devem aceitar novas alteracoes.")]
        public async Task FinalizarRotaAsync_NaoDevePermitirReordenarDepois()
        {
            using var context = DbContextFactory.Create();
            var entregador = new Usuario("Entregador", "entregador2@sistema.com", "hash", UserRole.Entregador);
            var criador = new Usuario("Operador", "operador2@sistema.com", "hash", UserRole.Operador);
            var cliente = new Cliente("Cliente 2", "11966666666", "32132132132");
            cliente.DefinirEnderecos(new[]
            {
                new ClienteEndereco
                {
                    Cep = "01001-000",
                    Logradouro = "Rua B",
                    Bairro = "Centro",
                    Cidade = "Sao Paulo",
                    Estado = "SP",
                    Numero = "20"
                }
            });

            var endereco = cliente.Enderecos.First();
            var pedido = new Pedido { ClienteId = cliente.ClienteId, Cliente = cliente, CriadoPeloUsuarioId = criador.UsuarioId, Tipo = TipoPedido.Delivery, Status = StatusPedido.Confirmado };
            var delivery = new Delivery { Pedido = pedido, PedidoId = pedido.PedidoId, ClienteEndereco = endereco, ClienteEnderecoId = endereco.ClienteEnderecoId, Status = StatusDelivery.Pendente };

            await context.Usuarios.AddRangeAsync(criador, entregador);
            await context.Clientes.AddAsync(cliente);
            await context.Pedidos.AddAsync(pedido);
            await context.Deliveries.AddAsync(delivery);
            await context.SaveChangesAsync();

            var service = new RotaService(
                new RotaRepository(context),
                new DeliveryRepository(context),
                new UsuarioRepository(context),
                new LogMudancaRotaRepository(context),
                new RotaCriarValidator(),
                new RotaReordenarParadasValidator());

            var rota = await service.CriarRotaAsync(new RotaCriarDto
            {
                CriadoPeloUsuarioId = criador.UsuarioId,
                EntregadorId = entregador.UsuarioId,
                DeliveryIds = new List<Guid> { delivery.DeliveryId }
            });

            await service.IniciarRotaAsync(rota.RotaId, criador.UsuarioId);
            var rotaFinalizada = await service.FinalizarRotaAsync(rota.RotaId, criador.UsuarioId);

            Assert.Equal(StatusRota.Finalizada, rotaFinalizada.Status);

            await Assert.ThrowsAsync<InvalidOperationException>(() => service.ReordenarParadasAsync(rota.RotaId, new RotaReordenarParadasDto
            {
                AlteradoPeloUsuarioId = criador.UsuarioId,
                ParadaIdsEmOrdem = rotaFinalizada.Paradas.Select(x => x.ParadaRotaId).ToList()
            }));
        }
    }
}
