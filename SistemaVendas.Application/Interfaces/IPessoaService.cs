using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IClienteService
    {
        Task<ClienteReadDto> CriarClienteAsync(ClienteCreateDto dto);
        Task<IEnumerable<ClienteReadDto>> BuscarClientesAsync();
        Task<ClienteReadDto> BuscarClientePorIdAsync(Guid id);
        Task<ClienteReadDto> AtualizarClienteAsync(Guid id, ClienteAtualizarDto clienteAtualizarDto);
        Task DeletarClienteAsync(Guid id);
    }
}
