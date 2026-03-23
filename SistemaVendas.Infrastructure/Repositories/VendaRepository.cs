using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class VendaRepository : IVendaRepository
    {
        private readonly AppDbContext _context;

        public VendaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task AdicionarListaVendaAsync(List<Venda> vendas)
        {
            throw new NotImplementedException();
        }

        public Task AdicionarVendaAsync(Venda venda)
        {
            throw new NotImplementedException();
        }

        public Task AtualizarVendaAsync(Venda venda)
        {
            throw new NotImplementedException();
        }

        public Task<Venda?> BuscarVendaPorIdAsync(int vendaId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Venda>> BuscarVendasAsync()
        {
            throw new NotImplementedException();
        }

        public Task DeletarVendaAsync(int vendaId)
        {
            throw new NotImplementedException();
        }
    }
}
