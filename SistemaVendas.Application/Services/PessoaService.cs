using System.Text.RegularExpressions;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _repository;
        private readonly ICepService _cepService;

        public ClienteService(IClienteRepository repository, ICepService cepService)
        {
            _repository = repository;
            _cepService = cepService;
        }

        public async Task<ClienteReadDto> BuscarClientePorIdAsync(Guid id)
        {
            var cliente = await _repository.BuscarClientePorIdAsync(id);

            if (cliente is null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            return MapearParaResponse(cliente);
        }

        public async Task<IEnumerable<ClienteReadDto>> BuscarClientesAsync()
        {
            var clientes = await _repository.BuscarClientesAsync();
            return clientes.Select(MapearParaResponse).ToList();
        }

        public async Task<PagedResultDto<ClienteReadDto>> BuscarClientesAsync(ClienteListQueryDto query)
        {
            var clientes = (await _repository.BuscarClientesAsync())
                .Select(MapearParaResponse);

            if (!string.IsNullOrWhiteSpace(query.Search))
            {
                var search = query.Search.Trim().ToLowerInvariant();
                clientes = clientes.Where(c =>
                    c.Nome.ToLowerInvariant().Contains(search) ||
                    c.Documento.ToLowerInvariant().Contains(search) ||
                    c.Telefone.ToLowerInvariant().Contains(search));
            }

            if (query.EstaAtivo.HasValue)
                clientes = clientes.Where(c => c.EstaAtivo == query.EstaAtivo.Value);

            clientes = clientes.OrderBy(c => c.Nome).ToList();

            return PaginacaoHelper.AplicarPaginacao(clientes, query);
        }

        public async Task<ClienteReadDto> CriarClienteAsync(ClienteCreateDto dto)
        {
            var cep = Regex.Replace(dto.Cep ?? string.Empty, "\\D", string.Empty);
            var cepResult = await _cepService.BuscarCepAsync(cep);

            var endereco = new ClienteEndereco
            {
                ClienteEnderecoId = Guid.NewGuid(),
                Cep = cepResult.Cep,
                Logradouro = cepResult.Logradouro,
                Bairro = cepResult.Bairro,
                Cidade = cepResult.Cidade,
                Estado = cepResult.Estado,
                Numero = dto.Numero,
                Complemento = dto.Complemento
            };

            var cliente = new Cliente(dto.Nome, dto.Telefone, dto.Documento);
            cliente.DefinirEnderecos(new[] { endereco });

            var clienteSalvo = await _repository.CriarClienteAsync(cliente);
            return MapearParaResponse(clienteSalvo);
        }

        public async Task<ClienteReadDto> AtualizarClienteAsync(Guid id, ClienteAtualizarDto clienteAtualizarDto)
        {
            var cliente = await _repository.BuscarClientePorIdAsync(id);

            if (cliente is null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            cliente.AtualizarDados(clienteAtualizarDto.Nome, clienteAtualizarDto.Telefone, clienteAtualizarDto.Documento);

            var dadosCep = await _cepService.BuscarCepAsync(clienteAtualizarDto.Cep);
            var endereco = cliente.Enderecos.FirstOrDefault();

            if (endereco is null)
            {
                endereco = new ClienteEndereco
                {
                    ClienteEnderecoId = Guid.NewGuid(),
                    ClienteId = cliente.ClienteId
                };
                cliente.DefinirEnderecos(new[] { endereco });
            }

            endereco.Cep = dadosCep.Cep;
            endereco.Numero = clienteAtualizarDto.Numero;
            endereco.Complemento = clienteAtualizarDto.Complemento;
            endereco.Logradouro = dadosCep.Logradouro;
            endereco.Bairro = dadosCep.Bairro;
            endereco.Cidade = dadosCep.Cidade;
            endereco.Estado = dadosCep.Estado;

            await _repository.AtualizarClienteAsync(cliente);

            return MapearParaResponse(cliente);
        }

        public async Task DeletarClienteAsync(Guid id)
        {
            var cliente = await _repository.BuscarClientePorIdAsync(id);

            if (cliente is null)
                throw new KeyNotFoundException("Cliente não encontrado.");

            await _repository.DeletarClienteAsync(cliente.ClienteId);
        }

        private static ClienteReadDto MapearParaResponse(Cliente cliente)
        {
            return new ClienteReadDto
            {
                ClienteId = cliente.ClienteId,
                Nome = cliente.Nome,
                Telefone = cliente.Telefone,
                Documento = cliente.Documento,
                EstaAtivo = cliente.EstaAtivo,
                CriadoEm = cliente.CriadoEm,
                AlteradoEm = cliente.AlteradoEm,
                Enderecos = cliente.Enderecos.Select(endereco => new ClienteEnderecoReadDto
                {
                    ClienteEnderecoId = endereco.ClienteEnderecoId,
                    Cep = endereco.Cep,
                    Logradouro = endereco.Logradouro,
                    Bairro = endereco.Bairro,
                    Cidade = endereco.Cidade,
                    Estado = endereco.Estado,
                    Numero = endereco.Numero,
                    Complemento = endereco.Complemento
                }).ToList()
            };
        }
    }
}
