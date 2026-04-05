namespace SistemaVendas.Application.DTOs
{
    public class ClienteEnderecoReadDto
    {
        public Guid ClienteEnderecoId { get; set; }
        public string Cep { get; set; } = string.Empty;
        public string Logradouro { get; set; } = string.Empty;
        public string Bairro { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
    }

    public class ClienteReadDto
    {
        public Guid ClienteId { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Documento { get; set; } = string.Empty;
        public bool EstaAtivo { get; set; }
        public DateTime CriadoEm { get; set; }
        public DateTime AlteradoEm { get; set; }
        public List<ClienteEnderecoReadDto> Enderecos { get; set; } = new();
    }
}
