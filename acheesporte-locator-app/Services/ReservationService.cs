using System.Net.Http.Headers;
using System.Net.Http.Json;
using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.Reservations;
using acheesporte_locator_app.Interfaces;

namespace acheesporte_locator_app.Services;

public class ReservationService : IReservationService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public ReservationService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<List<ReservationResponseDto>> GetReservationsByVenueIdAsync(int venueId)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedAccessException("Token não encontrado. Faça login novamente.");

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetReservationsByVenueIdEndpoint}{venueId}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao buscar reservas: {content}");
        }

        var dto = await response.Content.ReadFromJsonAsync<ReservationsResponseDto>();
        return dto?.Reservations ?? new List<ReservationResponseDto>();
    }

    public async Task<List<ReservationResponseDto>> GetReservationHistoryByVenueIdAsync(int venueId)
    {
        var token = await SecureStorage.GetAsync("auth_token");
        if (string.IsNullOrWhiteSpace(token))
            throw new UnauthorizedAccessException("Token não encontrado. Faça login novamente.");

        var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetHistoryByVenueIdEndpoint}{venueId}";

        var request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await _httpClient.SendAsync(request);
        if (!response.IsSuccessStatusCode)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Erro ao buscar histórico: {content}");
        }

        var dto = await response.Content.ReadFromJsonAsync<ReservationsResponseDto>();
        return dto?.Reservations ?? new List<ReservationResponseDto>();
    }

}
