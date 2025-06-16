using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.VenueDtos;

public class VenueListResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public List<VenueResponseDto> Data { get; set; }
}
