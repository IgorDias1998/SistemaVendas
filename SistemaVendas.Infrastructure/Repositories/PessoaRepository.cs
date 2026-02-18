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
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Pessoa>> BuscarPessoasAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CriarPessoaAsync(Pessoa pessoa)
        {
            await _context.Pessoas.AddAsync(pessoa);
            await _context.SaveChangesAsync();
        }

        public Task AtualizarPessoaAsync(Guid id, Pessoa pessoa)
        {
            throw new NotImplementedException();
        }

        public Task<Pessoa> DeletarPessoaAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
