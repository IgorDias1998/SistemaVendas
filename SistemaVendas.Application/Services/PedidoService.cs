using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IDeliveryRepository _deliveryRepository;

        public PedidoService(
            IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository,
            IProdutoRepository produtoRepository,
            IDeliveryRepository deliveryRepository)
        {
            _pedidoRepository = pedidoRepository;
            _clienteRepository = clienteRepository;
            _produtoRepository = produtoRepository;
            _deliveryRepository = deliveryRepository;
        }

        public async Task<PedidoReadDto> CriarRascunhoAsync(PedidoCriarDto pedidoDto)
        {
            if (pedidoDto is null)
                throw new ArgumentNullException(nameof(pedidoDto));

            var cliente = await _clienteRepository.BuscarClientePorIdAsync(pedidoDto.ClienteId);

            if (cliente is null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            var itens = new List<PedidoProduto>();

            foreach (var itemDto in pedidoDto.Itens)
            {
                if (itemDto.Quantidade <= 0)
                    throw new ArgumentException("A quantidade do item deve ser maior que zero.", nameof(pedidoDto));

                var produto = await _produtoRepository.BuscarProdutoPorIdAsync(itemDto.ProdutoId);

                if (produto is null)
                    throw new KeyNotFoundException($"Produto não encontrado para o id {itemDto.ProdutoId}.");

                itens.Add(new PedidoProduto
                {
                    ProdutoId = produto.ProdutoId,
                    Produto = produto,
                    Quantidade = itemDto.Quantidade,
                    PrecoUnitario = produto.PrecoProduto,
                    PrecoTotal = produto.PrecoProduto * itemDto.Quantidade
                });
            }

            var pedido = new Pedido
            {
                ClienteId = pedidoDto.ClienteId,
                CriadoPeloUsuarioId = pedidoDto.CriadoPeloUsuarioId,
                Tipo = pedidoDto.Tipo,
                Status = StatusPedido.Rascunho,
                Observacao = pedidoDto.Observacao,
                Itens = itens
            };

            var pedidoSalvo = await _pedidoRepository.AdicionarAsync(pedido);
            var pedidoCompleto = await _pedidoRepository.BuscarPorIdAsync(pedidoSalvo.PedidoId) ?? pedidoSalvo;

            return MapearParaResponse(pedidoCompleto);
        }

        public async Task<IEnumerable<PedidoReadDto>> BuscarPedidosAsync()
        {
            var pedidos = await _pedidoRepository.BuscarTodosAsync();
            return pedidos.Select(MapearParaResponse).ToList();
        }

        public async Task<PedidoReadDto> BuscarPedidoPorIdAsync(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.BuscarPorIdAsync(pedidoId);

            if (pedido is null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            return MapearParaResponse(pedido);
        }

        public async Task<PedidoReadDto> ConfirmarPedidoAsync(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.BuscarPorIdAsync(pedidoId);

            if (pedido is null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            if (pedido.Status == StatusPedido.Cancelado)
                throw new InvalidOperationException("Pedido cancelado não pode ser confirmado.");

            if (!pedido.Itens.Any())
                throw new InvalidOperationException("O pedido deve possuir ao menos um item para ser confirmado.");

            pedido.Status = StatusPedido.Confirmado;
            pedido.ConfirmadoEm = DateTime.UtcNow;

            if (pedido.Tipo == TipoPedido.Delivery && pedido.Delivery is null)
            {
                var cliente = pedido.Cliente ?? await _clienteRepository.BuscarClientePorIdAsync(pedido.ClienteId);

                if (cliente is null)
                    throw new KeyNotFoundException("Cliente não encontrado para geração do delivery.");

                var endereco = cliente.Enderecos.FirstOrDefault();

                if (endereco is null)
                    throw new InvalidOperationException("O cliente precisa ter endereço para gerar delivery.");

                var delivery = new Delivery
                {
                    PedidoId = pedido.PedidoId,
                    ClienteEnderecoId = endereco.ClienteEnderecoId,
                    Status = StatusDelivery.Pendente
                };

                pedido.Delivery = await _deliveryRepository.AdicionarAsync(delivery);
            }

            await _pedidoRepository.AtualizarAsync(pedido);

            var pedidoAtualizado = await _pedidoRepository.BuscarPorIdAsync(pedidoId) ?? pedido;
            return MapearParaResponse(pedidoAtualizado);
        }

        public async Task<PedidoReadDto> CancelarPedidoAsync(Guid pedidoId)
        {
            var pedido = await _pedidoRepository.BuscarPorIdAsync(pedidoId);

            if (pedido is null)
                throw new KeyNotFoundException("Pedido não encontrado.");

            if (pedido.Status == StatusPedido.Completo)
                throw new InvalidOperationException("Pedido completo não pode ser cancelado.");

            pedido.Status = StatusPedido.Cancelado;

            if (pedido.Delivery is not null)
            {
                pedido.Delivery.Status = StatusDelivery.Cancelado;
                await _deliveryRepository.AtualizarAsync(pedido.Delivery);
            }

            await _pedidoRepository.AtualizarAsync(pedido);

            var pedidoAtualizado = await _pedidoRepository.BuscarPorIdAsync(pedidoId) ?? pedido;
            return MapearParaResponse(pedidoAtualizado);
        }

        private static PedidoReadDto MapearParaResponse(Pedido pedido)
        {
            return new PedidoReadDto
            {
                PedidoId = pedido.PedidoId,
                ClienteId = pedido.ClienteId,
                CriadoPeloUsuarioId = pedido.CriadoPeloUsuarioId,
                Tipo = pedido.Tipo,
                Status = pedido.Status,
                Observacao = pedido.Observacao,
                CriadoEm = pedido.CriadoEm,
                ConfirmadoEm = pedido.ConfirmadoEm,
                DeliveryId = pedido.Delivery?.DeliveryId,
                Itens = pedido.Itens.Select(item => new PedidoItemReadDto
                {
                    PedidoProdutoId = item.PedidoProdutoId,
                    ProdutoId = item.ProdutoId,
                    TituloProduto = item.Produto?.TituloProduto ?? string.Empty,
                    Quantidade = item.Quantidade,
                    PrecoUnitario = item.PrecoUnitario,
                    PrecoTotal = item.PrecoTotal
                }).ToList()
            };
        }
    }
}
