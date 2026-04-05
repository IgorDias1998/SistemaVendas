using SistemaVendas.Domain.Enums;

namespace SistemaVendas.Application.DTOs
{
    public class UsuarioCriarDto
    {
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Senha { get; set; } = string.Empty;
        public UserRole Role { get; set; } = UserRole.Operador;
    }
}
