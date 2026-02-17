namespace SistemaVendas.Domain.Entities
{
    public class Pessoa
    {
        public Guid PessoaId { get; set; } = Guid.NewGuid();
        public string NomePessoa { get; set; } = string.Empty;
        public string EmailPessoa { get; set; } = string.Empty;
        public DateTime DataNascimento { get; set; }
        public string TelefonePessoa { get; set; } = string.Empty;
        public string DocumentoPessoa { get; set; } = string.Empty;
        public Endereco EnderecoPessoa { get; private set; }
    }
}
