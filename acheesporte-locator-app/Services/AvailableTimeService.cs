using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.AvailabilityTimes;
using acheesporte_locator_app.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace acheesporte_locator_app.Services;

public class AvailableTimesService : IAvailableTimesService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public AvailableTimesService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<List<VenueAvailabilityTimeDto>> GetAvailableTimesByVenueIdAsync(int venueId, bool? isReserved = null)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Token não encontrado.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetAvailableTimesByVenueIdEndpoint}{venueId}";

        if (isReserved.HasValue)
            url += $"?isReserved={isReserved.Value.ToString().ToLower()}";

        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao buscar horários: {error}");
        }

        var dto = await response.Content.ReadFromJsonAsync<VenueAvailabilityTimeResponseDto>();
        return dto?.Data ?? new();
    }

    public async Task<VenueAvailabilityTimeDto> CreateAvailabilityTimeAsync(CreateVenueAvailabilityTimeDto dto)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Token não encontrado.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.CreateAvailableTimesEndpoint}";

        var response = await _httpClient.PostAsync(url, content);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao criar horário: {error}");
        }

        var responseContent = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<VenueAvailabilityTimeDto>(responseContent, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        return result!;
    }

    public async Task<bool> DeleteAvailabilityTimeAsync(int id)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Token não encontrado.");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.DeleteAvailableTimesEndpoint}{id}";
        var response = await _httpClient.DeleteAsync(url);

        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao excluir horário: {error}");
        }

        return true;
    }

    public async Task<VenueAvailabilityTimeDto> UpdateAvailabilityTimeAsync(int id,
                                          UpdateVenueAvailabilityTimeDto dto)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Token não encontrado.");

        _httpClient.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", token);

        var json = JsonSerializer.Serialize(dto);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.UpdateAvailableTimesEndpoint}{id}";

        var resp = await _httpClient.PutAsync(url, content);

        if (!resp.IsSuccessStatusCode)
        {
            var err = await resp.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao atualizar horário: {err}");
        }

        return await resp.Content.ReadFromJsonAsync<VenueAvailabilityTimeDto>()
               ?? throw new Exception("Resposta inválida do servidor.");
    }

    public async Task<VenueAvailabilityTimeDto> GetAvailabilityTimeByIdAsync(int id)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new Exception("Token não encontrado.");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetAvailableTimesByIdEndpoint}{id}";
        var response = await _httpClient.GetAsync(url);
        if (!response.IsSuccessStatusCode)
        {
            var error = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao buscar horário: {error}");
        }
        return await response.Content.ReadFromJsonAsync<VenueAvailabilityTimeDto>()
               ?? throw new Exception("Resposta inválida do servidor.");
    }
}

