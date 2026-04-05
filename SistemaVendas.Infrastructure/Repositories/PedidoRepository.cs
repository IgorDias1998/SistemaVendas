using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly AppDbContext _context;

        public PedidoRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Pedido> AdicionarAsync(Pedido pedido)
        {
            await _context.Pedidos.AddAsync(pedido);
            await _context.SaveChangesAsync();
            return pedido;
        }

        public async Task<Pedido?> BuscarPorIdAsync(Guid pedidoId)
        {
            return await _context.Pedidos
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Include(p => p.Delivery)
                .Include(p => p.Cliente)
                    .ThenInclude(c => c.Enderecos)
                .FirstOrDefaultAsync(p => p.PedidoId == pedidoId);
        }

        public async Task<IEnumerable<Pedido>> BuscarTodosAsync()
        {
            return await _context.Pedidos
                .AsNoTracking()
                .Include(p => p.Itens)
                    .ThenInclude(i => i.Produto)
                .Include(p => p.Delivery)
                .ToListAsync();
        }

        public async Task AtualizarAsync(Pedido pedido)
        {
            _context.Pedidos.Update(pedido);
            await _context.SaveChangesAsync();
        }
    }
}
