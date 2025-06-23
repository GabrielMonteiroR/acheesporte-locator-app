using acheesporte_athlete_app.Dtos.GooglePlaces;
using acheesporte_locator_app.Config;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

public class GooglePlacesService
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public GooglePlacesService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<List<Prediction>> GetAutocompleteSuggestionsAsync(string input)
    {
        var encoded = HttpUtility.UrlEncode(input);
        var url = $"https://maps.googleapis.com/maps/api/place/autocomplete/json?input={encoded}&key={_apiSettings.PlacesApiKey}&language=pt-BR&components=country:br";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<AutocompleteResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Predictions ?? new List<Prediction>();
    }

    public async Task<PlaceLocation?> GetPlaceLocationAsync(string placeId)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={_apiSettings.PlacesApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<PlaceDetailsResponse>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        return result?.Result?.Geometry?.Location;
    }

    public async Task<(double Latitude, double Longitude)> GetPlaceCoordinatesAsync(string placeId)
    {
        var url = $"https://maps.googleapis.com/maps/api/place/details/json?place_id={placeId}&key={_apiSettings.PlacesApiKey}";

        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var json = await response.Content.ReadAsStringAsync();
        var data = JsonSerializer.Deserialize<PlaceDetailsResponse>(json);

        var location = data?.Result?.Geometry?.Location;
        return location is null ? (0, 0) : (location.Lat, location.Lng);
    }
}
