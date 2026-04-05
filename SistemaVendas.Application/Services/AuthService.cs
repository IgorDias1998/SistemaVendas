using FluentValidation;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Domain.Enums;

namespace SistemaVendas.Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;
        private readonly ITokenService _tokenService;
        private readonly IValidator<AuthLoginDto> _loginValidator;
        private readonly IValidator<UsuarioCriarDto> _usuarioValidator;

        public AuthService(
            IUsuarioRepository usuarioRepository,
            IPasswordHasher passwordHasher,
            ITokenService tokenService,
            IValidator<AuthLoginDto> loginValidator,
            IValidator<UsuarioCriarDto> usuarioValidator)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
            _tokenService = tokenService;
            _loginValidator = loginValidator;
            _usuarioValidator = usuarioValidator;
        }

        public async Task<UsuarioReadDto> BootstrapAdminAsync(UsuarioCriarDto usuarioDto)
        {
            await _usuarioValidator.ValidateAndThrowAsync(usuarioDto);

            if (await _usuarioRepository.ExisteAlgumUsuarioAsync())
                throw new InvalidOperationException("O bootstrap inicial so pode ser executado quando nao ha usuarios cadastrados.");

            var admin = new Usuario(
                usuarioDto.Nome,
                usuarioDto.Email,
                _passwordHasher.HashPassword(usuarioDto.Senha),
                UserRole.Admin);

            var usuarioSalvo = await _usuarioRepository.AdicionarAsync(admin);

            return new UsuarioReadDto
            {
                UsuarioId = usuarioSalvo.UsuarioId,
                Nome = usuarioSalvo.Nome,
                Email = usuarioSalvo.Email,
                Role = usuarioSalvo.Role,
                EhAtivo = usuarioSalvo.EhAtivo,
                CriadoEm = usuarioSalvo.CriadoEm
            };
        }

        public async Task<AuthResponseDto> LoginAsync(AuthLoginDto loginDto)
        {
            if (loginDto is null)
                throw new ArgumentNullException(nameof(loginDto));

            await _loginValidator.ValidateAndThrowAsync(loginDto);

            var usuario = await _usuarioRepository.BuscarPorEmailAsync(loginDto.Email.Trim().ToLowerInvariant());

            if (usuario is null || !_passwordHasher.VerifyPassword(loginDto.Senha, usuario.SenhaHash))
                throw new UnauthorizedAccessException("E-mail ou senha invalidos.");

            if (!usuario.EhAtivo)
                throw new UnauthorizedAccessException("Usuario inativo.");

            return _tokenService.GenerateToken(usuario);
        }
    }
}
