using System.Security.Cryptography;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Infrastructure.Security
{
    public class PasswordHasherService : IPasswordHasher
    {
        private const int SaltSize = 16;
        private const int KeySize = 32;
        private const int Iterations = 100_000;

        public string HashPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("A senha é obrigatória.", nameof(password));

            using var algorithm = new Rfc2898DeriveBytes(password, SaltSize, Iterations, HashAlgorithmName.SHA256);
            var salt = algorithm.Salt;
            var key = algorithm.GetBytes(KeySize);

            return $"{Iterations}.{Convert.ToBase64String(salt)}.{Convert.ToBase64String(key)}";
        }

        public bool VerifyPassword(string password, string passwordHash)
        {
            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(passwordHash))
                return false;

            var parts = passwordHash.Split('.');

            if (parts.Length != 3 || !int.TryParse(parts[0], out var iterations))
                return false;

            var salt = Convert.FromBase64String(parts[1]);
            var expectedKey = Convert.FromBase64String(parts[2]);

            using var algorithm = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            var actualKey = algorithm.GetBytes(KeySize);

            return CryptographicOperations.FixedTimeEquals(actualKey, expectedKey);
        }
    }
}
