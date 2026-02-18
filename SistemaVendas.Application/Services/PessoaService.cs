using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class PessoaService : IPessoaService
    {
        private readonly IPessoaRepository _repository;
        private readonly ICepService _cepService;

        public PessoaService(IPessoaRepository repository, ICepService cepService)
        {
            _repository = repository;
            _cepService = cepService;
        }

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
            var cep = System.Text.RegularExpressions.Regex.Replace(dto.Cep ?? string.Empty, "\\D", string.Empty);

            var cepResult = await _cepService.BuscarCepAsync(cep);

            var endereco = new Endereco
            {
                EnderecoId = Guid.NewGuid(),
                Cep = cepResult.Cep,
                Logradouro = cepResult.Logradouro,
                Bairro = cepResult.Bairro,
                Cidade = cepResult.Cidade,
                Estado = cepResult.Estado,
                Numero = dto.Numero,
                Complemento = dto.Complemento
            };

            var pessoa = new Pessoa
            {
                PessoaId = Guid.NewGuid(),
                NomePessoa = dto.NomePessoa,
                EmailPessoa = dto.EmailPessoa,
                DataNascimento = dto.DataNascimento,
                TelefonePessoa = dto.TelefonePessoa,
                DocumentoPessoa = dto.DocumentoPessoa,
                EnderecoId = endereco.EnderecoId,
                EnderecoPessoa = endereco
            };

            await _repository.CriarPessoaAsync(pessoa);
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
