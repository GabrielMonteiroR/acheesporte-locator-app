using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.Reservations;

public class ReservationUserDto
{
    [JsonPropertyName("first_name")]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("last_name")]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("profile_image_url")]
    public string ProfileImageUrl { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    public string Phone { get; set; } = string.Empty;

    /// <summary>Nome completo pronto para binding.</summary>
    public string FullName => $"{FirstName.Trim()} {LastName.Trim()}".Trim();
}
