using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.AvailabilityTimes;

public class VenueAvailabilityTimeResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("data")]
    public List<VenueAvailabilityTimeDto> Data { get; set; }
}
