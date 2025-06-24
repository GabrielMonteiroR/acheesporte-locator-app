using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.Availability
{
    public class VenueAvailabilityTimeResponseDto
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public List<VenueAvailabilityTimeDto> Data { get; set; }
    }
}
