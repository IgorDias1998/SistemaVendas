using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPasswordHasher _passwordHasher;

        public UsuarioService(IUsuarioRepository usuarioRepository, IPasswordHasher passwordHasher)
        {
            _usuarioRepository = usuarioRepository;
            _passwordHasher = passwordHasher;
        }

        public async Task<UsuarioReadDto> CriarUsuarioAsync(UsuarioCriarDto usuarioDto)
        {
            if (usuarioDto is null)
                throw new ArgumentNullException(nameof(usuarioDto));

            var email = usuarioDto.Email.Trim().ToLowerInvariant();
            var usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(email);

            if (usuarioExistente is not null)
                throw new InvalidOperationException("Já existe um usuário com este e-mail.");

            var usuario = new Usuario(
                usuarioDto.Nome,
                email,
                _passwordHasher.HashPassword(usuarioDto.Senha),
                usuarioDto.Role
            );

            var usuarioSalvo = await _usuarioRepository.AdicionarAsync(usuario);
            return MapearParaResponse(usuarioSalvo);
        }

        public async Task<IEnumerable<UsuarioReadDto>> BuscarUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.BuscarTodosAsync();
            return usuarios.Select(MapearParaResponse).ToList();
        }

        public async Task<UsuarioReadDto> BuscarUsuarioPorIdAsync(Guid usuarioId)
        {
            var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId);

            if (usuario is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            return MapearParaResponse(usuario);
        }

        public async Task<UsuarioReadDto> AtualizarRoleAsync(Guid usuarioId, UsuarioRoleAtualizarDto roleDto)
        {
            var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId);

            if (usuario is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            usuario.DefinirRole(roleDto.Role);
            await _usuarioRepository.AtualizarAsync(usuario);

            return MapearParaResponse(usuario);
        }

        public async Task<UsuarioReadDto> AlterarStatusAsync(Guid usuarioId, bool ativo)
        {
            var usuario = await _usuarioRepository.BuscarPorIdAsync(usuarioId);

            if (usuario is null)
                throw new KeyNotFoundException("Usuário não encontrado.");

            if (ativo)
                usuario.Ativar();
            else
                usuario.Desativar();

            await _usuarioRepository.AtualizarAsync(usuario);

            return MapearParaResponse(usuario);
        }

        private static UsuarioReadDto MapearParaResponse(Usuario usuario)
        {
            return new UsuarioReadDto
            {
                UsuarioId = usuario.UsuarioId,
                Nome = usuario.Nome,
                Email = usuario.Email,
                Role = usuario.Role,
                EhAtivo = usuario.EhAtivo,
                CriadoEm = usuario.CriadoEm
            };
        }
    }
}
