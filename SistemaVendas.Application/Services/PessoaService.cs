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
            var pessoa = await _repository.BuscarPessoaIdAsync(id);

            if (pessoa is null)
                throw new Exception("Cadastro de pessoa não encontrado no sistema.");

            return MapearParaResponse(pessoa);
        }

        public async Task<IEnumerable<PessoaReadDto>> BuscarPessoasAsync()
        {
            var pessoas = await _repository.BuscarPessoasAsync();

            if (pessoas is null)
                throw new Exception("Cadastros de pessoas não encontrados.");

            return pessoas.Select(MapearParaResponse).ToList();
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
            var pessoa = await _repository.BuscarPessoaIdAsync(id);

            if (pessoa is null)
                throw new Exception("cadastro de pessoa não encontrado.");

            //Atualizar dados da pessoa
            pessoa.NomePessoa = pessoaAtualizarDto.NomePessoa;
            pessoa.EmailPessoa = pessoaAtualizarDto.EmailPessoa;
            pessoa.DataNascimento = pessoaAtualizarDto.DataNascimento;
            pessoa.TelefonePessoa = pessoaAtualizarDto.TelefonePessoa;
            pessoa.DocumentoPessoa = pessoaAtualizarDto.DocumentoPessoa;

            // Busca dados do CEP na API externa
            var dadosCep = await _cepService.BuscarCepAsync(pessoaAtualizarDto.Cep);

            if (dadosCep is null)
                throw new Exception("CEP inválido ou não encontrado.");

            // Atualiza o endereço da pessoa
            pessoa.EnderecoPessoa.Cep = pessoaAtualizarDto.Cep;
            pessoa.EnderecoPessoa.Numero = pessoaAtualizarDto.Numero;
            pessoa.EnderecoPessoa.Complemento = pessoaAtualizarDto.Complemento;
            pessoa.EnderecoPessoa.Logradouro = dadosCep.Logradouro;
            pessoa.EnderecoPessoa.Bairro = dadosCep.Bairro;
            pessoa.EnderecoPessoa.Cidade = dadosCep.Cidade;
            pessoa.EnderecoPessoa.Estado = dadosCep.Estado;

            await _repository.AtualizarPessoaAsync(id, pessoa);
        }

        public async Task DeletarPessoaAsync(Guid id)
        {
            var pessoa = await _repository.BuscarPessoaIdAsync(id);

            if (pessoa is null)
                throw new Exception("Cadastro de pessoa não encontrado.");

            await _repository.DeletarPessoaAsync(pessoa.PessoaId);
        }

        private static PessoaReadDto MapearParaResponse(Pessoa pessoa)
        {
            return new PessoaReadDto
            {
                PessoaId = pessoa.PessoaId,
                NomePessoa = pessoa.NomePessoa,
                EmailPessoa = pessoa.EmailPessoa,
                DataNascimento = pessoa.DataNascimento,
                TelefonePessoa = pessoa.TelefonePessoa,
                DocumentoPessoa = pessoa.DocumentoPessoa,
                EnderecoPessoa = pessoa.EnderecoPessoa
            };
        }
    }
}
