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

        public RotaService(
            IRotaRepository rotaRepository,
            IDeliveryRepository deliveryRepository,
            IUsuarioRepository usuarioRepository)
        {
            _rotaRepository = rotaRepository;
            _deliveryRepository = deliveryRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<RotaReadDto> CriarRotaAsync(RotaCriarDto dto)
        {
            if (dto.DeliveryIds is null || dto.DeliveryIds.Count == 0)
                throw new ArgumentException("A rota deve possuir ao menos uma delivery.", nameof(dto));

            if (await _rotaRepository.AlgumaDeliveryEmRotaAtivaAsync(dto.DeliveryIds))
                throw new InvalidOperationException("Uma ou mais deliveries já estão vinculadas a uma rota ativa.");

            var deliveries = (await _deliveryRepository.BuscarPorIdsAsync(dto.DeliveryIds)).ToList();

            if (deliveries.Count != dto.DeliveryIds.Count)
                throw new KeyNotFoundException("Uma ou mais deliveries não foram encontradas.");

            if (deliveries.Any(d => d.Status != StatusDelivery.Pendente && d.Status != StatusDelivery.Associado))
                throw new InvalidOperationException("Somente deliveries pendentes podem entrar em uma rota.");

            var rota = new Rota
            {
                CriadoPeloUsuarioId = dto.CriadoPeloUsuarioId,
                Status = StatusRota.Rascunho,
                Paradas = dto.DeliveryIds.Select((deliveryId, index) => new ParadaRota
                {
                    DeliveryId = deliveryId,
                    StopOrder = index + 1,
                    Status = StatusParadaRota.Pendente
                }).ToList()
            };

            var rotaSalva = await _rotaRepository.AdicionarAsync(rota);
            var rotaCompleta = await _rotaRepository.BuscarPorIdAsync(rotaSalva.RotaId) ?? rotaSalva;

            return MapearParaResponse(rotaCompleta);
        }

        public async Task<IEnumerable<RotaReadDto>> BuscarRotasAsync()
        {
            var rotas = await _rotaRepository.BuscarTodosAsync();
            return rotas.Select(MapearParaResponse).ToList();
        }

        public async Task<RotaReadDto> BuscarRotaPorIdAsync(Guid rotaId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota não encontrada.");

            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> AtribuirEntregadorAsync(Guid rotaId, Guid entregadorId)
        {
            var rota = await ObterRotaEditavelAsync(rotaId);
            var entregador = await _usuarioRepository.BuscarPorIdAsync(entregadorId);

            if (entregador is null)
                throw new KeyNotFoundException("Entregador não encontrado.");

            if (entregador.Role != UserRole.Entregador)
                throw new InvalidOperationException("O usuário informado não possui papel de entregador.");

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
            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> ReordenarParadasAsync(Guid rotaId, RotaReordenarParadasDto dto)
        {
            var rota = await ObterRotaEditavelAsync(rotaId);

            var paradaIdsAtuais = rota.Paradas.Select(p => p.ParadaRotaId).OrderBy(id => id).ToList();
            var paradaIdsRecebidos = dto.ParadaIdsEmOrdem.OrderBy(id => id).ToList();

            if (!paradaIdsAtuais.SequenceEqual(paradaIdsRecebidos))
                throw new InvalidOperationException("A lista de paradas enviada não corresponde às paradas da rota.");

            for (var i = 0; i < dto.ParadaIdsEmOrdem.Count; i++)
            {
                var parada = rota.Paradas.First(p => p.ParadaRotaId == dto.ParadaIdsEmOrdem[i]);
                parada.StopOrder = i + 1;
            }

            await _rotaRepository.AtualizarAsync(rota);
            var rotaAtualizada = await _rotaRepository.BuscarPorIdAsync(rotaId) ?? rota;
            return MapearParaResponse(rotaAtualizada);
        }

        public async Task<RotaReadDto> IniciarRotaAsync(Guid rotaId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota não encontrada.");

            if (rota.Status == StatusRota.Finalizada)
                throw new InvalidOperationException("Rotas finalizadas não podem ser iniciadas novamente.");

            if (rota.Status != StatusRota.Atribuida)
                throw new InvalidOperationException("Somente rotas atribuídas podem ser iniciadas.");

            if (rota.AssociadoAoEntregadorId is null)
                throw new InvalidOperationException("A rota precisa estar atribuída a um entregador antes de iniciar.");

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
            return MapearParaResponse(rota);
        }

        public async Task<RotaReadDto> FinalizarRotaAsync(Guid rotaId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota não encontrada.");

            if (rota.Status == StatusRota.Finalizada)
                throw new InvalidOperationException("A rota já está finalizada.");

            if (rota.Status != StatusRota.EmProgresso)
                throw new InvalidOperationException("Somente rotas em progresso podem ser finalizadas.");

            rota.Status = StatusRota.Finalizada;
            rota.TerminoEm = DateTime.UtcNow;

            await _rotaRepository.AtualizarAsync(rota);
            return MapearParaResponse(rota);
        }

        private async Task<Rota> ObterRotaEditavelAsync(Guid rotaId)
        {
            var rota = await _rotaRepository.BuscarPorIdAsync(rotaId);

            if (rota is null)
                throw new KeyNotFoundException("Rota não encontrada.");

            if (rota.Status == StatusRota.Finalizada)
                throw new InvalidOperationException("Não é permitido alterar uma rota finalizada.");

            return rota;
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
                        ClienteNome = parada.Delivery?.Pedido?.Cliente?.Nome ?? string.Empty,
                        EnderecoResumo = $"{parada.Delivery?.ClienteEndereco?.Logradouro}, {parada.Delivery?.ClienteEndereco?.Numero} - {parada.Delivery?.ClienteEndereco?.Bairro}"
                    })
                    .ToList()
            };
        }
    }
}
