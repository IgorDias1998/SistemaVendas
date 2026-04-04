using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IVendaRepository
    {
        Task<Venda> AdicionarVendaAsync(Venda venda);
        Task<IEnumerable<Venda>> BuscarVendasAsync();
        Task<Venda?> BuscarVendaPorIdAsync(int vendaId);
        Task<bool> AtualizarVendaAsync(Venda venda);
        Task<bool> DeletarVendaAsync(int vendaId);
        Task AdicionarListaVendaAsync(List<Venda> vendas);
    }
}
