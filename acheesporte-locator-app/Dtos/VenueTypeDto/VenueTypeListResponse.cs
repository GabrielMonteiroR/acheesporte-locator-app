using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.VenueTypeDtos;

public class VenueTypeListResponse
{
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;

    [JsonPropertyName("venueTypesList")]
    public List<VenueTypeResponseDto> VenueTypesList { get; set; } = new();
}
