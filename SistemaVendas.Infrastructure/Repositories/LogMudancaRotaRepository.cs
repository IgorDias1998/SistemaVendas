using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class LogMudancaRotaRepository : ILogMudancaRotaRepository
    {
        private readonly AppDbContext _context;

        public LogMudancaRotaRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AdicionarAsync(LogMudancaRota log)
        {
            await _context.LogsMudancaRota.AddAsync(log);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<LogMudancaRota>> BuscarPorRotaAsync(Guid rotaId)
        {
            return await _context.LogsMudancaRota
                .AsNoTracking()
                .Where(l => l.RotaId == rotaId)
                .OrderByDescending(l => l.MudouEm)
                .ToListAsync();
        }
    }
}
