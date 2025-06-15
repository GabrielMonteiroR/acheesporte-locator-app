using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos;
using Microsoft.Maui.Storage;
using System.Net.Http.Headers;
using System.Net.Http.Json;

public class VenueService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public VenueService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<bool> CreateVenueAsync(CreateVenueRequestDto request)
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrEmpty(token))
                throw new Exception("Usuário não autenticado. Token não encontrado.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.PostAsJsonAsync(
                $"{_apiSettings.BaseUrl}/{_apiSettings.VenuesEndpoint}", request);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
            {
                var errorMessage = await response.Content.ReadAsStringAsync();
                throw new Exception($"Falha ao criar o local: {errorMessage}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Erro ao criar o local.", ex);
        }
    }
}
