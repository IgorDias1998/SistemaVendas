using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class UsuarioRepository : IUsuarioRepository
    {
        private readonly AppDbContext _context;

        public UsuarioRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> AdicionarAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            return usuario;
        }

        public async Task<Usuario?> BuscarPorIdAsync(Guid usuarioId)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.UsuarioId == usuarioId);
        }

        public async Task<Usuario?> BuscarPorEmailAsync(string email)
        {
            var emailNormalizado = email.Trim().ToLowerInvariant();
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == emailNormalizado);
        }

        public async Task<IEnumerable<Usuario>> BuscarTodosAsync()
        {
            return await _context.Usuarios
                .AsNoTracking()
                .OrderBy(u => u.Nome)
                .ToListAsync();
        }

        public async Task<bool> ExisteAlgumUsuarioAsync()
        {
            return await _context.Usuarios.AnyAsync();
        }

        public async Task AtualizarAsync(Usuario usuario)
        {
            _context.Usuarios.Update(usuario);
            await _context.SaveChangesAsync();
        }
    }
}
