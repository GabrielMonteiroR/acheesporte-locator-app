using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.VenueDtos;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Interfaces;
using CommunityToolkit.Maui.Core.Views;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

namespace acheesporte_locator_app.Services;

public class VenueService : IVenueService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IUserService _userService;

    public VenueService(HttpClient httpClient, ApiSettings apiSettings, IUserService userService)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
        _userService = userService;
    }

    public async Task<List<VenueResponseDto>> GetVenuesByOwnerAsync()
    {
        try
        {
            var ownerId = UserSession.CurrentUser.Id;

            if (ownerId == null)
                throw new Exception("Usuário não encontrado na sessão.");

            var authToken = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrWhiteSpace(authToken))
                throw new Exception("Token não encontrado.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var endpoint = $"{_apiSettings.BaseUrl}{_apiSettings.VenuesEndpoint}{ownerId}";
            var response = await _httpClient.GetAsync(endpoint);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao buscar locais do dono: {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<VenueListResponseDto>();
            return result?.Data ?? new List<VenueResponseDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro: {ex.Message}");
            return new List<VenueResponseDto>();
        }
    }
    public async Task CreateVenueAsync(CreateVenueRequestDto dto)
    {
        try
        {
            var authToken = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrWhiteSpace(authToken))
                throw new Exception("Token não encontrado.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var url = $"{_apiSettings.BaseUrl}{_apiSettings.CreateVenuesEndpoint}";
            await Shell.Current.DisplayAlert("Endpoint URL", $"The URL is: {url}", "OK");

            var json = JsonSerializer.Serialize(dto);
            await Shell.Current.DisplayAlert("JSON Enviado", json, "OK");

            var response = await _httpClient.PostAsJsonAsync(url, dto);

            var responseContent = await response.Content.ReadAsStringAsync();

            await Shell.Current.DisplayAlert("Response",
                $"Status: {(int)response.StatusCode}\n\n{responseContent}",
                "OK");


            if (!response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao cadastrar local: {content}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao criar local: {ex.Message}");
            throw;
        }
    }

    public async Task UpdateVenueAsync(int venueId, UpdateVenueRequestDto dto)
    {
        try
        {
            var authToken = await SecureStorage.GetAsync("auth_token");

            if (string.IsNullOrWhiteSpace(authToken))
                throw new Exception("Token não encontrado.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var url = $"{_apiSettings.BaseUrl}{_apiSettings.UpdateVenueEndpoint}{venueId}";

            var response = await _httpClient.PutAsJsonAsync(url, dto);
            var responseContent = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                throw new Exception($"Erro ao atualizar local: {responseContent}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao atualizar local: {ex.Message}");
            throw;
        }
    }

    public async Task<VenueResponseDto> GetVenueByIdAsync(int venueId)
    {
        try
        {
            var authToken = await SecureStorage.GetAsync("auth_token");
            if (string.IsNullOrWhiteSpace(authToken))
                throw new Exception("Token não encontrado.");

            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var url = $"{_apiSettings.BaseUrl}{_apiSettings.GetVenueByIdEndpoint}{venueId}";
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var error = await response.Content.ReadAsStringAsync();
                throw new Exception($"Erro ao buscar local: {error}");
            }

            var result = await response.Content.ReadFromJsonAsync<VenueResponseDto>();

            return result;

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao buscar local por ID: {ex.Message}");
            throw;
        }
    }
}
