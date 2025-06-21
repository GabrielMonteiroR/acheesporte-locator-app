using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.VenueDtos;
using acheesporte_locator_app.Helpers;
using acheesporte_locator_app.Interfaces;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services;

public class VenueService : IVenueService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;
    private readonly IUserInterface _userService;

    public VenueService(HttpClient httpClient, ApiSettings apiSettings, IUserInterface userService)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
        _userService = userService;
    }

public async Task<List<VenueResponseDto>> GetVenuesByOwnerAsync()
{
    try
    {
        var ownerId = UserSession.CurrentUser?.Id;

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

            var url = $"{_apiSettings.BaseUrl}/{_apiSettings.VenuesEndpoint}"; 

            var response = await _httpClient.PostAsJsonAsync(url, dto);

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


}
