using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IVendaRepository
    {
        public Task AdicionarVendaAsync(Venda venda);
        public Task<IEnumerable<Venda>> BuscarVendasAsync();
        public Task<Venda?> BuscarVendaPorIdAsync(int vendaId);
        public Task AtualizarVendaAsync(Venda venda);
        public Task DeletarVendaAsync(int vendaId);
        Task AdicionarListaVendaAsync(List<Venda> vendas);
    }
}
