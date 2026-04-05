using SistemaVendas.Application.DTOs;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Interfaces
{
    public interface ITokenService
    {
        AuthResponseDto GenerateToken(Usuario usuario);
    }
}
