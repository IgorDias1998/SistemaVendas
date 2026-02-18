using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Application.Services
{
    public class PessoaService : IPessoaService
    {

        public async Task<PessoaReadDto> BuscarPessoaIdAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<PessoaReadDto>> BuscarPessoasAsync()
        {
            throw new NotImplementedException();
        }

        public async Task CriarPessoaAsync(PessoaCreateDto dto)
        {
            throw new NotImplementedException();
        }

        public async Task AtualizarPessoaAsync(Guid id, PessoaAtualizarDto pessoaAtualizarDto)
        {
            throw new NotImplementedException();
        }

        public async Task<PessoaReadDto> DeletarPessoaAsync(Guid id)
        {
            throw new NotImplementedException();
        }
    }
}
