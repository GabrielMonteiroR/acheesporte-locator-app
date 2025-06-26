using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.Reservations;

public class ReservationsResponseDto
{
    [JsonPropertyName("message")]
    public string Message { get; set; }

    [JsonPropertyName("reservations")]
    public List<ReservationResponseDto> Reservations { get; set; }
}
