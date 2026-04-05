using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class DeliveryRepository : IDeliveryRepository
    {
        private readonly AppDbContext _context;

        public DeliveryRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Delivery> AdicionarAsync(Delivery delivery)
        {
            await _context.Deliveries.AddAsync(delivery);
            await _context.SaveChangesAsync();
            return delivery;
        }

        public async Task<Delivery?> BuscarPorIdAsync(Guid deliveryId)
        {
            return await _context.Deliveries
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.ClienteEndereco)
                .FirstOrDefaultAsync(d => d.DeliveryId == deliveryId);
        }

        public async Task<IEnumerable<Delivery>> BuscarTodosAsync()
        {
            return await _context.Deliveries
                .AsNoTracking()
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.ClienteEndereco)
                .ToListAsync();
        }

        public async Task<IEnumerable<Delivery>> BuscarPendentesAsync()
        {
            return await _context.Deliveries
                .AsNoTracking()
                .Include(d => d.Pedido)
                    .ThenInclude(p => p.Cliente)
                .Include(d => d.ClienteEndereco)
                .Where(d => d.Status == StatusDelivery.Pendente)
                .ToListAsync();
        }

        public async Task AtualizarAsync(Delivery delivery)
        {
            _context.Deliveries.Update(delivery);
            await _context.SaveChangesAsync();
        }
    }
}
