using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace acheesporte_locator_app.Config;

public class ApiSettings
{
    [JsonPropertyName("BaseUrl")]
    public string BaseUrl { get; set; }

    [JsonPropertyName("SignInEndpoint")]
    public string SignInEndpoint { get; set; }

    [JsonPropertyName("SignUpEndpoint")]
    public string SignUpEndpoint { get; set; }

    [JsonPropertyName("ImageUploadEndpoint")]
    public string ImageUploadEndpoint { get; set; }

    [JsonPropertyName("CurrentUserEndpoint")]
    public string CurrentUserEndpoint { get; set; }

    [JsonPropertyName("PlacesApiKey")]
    public string PlacesApiKey { get; set; }

    public string VenuesEndpoint { get; set; }

    public string UploadVenueImagesEndpoint { get; set; }

    public string VenueTypeEndpoint { get; set; }

    public string CreateVenuesEndpoint { get; set; }

    public string UpdateVenueEndpoint { get; set; }

    public string GetVenueByIdEndpoint { get; set; }

    public string GetAvailableTimesByVenueIdEndpoint { get; set; }

    public string CreateAvailableTimesEndpoint { get; set; }
}
