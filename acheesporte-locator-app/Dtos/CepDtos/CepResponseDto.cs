using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.CepDtos;

public class CepResponseDto
{
    [JsonPropertyName("logradouro")]
    public string Street { get; set; }

    [JsonPropertyName("bairro")]
    public string Neighborhood { get; set; }

    [JsonPropertyName("localidade")]
    public string City { get; set; }

    [JsonPropertyName("uf")]
    public string State { get; set; }

    [JsonPropertyName("cep")]
    public string PostalCode { get; set; }
}
