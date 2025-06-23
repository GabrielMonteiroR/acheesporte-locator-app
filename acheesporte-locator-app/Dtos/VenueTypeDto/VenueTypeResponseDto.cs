using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.VenueTypeDtos;

public class VenueTypeResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("description")]
    public string Description { get; set; } = string.Empty;
}
