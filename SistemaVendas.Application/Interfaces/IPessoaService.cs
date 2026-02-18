using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IPessoaService
    {
        public Task CriarPessoaAsync(PessoaCreateDto dto);
        public Task<IEnumerable<PessoaReadDto>> BuscarPessoasAsync();
        public Task<PessoaReadDto> BuscarPessoaIdAsync(Guid id);
        public Task AtualizarPessoaAsync(Guid id, PessoaAtualizarDto pessoaAtualizarDto);
        public Task<PessoaReadDto> DeletarPessoaAsync(Guid id);
    }
}
