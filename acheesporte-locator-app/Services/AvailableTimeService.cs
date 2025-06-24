using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.Availability;
using acheesporte_locator_app.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services
{
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
    }
}
