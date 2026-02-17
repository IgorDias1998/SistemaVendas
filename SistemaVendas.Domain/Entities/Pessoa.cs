namespace SistemaVendas.Domain.Entities
{
    public class Pessoa
    {
        public Guid ClienteId { get; set; } = Guid.NewGuid();
        public string NomeCliente { get; set; } = string.Empty;
        public string EmailCliente { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string TelefoneCliente { get; set; } = string.Empty;
        public string DocumentoCliente { get; set; } = string.Empty;
        public Endereco EnderecoCliente { get; private set; } = null!;
    }
}
