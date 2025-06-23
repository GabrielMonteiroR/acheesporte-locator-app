using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.VenueTypeDtos;
using acheesporte_locator_app.Interfaces;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services;

public class VenueTypeService : IVenueTypeService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public VenueTypeService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<List<VenueTypeResponseDto>> GetVenueTypesAsync()
    {
        try
        {
            var token = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrWhiteSpace(token))
            {
                await Shell.Current.DisplayAlert("Erro", "Token de autenticação não encontrado.", "OK");
                return new();
            }

            var request = new HttpRequestMessage(HttpMethod.Get, $"{_apiSettings.BaseUrl}{_apiSettings.VenueTypeEndpoint}");
            request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                await Shell.Current.DisplayAlert("Erro", $"Erro ao buscar tipos: {response.StatusCode}\n{content}", "OK");
                return new();
            }

            var result = await response.Content.ReadFromJsonAsync<VenueTypeListResponse>();
            return result?.VenueTypesList ?? new();
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Erro", $"Erro inesperado: {ex.Message}", "OK");
            return new();
        }
    }
}
