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
            return _context.Pessoas.FindAsync(id).AsTask();
        }

        public Task<IEnumerable<Pessoa>> BuscarPessoasAsync()
        {
            return Task.FromResult(_context.Pessoas.AsEnumerable());
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

        public Task<Pessoa> DeletarPessoaAsync(Guid id)
        {
            var pessoa = _context.Pessoas.Find(id);
            if (pessoa is null)
                return Task.FromResult<Pessoa>(null!);

            _context.Pessoas.Remove(pessoa);
            _context.SaveChangesAsync();
            return Task.FromResult(pessoa);
        }
    }
}
