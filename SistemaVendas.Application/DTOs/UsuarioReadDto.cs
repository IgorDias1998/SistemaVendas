using SistemaVendas.Domain.Enums;

namespace SistemaVendas.Application.DTOs
{
    public class UsuarioReadDto
    {
        public Guid UsuarioId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public UserRole Role { get; set; }
        public bool EhAtivo { get; set; }
        public DateTime CriadoEm { get; set; }
    }
}
