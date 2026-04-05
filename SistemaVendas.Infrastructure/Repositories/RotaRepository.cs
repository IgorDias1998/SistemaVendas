using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class RotaRepository : IRotaRepository
    {
        private readonly AppDbContext _context;

        public RotaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Rota> AdicionarAsync(Rota rota)
        {
            await _context.Rotas.AddAsync(rota);
            await _context.SaveChangesAsync();
            return rota;
        }

        public async Task<Rota?> BuscarPorIdAsync(Guid rotaId)
        {
            return await _context.Rotas
                .Include(r => r.Paradas.OrderBy(p => p.StopOrder))
                    .ThenInclude(p => p.Delivery)
                        .ThenInclude(d => d.Pedido)
                            .ThenInclude(p => p!.Cliente)
                .Include(r => r.Paradas)
                    .ThenInclude(p => p.Delivery)
                        .ThenInclude(d => d.ClienteEndereco)
                .FirstOrDefaultAsync(r => r.RotaId == rotaId);
        }

        public async Task<IEnumerable<Rota>> BuscarTodosAsync()
        {
            return await _context.Rotas
                .AsNoTracking()
                .Include(r => r.Paradas.OrderBy(p => p.StopOrder))
                    .ThenInclude(p => p.Delivery)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rota>> BuscarPorEntregadorIdAsync(Guid entregadorId)
        {
            return await _context.Rotas
                .AsNoTracking()
                .Where(r => r.AssociadoAoEntregadorId == entregadorId)
                .Include(r => r.Paradas.OrderBy(p => p.StopOrder))
                    .ThenInclude(p => p.Delivery)
                        .ThenInclude(d => d.Pedido)
                            .ThenInclude(p => p!.Cliente)
                .Include(r => r.Paradas)
                    .ThenInclude(p => p.Delivery)
                        .ThenInclude(d => d.ClienteEndereco)
                .ToListAsync();
        }

        public async Task AtualizarAsync(Rota rota)
        {
            _context.Rotas.Update(rota);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> AlgumaDeliveryEmRotaAtivaAsync(IEnumerable<Guid> deliveryIds)
        {
            var statusesAtivos = new[] { StatusRota.Rascunho, StatusRota.Atribuida, StatusRota.EmProgresso };

            return await _context.ParadasRota
                .AnyAsync(p => deliveryIds.Contains(p.DeliveryId) && statusesAtivos.Contains(p.Rota!.Status));
        }
    }
}
