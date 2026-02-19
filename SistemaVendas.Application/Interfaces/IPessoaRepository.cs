using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IPessoaRepository
    {
        public Task CriarPessoaAsync(Pessoa dto);
        public Task<IEnumerable<Pessoa>> BuscarPessoasAsync();
        public Task<Pessoa> BuscarPessoaIdAsync(Guid id);
        public Task AtualizarPessoaAsync(Guid id, Pessoa pessoa);
        public Task DeletarPessoaAsync(Guid id);
    }
}
