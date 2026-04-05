using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface IVendaService
    {
        Task<VendaReadDto> AdicionarVendaAsync(VendaCriarDto vendaDto);
        Task<IEnumerable<VendaReadDto>> BuscarVendasAsync();
        Task<VendaReadDto?> BuscarVendaPorIdAsync(int vendaId);
        Task<VendaReadDto> AtualizarVendaAsync(int vendaId, VendaAtualizarDto vendaDto);
        Task DeletarVendaAsync(int vendaId);
    }
}
