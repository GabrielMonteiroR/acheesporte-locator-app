using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos.VenueTypeDtos;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services;

public class VenueTypeService
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
            var response = await _httpClient.GetFromJsonAsync<VenueTypeListResponse>($"{_apiSettings.BaseUrl}{_apiSettings.VenueTypeEndpoint}");
            return response?.VenueTypesList ?? new();
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"An error occurred while fetching venue types: {ex.Message}");
            return new List<VenueTypeResponseDto>();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"An unexpected error occurred: {ex.Message}");
            return new List<VenueTypeResponseDto>();
        }
    }
}
