using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IProdutoRepository _produtoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IRotaRepository _rotaRepository;

        public DashboardService(
            IProdutoRepository produtoRepository,
            IClienteRepository clienteRepository,
            IPedidoRepository pedidoRepository,
            IDeliveryRepository deliveryRepository,
            IRotaRepository rotaRepository)
        {
            _produtoRepository = produtoRepository;
            _clienteRepository = clienteRepository;
            _pedidoRepository = pedidoRepository;
            _deliveryRepository = deliveryRepository;
            _rotaRepository = rotaRepository;
        }

        public async Task<DashboardReadDto> ObterDashboardAsync(Guid usuarioId, string role)
        {
            var produtos = await _produtoRepository.BuscarProdutosAsync();
            var clientes = await _clienteRepository.BuscarClientesAsync();
            var pedidos = await _pedidoRepository.BuscarTodosAsync();
            var deliveries = role == "Entregador"
                ? await _deliveryRepository.BuscarPorEntregadorIdAsync(usuarioId)
                : await _deliveryRepository.BuscarTodosAsync();
            var rotas = role == "Entregador"
                ? await _rotaRepository.BuscarPorEntregadorIdAsync(usuarioId)
                : await _rotaRepository.BuscarTodosAsync();

            return new DashboardReadDto
            {
                TotalProdutos = produtos.Count(),
                TotalClientes = clientes.Count(),
                PedidosRascunho = pedidos.Count(p => p.Status == StatusPedido.Rascunho),
                PedidosConfirmados = pedidos.Count(p => p.Status == StatusPedido.Confirmado),
                DeliveriesPendentes = deliveries.Count(d => d.Status == StatusDelivery.Pendente || d.Status == StatusDelivery.Associado),
                DeliveriesEmRota = deliveries.Count(d => d.Status == StatusDelivery.EmRota),
                RotasRascunho = rotas.Count(r => r.Status == StatusRota.Rascunho),
                RotasEmProgresso = rotas.Count(r => r.Status == StatusRota.EmProgresso),
                MinhasRotas = role == "Entregador" ? rotas.Count() : 0,
                MinhasDeliveriesPendentes = role == "Entregador"
                    ? deliveries.Count(d => d.Status == StatusDelivery.Pendente || d.Status == StatusDelivery.Associado || d.Status == StatusDelivery.EmRota)
                    : 0
            };
        }
    }
}
