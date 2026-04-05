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

        public async Task<IEnumerable<DeliveryReadDto>> BuscarDeliveriesPendentesAsync()
        {
            var deliveries = await _deliveryRepository.BuscarPendentesAsync();
            return deliveries.Select(MapearParaResponse).ToList();
        }

        public async Task<DeliveryReadDto> BuscarDeliveryPorIdAsync(Guid deliveryId)
        {
            var delivery = await _deliveryRepository.BuscarPorIdAsync(deliveryId);

            if (delivery is null)
                throw new KeyNotFoundException("Delivery nao encontrado.");

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

        private static DeliveryReadDto MapearParaResponse(Delivery delivery)
        {
            return new DeliveryReadDto
            {
                DeliveryId = delivery.DeliveryId,
                PedidoId = delivery.PedidoId,
                ClienteEnderecoId = delivery.ClienteEnderecoId,
                Status = delivery.Status,
                CriadoEm = delivery.CriadoEm,
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
