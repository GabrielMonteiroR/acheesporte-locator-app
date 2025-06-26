using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.Reservations;

public class ReservationResponseDto
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("userId")]
    public int UserId { get; set; }

    [JsonPropertyName("venueId")]
    public int VenueId { get; set; }

    [JsonPropertyName("venueAvailabilityTimeId")]
    public int VenueAvailabilityTimeId { get; set; }

    [JsonPropertyName("venueAvailabilityTime")]
    public VenueAvailabilityTimeDto VenueAvailabilityTime { get; set; } = null!;

    [JsonPropertyName("user")]
    public ReservationUserDto User { get; set; } = null!;

    [JsonPropertyName("paymentMethodId")]
    public int PaymentMethodId { get; set; }

    [JsonPropertyName("isPaid")]
    public bool IsPaid { get; set; }
}
