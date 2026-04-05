using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface IClienteRepository
    {
        Task<Cliente> CriarClienteAsync(Cliente cliente);
        Task<IEnumerable<Cliente>> BuscarClientesAsync();
        Task<Cliente?> BuscarClientePorIdAsync(Guid id);
        Task AtualizarClienteAsync(Cliente cliente);
        Task DeletarClienteAsync(Guid id);
    }
}
