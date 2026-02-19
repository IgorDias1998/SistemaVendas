using Microsoft.EntityFrameworkCore;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Infrastructure.Persistence;

namespace SistemaVendas.Infrastructure.Repositories
{
    public class PessoaRepository : IPessoaRepository
    {
        private readonly AppDbContext _context;

        public PessoaRepository(AppDbContext context)
        {
            _context = context;
        }

        public Task<Pessoa> BuscarPessoaIdAsync(Guid id)
        {
            return _context.Pessoas
                .Include(p => p.EnderecoPessoa)
                .FirstOrDefaultAsync(p => p.PessoaId == id)!;
        }

        public Task<IEnumerable<Pessoa>> BuscarPessoasAsync()
        {
            return _context.Pessoas
                .Include(p => p.EnderecoPessoa)
                .ToListAsync()
                .ContinueWith<Task<IEnumerable<Pessoa>>>(t => Task.FromResult<IEnumerable<Pessoa>>(t.Result))
                .Unwrap();
        }

        public async Task CriarPessoaAsync(Pessoa pessoa)
        {
            await _context.Pessoas.AddAsync(pessoa);
            await _context.SaveChangesAsync();
        }

        public Task AtualizarPessoaAsync(Guid id, Pessoa pessoa)
        {
            _context.Pessoas.Update(pessoa);
            return _context.SaveChangesAsync();
        }

        public Task DeletarPessoaAsync(Guid id)
        {
            return DeleteAsync(id);
        }

        private async Task<Pessoa> DeleteAsync(Guid id)
        {
            var pessoa = await _context.Pessoas
                .Include(p => p.EnderecoPessoa)
                .FirstOrDefaultAsync(p => p.PessoaId == id);

            if (pessoa is null)
                return null!;

            _context.Pessoas.Remove(pessoa);
            await _context.SaveChangesAsync();
            return pessoa;
        }
    }
}
