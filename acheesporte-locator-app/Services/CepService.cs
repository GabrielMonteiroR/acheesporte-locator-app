using acheesporte_locator_app.Dtos.CepDtos;
using acheesporte_locator_app.Interfaces;
using System.Net.Http;
using System.Text.Json;

namespace acheesporte_locator_app.Services;

public class CepService : ICepService
{
    private readonly HttpClient _httpClient;

    public CepService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<CepResponseDto> GetAddressByCepAsync(string cep)
    {
        var cleanedCep = cep?.Replace("-", "").Trim();
        if (string.IsNullOrEmpty(cleanedCep) || cleanedCep.Length != 8)
            throw new ArgumentException("CEP inválido");

        var response = await _httpClient.GetAsync($"https://viacep.com.br/ws/{cleanedCep}/json/");
        if (!response.IsSuccessStatusCode)
            throw new Exception("Erro ao consultar o CEP");

        var json = await response.Content.ReadAsStringAsync();
        var dto = JsonSerializer.Deserialize<CepResponseDto>(json);

        if (dto == null || string.IsNullOrWhiteSpace(dto.Street))
            throw new Exception("CEP não encontrado ou incompleto");

        return dto;
    }
}
