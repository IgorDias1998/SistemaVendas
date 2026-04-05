using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class VendaService : IVendaService
    {
        private readonly IVendaRepository _vendaRepository;
        private readonly IProdutoRepository _produtoRepository;

        public VendaService(IVendaRepository vendaRepository, IProdutoRepository produtoRepository)
        {
            _vendaRepository = vendaRepository;
            _produtoRepository = produtoRepository;
        }

        public async Task<VendaReadDto> AdicionarVendaAsync(VendaCriarDto vendaDto)
        {
            if (vendaDto is null)
                throw new ArgumentNullException(nameof(vendaDto));

            if (vendaDto.ItensVenda is null || vendaDto.ItensVenda.Count == 0)
                throw new ArgumentException("A venda deve possuir ao menos um item.", nameof(vendaDto));

            var itensVenda = new List<ItemVenda>();
            decimal valorTotalVenda = 0;

            foreach (var itemDto in vendaDto.ItensVenda)
            {
                if (itemDto.Quantidade <= 0)
                    throw new ArgumentException("A quantidade do item deve ser maior que zero.", nameof(vendaDto));

                var produto = await _produtoRepository.BuscarProdutoPorIdAsync(itemDto.ProdutoId);

                if (produto is null)
                    throw new KeyNotFoundException($"Produto não encontrado para o id {itemDto.ProdutoId}.");

                produto.RemoverEstoque(itemDto.Quantidade);
                await _produtoRepository.AtualizarProdutoAsync(produto);

                var valorTotalItem = produto.PrecoProduto * itemDto.Quantidade;

                itensVenda.Add(new ItemVenda
                {
                    ProdutoId = produto.ProdutoId,
                    Produto = produto,
                    Quantidade = itemDto.Quantidade,
                    ValorUnitario = produto.PrecoProduto,
                    ValorTotal = valorTotalItem
                });

                valorTotalVenda += valorTotalItem;
            }

            var venda = new Venda
            {
                PessoaId = vendaDto.PessoaId,
                DataVenda = vendaDto.DataVenda ?? DateTime.UtcNow,
                Status = NormalizarStatus(vendaDto.Status),
                ValorTotal = valorTotalVenda,
                ItensVenda = itensVenda
            };

            var vendaSalva = await _vendaRepository.AdicionarVendaAsync(venda);
            var vendaCompleta = await _vendaRepository.BuscarVendaPorIdAsync(vendaSalva.VendaId) ?? vendaSalva;

            return MapearParaResponse(vendaCompleta);
        }

        public async Task<IEnumerable<VendaReadDto>> BuscarVendasAsync()
        {
            var vendas = await _vendaRepository.BuscarVendasAsync();
            return vendas.Select(MapearParaResponse).ToList();
        }

        public async Task<VendaReadDto?> BuscarVendaPorIdAsync(int vendaId)
        {
            var venda = await _vendaRepository.BuscarVendaPorIdAsync(vendaId);

            if (venda is null)
                throw new KeyNotFoundException("Venda não encontrada.");

            return MapearParaResponse(venda);
        }

        public async Task<VendaReadDto> AtualizarVendaAsync(int vendaId, VendaAtualizarDto vendaDto)
        {
            if (vendaDto is null)
                throw new ArgumentNullException(nameof(vendaDto));

            var venda = await _vendaRepository.BuscarVendaPorIdAsync(vendaId);

            if (venda is null)
                throw new KeyNotFoundException("Venda não encontrada.");

            venda.PessoaId = vendaDto.PessoaId;
            venda.DataVenda = vendaDto.DataVenda ?? venda.DataVenda;
            venda.Status = NormalizarStatus(vendaDto.Status);

            var atualizou = await _vendaRepository.AtualizarVendaAsync(venda);

            if (!atualizou)
                throw new InvalidOperationException("Não foi possível atualizar a venda.");

            var vendaAtualizada = await _vendaRepository.BuscarVendaPorIdAsync(vendaId) ?? venda;

            return MapearParaResponse(vendaAtualizada);
        }

        public async Task DeletarVendaAsync(int vendaId)
        {
            var venda = await _vendaRepository.BuscarVendaPorIdAsync(vendaId);

            if (venda is null)
                throw new KeyNotFoundException("Venda não encontrada.");

            foreach (var item in venda.ItensVenda)
            {
                var produto = item.Produto ?? await _produtoRepository.BuscarProdutoPorIdAsync(item.ProdutoId);

                if (produto is null)
                    continue;

                produto.AdicionarEstoque(item.Quantidade);
                await _produtoRepository.AtualizarProdutoAsync(produto);
            }

            var removeu = await _vendaRepository.DeletarVendaAsync(vendaId);

            if (!removeu)
                throw new InvalidOperationException("Não foi possível remover a venda.");
        }

        private static string NormalizarStatus(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return "Pendente";

            return status.Trim();
        }

        private static VendaReadDto MapearParaResponse(Venda venda)
        {
            return new VendaReadDto
            {
                VendaId = venda.VendaId,
                PessoaId = venda.PessoaId,
                DataVenda = venda.DataVenda,
                ValorTotal = venda.ValorTotal,
                Status = venda.Status,
                ItensVenda = venda.ItensVenda.Select(item => new ItemVendaReadDto
                {
                    ItemVendaId = item.ItemVendaId,
                    ProdutoId = item.ProdutoId,
                    TituloProduto = item.Produto?.TituloProduto ?? string.Empty,
                    Quantidade = item.Quantidade,
                    ValorUnitario = item.ValorUnitario,
                    ValorTotal = item.ValorTotal
                }).ToList()
            };
        }
    }
}
