using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.Users;

public class UpdateUserProfileImageRequestDto
{
    [JsonPropertyName("image_url")]
    public string ImageUrl { get; set; } = string.Empty;
}
