namespace SistemaVendas.Domain.Entities
{
    public class ClienteEndereco
    {
        public Guid ClienteEnderecoId { get; set; } = Guid.NewGuid();
        public Guid ClienteId { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
        public DateTime CriadoEm { get; set; } = DateTime.UtcNow;
        public Cliente? Cliente { get; set; }
    }
}
