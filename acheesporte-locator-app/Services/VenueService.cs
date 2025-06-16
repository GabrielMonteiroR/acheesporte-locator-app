using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos;
using Microsoft.Maui.Storage;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services;

public class VenueService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public VenueService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<List<VenueResponseDto>> GetAllVenuesAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrWhiteSpace(token))
                throw new Exception("Usuário não autenticado. Token não encontrado.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.GetAsync($"{_apiSettings.BaseUrl}/{_apiSettings.VenuesEndpoint}");

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao buscar locais: {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<VenueListResponseDto>();
            return result?.Data ?? new List<VenueResponseDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            throw;
        }
    }
}
