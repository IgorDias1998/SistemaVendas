using Moq;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Application.Services;
using SistemaVendas.Application.Validators;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Domain.Enums;
using SistemaVendas.Infrastructure.Repositories;
using SistemaVendas.Infrastructure.Security;

namespace SistemaVendas.Tests.AuthTests
{
    public class AuthServiceTests
    {
        [Fact(DisplayName = "Bootstrap inicial deve criar usuario admin quando nao houver usuarios.")]
        public async Task BootstrapAdminAsync_DeveCriarAdmin()
        {
            using var context = DbContextFactory.Create();
            var repository = new UsuarioRepository(context);
            var passwordHasher = new PasswordHasherService();
            var tokenService = new Mock<ITokenService>();
            var service = new AuthService(
                repository,
                passwordHasher,
                tokenService.Object,
                new AuthLoginValidator(),
                new UsuarioCriarValidator());

            var dto = new UsuarioCriarDto
            {
                Nome = "Administrador",
                Email = "admin@sistema.com",
                Senha = "123456",
                Role = UserRole.Operador
            };

            var usuario = await service.BootstrapAdminAsync(dto);
            var usuarioSalvo = await repository.BuscarPorEmailAsync(dto.Email);

            Assert.NotNull(usuarioSalvo);
            Assert.Equal(UserRole.Admin, usuario.Role);
            Assert.Equal(UserRole.Admin, usuarioSalvo!.Role);
            Assert.True(passwordHasher.VerifyPassword(dto.Senha, usuarioSalvo.SenhaHash));
        }

        [Fact(DisplayName = "Login deve retornar token quando credenciais forem validas.")]
        public async Task LoginAsync_DeveRetornarToken()
        {
            using var context = DbContextFactory.Create();
            var repository = new UsuarioRepository(context);
            var passwordHasher = new PasswordHasherService();
            var usuario = new Usuario("Operador", "operador@sistema.com", passwordHasher.HashPassword("123456"), UserRole.Operador);
            await repository.AdicionarAsync(usuario);

            var tokenEsperado = new AuthResponseDto
            {
                Token = "token-valido",
                ExpiraEm = DateTime.UtcNow.AddHours(1),
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role
            };

            var tokenService = new Mock<ITokenService>();
            tokenService.Setup(x => x.GenerateToken(It.IsAny<Usuario>()))
                .Returns(tokenEsperado);

            var service = new AuthService(
                repository,
                passwordHasher,
                tokenService.Object,
                new AuthLoginValidator(),
                new UsuarioCriarValidator());

            var resposta = await service.LoginAsync(new AuthLoginDto
            {
                Email = usuario.Email,
                Senha = "123456"
            });

            Assert.Equal(tokenEsperado.Token, resposta.Token);
            Assert.Equal(usuario.UsuarioId, resposta.UsuarioId);
        }
    }
}
