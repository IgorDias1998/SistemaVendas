namespace SistemaVendas.Domain.Entities
{
    public class Cliente
    {
        public Guid ClienteId { get; private set; } = Guid.NewGuid();
        public string Nome { get; private set; } = string.Empty;
        public string Telefone { get; private set; } = string.Empty;
        public string Documento { get; private set; } = string.Empty;
        public bool EstaAtivo { get; private set; } = true;
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;
        public DateTime AlteradoEm { get; private set; } = DateTime.UtcNow;
        public ICollection<ClienteEndereco> Enderecos { get; private set; } = new List<ClienteEndereco>();

        public Cliente(string nome, string telefone, string documento)
        {
            AtualizarDados(nome, telefone, documento);
        }

        protected Cliente() { }

        public void AtualizarDados(string nome, string telefone, string documento)
        {
            Nome = nome.Trim();
            Telefone = telefone.Trim();
            Documento = documento?.Trim() ?? string.Empty;
            AlteradoEm = DateTime.UtcNow;
        }

        public void DefinirEnderecos(IEnumerable<ClienteEndereco> enderecos)
        {
            Enderecos = enderecos.ToList();
            AlteradoEm = DateTime.UtcNow;
        }

        public void Ativar()
        {
            EstaAtivo = true;
            AlteradoEm = DateTime.UtcNow;
        }

        public void Desativar()
        {
            EstaAtivo = false;
            AlteradoEm = DateTime.UtcNow;
        }
    }
}
