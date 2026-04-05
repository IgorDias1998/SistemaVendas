using System.Text.Json;
using FluentValidation;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;
using SistemaVendas.Domain.Enums;

namespace SistemaVendas.Application.Services
{
    public class RotaService : IRotaService
    {
        private readonly IRotaRepository _rotaRepository;
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ILogMudancaRotaRepository _logRepository;
        private readonly IValidator<RotaCriarDto> _rotaCriarValidator;
        private readonly IValidator<RotaReordenarParadasDto> _rotaReordenarValidator;

        public RotaService(
            IRotaRepository rotaRepository,
            IDeliveryRepository deliveryRepository,
            IUsuarioRepository usuarioRepository,
            ILogMudancaRotaRepository logRepository,
            IValidator<RotaCriarDto> rotaCriarValidator,
            IValidator<RotaReordenarParadasDto> rotaReordenarValidator)
        {
            _rotaRepository = rotaRepository;
            _deliveryRepository = deliveryRepository;
            _usuarioRepository = usuarioRepository;
            _logRepository = logRepository;
            _rotaCriarValidator = rotaCriarValidator;
            _rotaReordenarValidator = rotaReordenarValidator;
        }

        public async Task<RotaReadDto> CriarRotaAsync(RotaCriarDto dto, Guid criadoPorUsuarioId)
        {
            await _rotaCriarValidator.ValidateAndThrowAsync(dto);

            if (await _rotaRepository.AlgumaDeliveryEmRotaAtivaAsync(dto.DeliveryIds))
                throw new InvalidOperationException("Uma ou mais deliveries ja estao vinculadas a uma rota ativa.");

            var deliveries = (await _deliveryRepository.BuscarPorIdsAsync(dto.DeliveryIds)).ToList();

            if (deliveries.Count != dto.DeliveryIds.Count)
                throw new KeyNotFoundException("Uma ou mais deliveries nao foram encontradas.");

            if (deliveries.Any(d => d.Status != StatusDelivery.Pendente && d.Status != StatusDelivery.Associado))
                throw new InvalidOperationException("Somente deliveries pendentes podem entrar em uma rota.");

            var rota = new Rota
            {
                CriadoPeloUsuarioId = criadoPorUsuarioId,
                Status = StatusRota.Rascunho,
                Paradas = dto.DeliveryIds.Select((deliveryId, index) => new ParadaRota
                {
                    DeliveryId = deliveryId,
                    StopOrder = index + 1,
                    Status = StatusParadaRota.Pendente
                }).ToList()
            };

            var rotaSalva = await _rotaRepository.AdicionarAsync(rota);

            if (dto.EntregadorId.HasValue)
                return await AtribuirEntregadorAsync(rotaSalva.RotaId, dto.EntregadorId.Value, criadoPorUsuarioId);

            var rotaCompleta = await _rotaRepository.BuscarPorIdAsync(rotaSalva.RotaId) ?? rotaSalva;
            return MapearParaResponse(rotaCompleta);
        }

        public async Task<IEnumerable<RotaReadDto>> BuscarRotasAsync()
        {
            var rotas = await _rotaRepository.BuscarTodosAsync();
            return rotas.Select(MapearParaResponse).ToList();
        }

        public async Task<IEnumerable<RotaReadDto>> BuscarRotasAsync(Guid usuarioId, string role)
        {
            var rotas = role == "Entregador"
                ? await _rotaRepository.BuscarPorEntregadorIdAsync(usuarioId)
                : await _rotaRepository.BuscarTodosAsync();

            return rotas.Select(MapearParaResponse).ToList();
        }

        public async Task<PagedResultDto<RotaReadDto>> BuscarRotasAsync(Guid usuarioId, string role, RotaListQueryDto query)
        {
            var rotas = await BuscarRotasAsync(usuarioId, role);

            var filtradas = rotas.AsEnumerable();

            if (query.EntregadorId.HasValue)
                filtradas = filtradas.Where(r => r.AssociadoAoEntregadorId == query.EntregadorId.Value);

            if (query.Status.HasValue)
                filtradas = filtradas.Where(r => r.Status == query.Status.Value);

            filtradas = filtradas.OrderByDescending(r => r.CriadoEm).ToList();

            return PaginacaoHelper.AplicarPaginacao(filtradas, query);
        }

        public async Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId, Guid usuarioId, string role)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (role == "Entregador" && rota.AssociadoAoEntregadorId != usuarioId)
                throw new UnauthorizedAccessException("O entregador nao pode acessar uma rota que nao pertence a ele.");

            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> AtribuirEntregadorAsync(Guid rotaId, Guid entregadorId, Guid alteradoPorUsuarioId)
        {
            var rota = await ObterRotaEditavelAsync(rotaId);
            var entregador = await _usuarioRepository.BuscarPorIdAsync(entregadorId);

            if (entregador is null)
                throw new KeyNotFoundException("Entregador nao encontrado.");

            if (entregador.Role != UserRole.Entregador)
                throw new InvalidOperationException("O usuario informado nao possui papel de entregador.");

            var oldValue = new
            {
                rota.AssociadoAoEntregadorId,
                rota.Status
            };

            rota.AssociadoAoEntregadorId = entregadorId;
            rota.AtribuidoEm = DateTime.UtcNow;
            rota.Status = StatusRota.Atribuida;

            foreach (var parada in rota.Paradas)
            {
                if (parada.Delivery is not null)
                {
                    parada.Delivery.Status = StatusDelivery.Associado;
                    await _deliveryRepository.AtualizarAsync(parada.Delivery);
                }
            }

            await _rotaRepository.AtualizarAsync(rota);
            await RegistrarLogAsync(rota.RotaId, alteradoPorUsuarioId, TipoMudancaRota.Atribuir, oldValue, new
            {
                rota.AssociadoAoEntregadorId,
                rota.Status
            });

            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> ReordenarParadasAsync(Guid rotaId, RotaReordenarParadasDto dto, Guid alteradoPorUsuarioId)
        {
            await _rotaReordenarValidator.ValidateAndThrowAsync(dto);

            var rota = await ObterRotaEditavelAsync(rotaId);

            var paradaIdsAtuais = rota.Paradas.Select(p => p.ParadaRotaId).OrderBy(id => id).ToList();
            var paradaIdsRecebidos = dto.ParadaIdsEmOrdem.OrderBy(id => id).ToList();

            if (!paradaIdsAtuais.SequenceEqual(paradaIdsRecebidos))
                throw new InvalidOperationException("A lista de paradas enviada nao corresponde as paradas da rota.");

            var oldOrder = rota.Paradas
                .OrderBy(p => p.StopOrder)
                .Select(p => new { p.ParadaRotaId, p.StopOrder })
                .ToList();

            for (var i = 0; i < dto.ParadaIdsEmOrdem.Count; i++)
            {
                var parada = rota.Paradas.First(p => p.ParadaRotaId == dto.ParadaIdsEmOrdem[i]);
                parada.StopOrder = i + 1;
            }

            await _rotaRepository.AtualizarAsync(rota);

            var newOrder = rota.Paradas
                .OrderBy(p => p.StopOrder)
                .Select(p => new { p.ParadaRotaId, p.StopOrder })
                .ToList();

            await RegistrarLogAsync(rota.RotaId, alteradoPorUsuarioId, TipoMudancaRota.Reordenar, oldOrder, newOrder);

            var rotaAtualizada = await _rotaRepository.BuscarPorIdAsync(rotaId) ?? rota;
            return MapearParaResponse(rotaAtualizada);
        }

        public async Task<RotaReadDto> ConcluirParadaAsync(Guid rotaId, Guid paradaRotaId, Guid alteradoPorUsuarioId, string role)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (role == "Entregador" && rota.AssociadoAoEntregadorId != alteradoPorUsuarioId)
                throw new UnauthorizedAccessException("O entregador nao pode concluir parada de uma rota que nao pertence a ele.");

            if (rota.Status != StatusRota.EmProgresso)
                throw new InvalidOperationException("Somente rotas em progresso podem ter paradas concluidas.");

            var parada = rota.Paradas.FirstOrDefault(p => p.ParadaRotaId == paradaRotaId);

            if (parada is null)
                throw new KeyNotFoundException("Parada da rota nao encontrada.");

            parada.Status = StatusParadaRota.Realizado;
            parada.CompletoEm = DateTime.UtcNow;

            if (parada.Delivery is not null)
            {
                parada.Delivery.Status = StatusDelivery.Entregue;
                parada.Delivery.FinalizadoEm = DateTime.UtcNow;
                parada.Delivery.MotivoFalha = null;
                await _deliveryRepository.AtualizarAsync(parada.Delivery);
            }

            await _rotaRepository.AtualizarAsync(rota);
            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> RegistrarFalhaParadaAsync(Guid rotaId, Guid paradaRotaId, RegistrarFalhaEntregaDto dto, Guid alteradoPorUsuarioId, string role)
        {
            if (string.IsNullOrWhiteSpace(dto.MotivoFalha))
                throw new ArgumentException("O motivo da falha e obrigatorio.", nameof(dto));

            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (role == "Entregador" && rota.AssociadoAoEntregadorId != alteradoPorUsuarioId)
                throw new UnauthorizedAccessException("O entregador nao pode registrar falha de uma rota que nao pertence a ele.");

            if (rota.Status != StatusRota.EmProgresso)
                throw new InvalidOperationException("Somente rotas em progresso podem registrar falha de entrega.");

            var parada = rota.Paradas.FirstOrDefault(p => p.ParadaRotaId == paradaRotaId);

            if (parada is null)
                throw new KeyNotFoundException("Parada da rota nao encontrada.");

            parada.Status = StatusParadaRota.PulouPedido;
            parada.CompletoEm = DateTime.UtcNow;

            if (parada.Delivery is not null)
            {
                parada.Delivery.Status = StatusDelivery.Falhou;
                parada.Delivery.MotivoFalha = dto.MotivoFalha.Trim();
                parada.Delivery.FinalizadoEm = DateTime.UtcNow;
                await _deliveryRepository.AtualizarAsync(parada.Delivery);
            }

            await _rotaRepository.AtualizarAsync(rota);
            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> IniciarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (rota.Status == StatusRota.Finalizada)
                throw new InvalidOperationException("Rotas finalizadas nao podem ser iniciadas novamente.");

            if (rota.Status != StatusRota.Atribuida)
                throw new InvalidOperationException("Somente rotas atribuidas podem ser iniciadas.");

            if (rota.AssociadoAoEntregadorId is null)
                throw new InvalidOperationException("A rota precisa estar atribuida a um entregador antes de iniciar.");

            var oldValue = new { rota.Status, rota.InicioEm };

            rota.Status = StatusRota.EmProgresso;
            rota.InicioEm = DateTime.UtcNow;

            foreach (var parada in rota.Paradas)
            {
                if (parada.Delivery is not null)
                {
                    parada.Delivery.Status = StatusDelivery.EmRota;
                    await _deliveryRepository.AtualizarAsync(parada.Delivery);
                }
            }

            await _rotaRepository.AtualizarAsync(rota);
            await RegistrarLogAsync(rota.RotaId, alteradoPorUsuarioId, TipoMudancaRota.Iniciar, oldValue, new
            {
                rota.Status,
                rota.InicioEm
            });

            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> IniciarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId, string role)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (role == "Entregador" && rota.AssociadoAoEntregadorId != alteradoPorUsuarioId)
                throw new UnauthorizedAccessException("O entregador nao pode iniciar uma rota que nao pertence a ele.");

            return await IniciarRotaAsync(rotaId, alteradoPorUsuarioId);
        }

        public async Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (rota.Status == StatusRota.Finalizada)
                throw new InvalidOperationException("A rota ja esta finalizada.");

            if (rota.Status != StatusRota.EmProgresso)
                throw new InvalidOperationException("Somente rotas em progresso podem ser finalizadas.");

            var oldValue = new { rota.Status, rota.TerminoEm };

            rota.Status = StatusRota.Finalizada;
            rota.TerminoEm = DateTime.UtcNow;

            await _rotaRepository.AtualizarAsync(rota);
            await RegistrarLogAsync(rota.RotaId, alteradoPorUsuarioId, TipoMudancaRota.Finalizar, oldValue, new
            {
                rota.Status,
                rota.TerminoEm
            });

            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId, Guid alteradoPorUsuarioId, string role)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (role == "Entregador" && rota.AssociadoAoEntregadorId != alteradoPorUsuarioId)
                throw new UnauthorizedAccessException("O entregador nao pode finalizar uma rota que nao pertence a ele.");

            return await FinalizarRotaAsync(rotaId, alteradoPorUsuarioId);
        }

        public async Task<IEnumerable<LogMudancaRotaReadDto>> BuscarLogsAsync(Guid rotaId, Guid usuarioId, string role)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (role == "Entregador" && rota.AssociadoAoEntregadorId != usuarioId)
                throw new UnauthorizedAccessException("O entregador nao pode acessar logs de uma rota que nao pertence a ele.");

            var logs = await _logRepository.BuscarPorRotaAsync(rotaId);

            return logs.Select(log => new LogMudancaRotaReadDto
            {
                LogMudancaRotaId = log.LogMudancaRotaId,
                RotaId = log.RotaId,
                AlteradoPeloUsuarioId = log.AlteradoPeloUsuarioId,
                MudouEm = log.MudouEm,
                TipoMudanca = log.TipoMudanca,
                OldValue = log.OldValue,
                NewValue = log.NewValue
            }).ToList();
        }

        private async Task<Rota> ObterRotaEditavelAsync(Guid rotaId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota nao encontrada.");

            if (rota.Status == StatusRota.Finalizada)
                throw new InvalidOperationException("Nao e permitido alterar uma rota finalizada.");

            return rota;
        }

        private async Task RegistrarLogAsync(Guid rotaId, Guid alteradoPorUsuarioId, TipoMudancaRota tipo, object oldValue, object newValue)
        {
            await _logRepository.AdicionarAsync(new LogMudancaRota
            {
                RotaId = rotaId,
                AlteradoPeloUsuarioId = alteradoPorUsuarioId,
                TipoMudanca = tipo,
                OldValue = JsonSerializer.Serialize(oldValue),
                NewValue = JsonSerializer.Serialize(newValue)
            });
        }

        private static RotaReadDto MapearParaResponse(Rota rota)
        {
            return new RotaReadDto
            {
                RotaId = rota.RotaId,
                CriadoPeloUsuarioId = rota.CriadoPeloUsuarioId,
                AssociadoAoEntregadorId = rota.AssociadoAoEntregadorId,
                Status = rota.Status,
                CriadoEm = rota.CriadoEm,
                AtribuidoEm = rota.AtribuidoEm,
                InicioEm = rota.InicioEm,
                TerminoEm = rota.TerminoEm,
                Paradas = rota.Paradas
                    .OrderBy(p => p.StopOrder)
                    .Select(parada => new RotaParadaReadDto
                    {
                        ParadaRotaId = parada.ParadaRotaId,
                        DeliveryId = parada.DeliveryId,
                        StopOrder = parada.StopOrder,
                        Status = parada.Status,
                        CompletoEm = parada.CompletoEm,
                        ClienteNome = parada.Delivery?.Pedido?.Cliente?.Nome ?? string.Empty,
                        EnderecoResumo = $"{parada.Delivery?.ClienteEndereco?.Logradouro}, {parada.Delivery?.ClienteEndereco?.Numero} - {parada.Delivery?.ClienteEndereco?.Bairro}"
                    })
                    .ToList()
            };
        }
    }
}
