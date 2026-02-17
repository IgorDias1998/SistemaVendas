using System.Text.Json;
using SistemaVendas.Application.DTOs;
using SistemaVendas.Application.Interfaces;

namespace SistemaVendas.Infrastructure.Integration.Cep
{
    public class ViaCepService : ICepService
    {
        private readonly HttpClient _httpClient;

        public ViaCepService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CepResultDto> BuscarCepAsync(string cep)
        {
            var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cep}/json/");

            if (!response.IsSuccessStatusCode)
                throw new Exception($"Erro ao consultar ViaCEP: {response.StatusCode}");

            var json = await response.Content.ReadAsStringAsync();

            var options = new System.Text.Json.JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var data = JsonSerializer.Deserialize<BuscarCepResponse>(json, options);

            if (data is null || data.Erro)
                throw new Exception("Cep não encontrado.");

            return new CepResultDto
            {
                Cep = data.Cep,
                Logradouro = data.Logradouro,
                Bairro = data.Bairro,
                Cidade = data.Cidade,
                Estado = data.Estado
            };
        }
    }
}
