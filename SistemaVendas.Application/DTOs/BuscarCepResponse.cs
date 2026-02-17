using System.Text.Json.Serialization;

namespace SistemaVendas.Application.DTOs
{
    public class BuscarCepResponse
    {
        [JsonPropertyName("cep")]
        public string Cep { get; set; } = string.Empty;

        [JsonPropertyName("logradouro")]
        public string Logradouro { get; set; } = string.Empty;

        [JsonPropertyName("bairro")]
        public string Bairro { get; set; } = string.Empty;

        [JsonPropertyName("localidade")]
        public string Cidade { get; set; } = string.Empty;


        [JsonPropertyName("uf")]
        public string Estado { get; set; } = string.Empty;

        [JsonPropertyName("erro")]
        public bool Erro { get; set; }
    }
}
