namespace SistemaVendas.Application.DTOs
{
    public class PessoaAtualizarDto
    {
        public string NomePessoa { get; set; } = string.Empty;
        public string EmailPessoa { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string TelefonePessoa { get; set; } = string.Empty;
        public string DocumentoPessoa { get; set; } = string.Empty;
        public string Cep { get; set; } = string.Empty;
        public string Numero { get; set; } = string.Empty;
        public string? Complemento { get; set; }
    }
}
