using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.DTOs
{
    public class PessoaReadDto
    {
        public Guid PessoaId { get; set; } = Guid.NewGuid();
        public string NomePessoa { get; set; } = string.Empty;
        public string EmailPessoa { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string TelefonePessoa { get; set; } = string.Empty;
        public string DocumentoPessoa { get; set; } = string.Empty;
        public Endereco EnderecoPessoa { get; set; }
    }
}
