using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.AvailabilityTimes;

public class UpdateVenueAvailabilityTimeDto
{
    [JsonPropertyName("start_date")]
    public DateTime? StartDate { get; set; }

    [JsonPropertyName("end_date")]
    public DateTime? EndDate { get; set; }

    [JsonPropertyName("price")]
    public decimal? Price { get; set; }
}
