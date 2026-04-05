using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly AppDbContext _context;

        public ClienteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Cliente?> BuscarClientePorIdAsync(Guid id)
        {
            return await _context.Clientes
                .Include(c => c.Enderecos)
                .FirstOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task<IEnumerable<Cliente>> BuscarClientesAsync()
        {
            return await _context.Clientes
                .AsNoTracking()
                .Include(c => c.Enderecos)
                .ToListAsync();
        }

        public async Task<Cliente> CriarClienteAsync(Cliente cliente)
        {
            await _context.Clientes.AddAsync(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task AtualizarClienteAsync(Cliente cliente)
        {
            _context.Clientes.Update(cliente);
            await _context.SaveChangesAsync();
        }

        public async Task DeletarClienteAsync(Guid id)
        {
            var cliente = await _context.Clientes
                .Include(c => c.Enderecos)
                .FirstOrDefaultAsync(c => c.ClienteId == id);

            if (cliente is null)
                return;

            _context.Clientes.Remove(cliente);
            await _context.SaveChangesAsync();
        }
    }
}
