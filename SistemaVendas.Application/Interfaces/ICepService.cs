using SistemaVendas.Application.DTOs;

namespace SistemaVendas.Application.Interfaces
{
    public interface ICepService
    {
        public Task<CepResultDto> BuscarCepAsync(string cep);
    }
}
