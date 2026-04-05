using FluentValidation;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;
using SistemaVendas.Domain.Entities;

namespace SistemaVendas.Application.Services
{
    public class DeliveryService : IDeliveryService
    {
        private readonly IDeliveryRepository _deliveryRepository;
        private readonly IValidator<DeliveryAtualizarStatusDto> _statusValidator;

        public DeliveryService(
            IDeliveryRepository deliveryRepository,
            IValidator<DeliveryAtualizarStatusDto> statusValidator)
        {
            _deliveryRepository = deliveryRepository;
            _statusValidator = statusValidator;
        }

        public async Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesAsync()
        {
            var deliveries = await _deliveryRepository.BuscarTodosAsync();
            return deliveries.Select(MapearParaResponse).ToList();
        }

        public async Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesAsync(Guid usuarioId, string role)
        {
            var deliveries = role == "Entregador"
                ? await _deliveryRepository.BuscarPorEntregadorIdAsync(usuarioId)
                : await _deliveryRepository.BuscarTodosAsync();

            return deliveries.Select(MapearParaResponse).ToList();
        }

        public async Task<PagedResultDto<DeliveryReadDto>> BuscarDeliveriesAsync(Guid usuarioId, string role, DeliveryListQueryDto query)
        {
            var deliveries = await BuscarDeliveriesAsync(usuarioId, role);

            var filtrados = deliveries.AsEnumerable();

            if (query.PedidoId.HasValue)
                filtrados = filtrados.Where(d => d.PedidoId == query.PedidoId.Value);

            if (query.Status.HasValue)
                filtrados = filtrados.Where(d => d.Status == query.Status.Value);

            filtrados = filtrados.OrderByDescending(d => d.CriadoEm).ToList();

            return PaginacaoHelper.AplicarPaginacao(filtrados, query);
        }

        public async Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesPendentesAsync()
        {
            var deliveries = await _deliveryRepository.BuscarPendentesAsync();
            return deliveries.Select(MapearParaResponse).ToList();
        }

        public async Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesPendentesAsync(Guid usuarioId, string role)
        {
            var deliveries = role == "Entregador"
                ? await _deliveryRepository.BuscarPendentesPorEntregadorIdAsync(usuarioId)
                : await _deliveryRepository.BuscarPendentesAsync();

            return deliveries.Select(MapearParaResponse).ToList();
        }

        public async Task<DeliveryReadDto> BuscarDeliveryPorIdAsync(Guid deliveryId)
        {
            var delivery = await _deliveryRepository.BuscarPorIdAsync(deliveryId);

            if (delivery is null)
                throw new KeyNotFoundException("Delivery nao encontrado.");

            return MapearParaResponse(delivery);
        }

        public async Task<DeliveryReadDto> BuscarDeliveryPorIdAsync(Guid deliveryId, Guid usuarioId, string role)
        {
            var delivery = await _deliveryRepository.BuscarPorIdAsync(deliveryId);

            if (delivery is null)
                throw new KeyNotFoundException("Delivery nao encontrado.");

            if (role == "Entregador" && !await _deliveryRepository.PertenceAoEntregadorAsync(deliveryId, usuarioId))
                throw new UnauthorizedAccessException("O entregador nao pode acessar uma delivery que nao pertence a ele.");

            return MapearParaResponse(delivery);
        }

        public async Task<DeliveryReadDto> AtualizarStatusAsync(Guid deliveryId, DeliveryAtualizarStatusDto dto)
        {
            await _statusValidator.ValidateAndThrowAsync(dto);

            var delivery = await _deliveryRepository.BuscarPorIdAsync(deliveryId);

            if (delivery is null)
                throw new KeyNotFoundException("Delivery nao encontrado.");

            delivery.Status = dto.Status;
            await _deliveryRepository.AtualizarAsync(delivery);

            return MapearParaResponse(delivery);
        }

        public async Task<DeliveryReadDto> AtualizarStatusAsync(Guid deliveryId, DeliveryAtualizarStatusDto dto, Guid usuarioId, string role)
        {
            await _statusValidator.ValidateAndThrowAsync(dto);

            if (role == "Entregador" && !await _deliveryRepository.PertenceAoEntregadorAsync(deliveryId, usuarioId))
                throw new UnauthorizedAccessException("O entregador nao pode alterar uma delivery que nao pertence a ele.");

            return await AtualizarStatusAsync(deliveryId, dto);
        }

        public async Task<DeliveryReadDto> RegistrarFalhaAsync(Guid deliveryId, RegistrarFalhaEntregaDto dto, Guid usuarioId, string role)
        {
            if (string.IsNullOrWhiteSpace(dto.MotivoFalha))
                throw new ArgumentException("O motivo da falha e obrigatorio.", nameof(dto));

            if (role == "Entregador" && !await _deliveryRepository.PertenceAoEntregadorAsync(deliveryId, usuarioId))
                throw new UnauthorizedAccessException("O entregador nao pode alterar uma delivery que nao pertence a ele.");

            var delivery = await _deliveryRepository.BuscarPorIdAsync(deliveryId);

            if (delivery is null)
                throw new KeyNotFoundException("Delivery nao encontrado.");

            delivery.Status = StatusDelivery.Falhou;
            delivery.MotivoFalha = dto.MotivoFalha.Trim();
            delivery.FinalizadoEm = DateTime.UtcNow;

            await _deliveryRepository.AtualizarAsync(delivery);

            return MapearParaResponse(delivery);
        }

        private static DeliveryReadDto MapearParaResponse(Delivery delivery)
        {
            return new DeliveryReadDto
            {
                DeliveryId = delivery.DeliveryId,
                PedidoId = delivery.PedidoId,
                ClienteEnderecoId = delivery.ClienteEnderecoId,
                Status = delivery.Status,
                CriadoEm = delivery.CriadoEm,
                FinalizadoEm = delivery.FinalizadoEm,
                MotivoFalha = delivery.MotivoFalha,
                ClienteNome = delivery.Pedido?.Cliente?.Nome ?? string.Empty,
                Logradouro = delivery.ClienteEndereco?.Logradouro ?? string.Empty,
                Numero = delivery.ClienteEndereco?.Numero ?? string.Empty,
                Bairro = delivery.ClienteEndereco?.Bairro ?? string.Empty,
                Cidade = delivery.ClienteEndereco?.Cidade ?? string.Empty,
                Estado = delivery.ClienteEndereco?.Estado ?? string.Empty,
                Cep = delivery.ClienteEndereco?.Cep ?? string.Empty
            };
        }
    }
}
