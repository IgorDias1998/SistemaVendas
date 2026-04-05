using SistemaVendas.Domain.Enums;

namespace SistemaVendas.Domain.Entities
{
    public class Usuario
    {
        public Guid UsuarioId { get; private set; } = Guid.NewGuid();
        public string Nome { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string SenhaHash { get; private set; } = string.Empty;
        public UserRole Role { get; private set; }
        public bool EhAtivo { get; private set; } = true;
        public DateTime CriadoEm { get; private set; } = DateTime.UtcNow;

        public Usuario(string nome, string email, string senhaHash, UserRole role)
        {
            AtualizarDados(nome, email);
            DefinirSenhaHash(senhaHash);
            DefinirRole(role);
        }

        protected Usuario() { }

        public void AtualizarDados(string nome, string email)
        {
            Nome = ValidarNome(nome);
            Email = ValidarEmail(email);
        }

        public void DefinirSenhaHash(string senhaHash)
        {
            if (string.IsNullOrWhiteSpace(senhaHash))
                throw new ArgumentException("O hash da senha é obrigatório.", nameof(senhaHash));

            SenhaHash = senhaHash.Trim();
        }

        public void DefinirRole(UserRole role)
        {
            Role = role;
        }

        public void Ativar()
        {
            EhAtivo = true;
        }

        public void Desativar()
        {
            EhAtivo = false;
        }

        private static string ValidarNome(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException("O nome do usuário é obrigatório.", nameof(nome));

            nome = nome.Trim();

            if (nome.Length > 200)
                throw new ArgumentException("O nome do usuário deve ter no máximo 200 caracteres.", nameof(nome));

            return nome;
        }

        private static string ValidarEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("O e-mail do usuário é obrigatório.", nameof(email));

            email = email.Trim().ToLowerInvariant();

            if (email.Length > 200)
                throw new ArgumentException("O e-mail do usuário deve ter no máximo 200 caracteres.", nameof(email));

            return email;
        }
    }
}
