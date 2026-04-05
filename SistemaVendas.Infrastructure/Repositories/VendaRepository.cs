using Microsoft.EntityFrameworkCore;
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

        public async Task AdicionarListaVendaAsync(List<Venda> vendas)
        {
            await _context.Vendas.AddRangeAsync(vendas);
            await _context.SaveChangesAsync();
        }

        public async Task<Venda> AdicionarVendaAsync(Venda venda)
        {
            await _context.Vendas.AddAsync(venda);
            await _context.SaveChangesAsync();

            return venda;
        }

        public async Task<bool> AtualizarVendaAsync(Venda venda)
        {
            _context.Vendas.Update(venda);

            var linhasAfetadas = await _context.SaveChangesAsync();
            return linhasAfetadas > 0;
        }

        public async Task<Venda?> BuscarVendaPorIdAsync(int vendaId)
        {
            return await _context.Vendas
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Include(v => v.ItensVenda)
                    .ThenInclude(iv => iv.Produto)
                .FirstOrDefaultAsync(v => v.VendaId == vendaId);
        }

        public async Task<IEnumerable<Venda>> BuscarVendasAsync()
        {
            return await _context.Vendas
                .AsNoTracking()
                .Include(v => v.Cliente)
                .Include(v => v.ItensVenda)
                    .ThenInclude(iv => iv.Produto)
                .ToListAsync();
        }

        public async Task<bool> DeletarVendaAsync(int vendaId)
        {
            var venda = await _context.Vendas
                .Include(v => v.ItensVenda)
                .FirstOrDefaultAsync(v => v.VendaId == vendaId);

            if (venda is null)
                return false;

            _context.Vendas.Remove(venda);

            var linhasAfetadas = await _context.SaveChangesAsync();
            return linhasAfetadas > 0;
        }
    }
}
