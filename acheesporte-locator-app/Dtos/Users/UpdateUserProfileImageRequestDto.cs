using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.Users;

public class UpdateUserProfileImageRequestDto
{
    [JsonPropertyName("imageUrl")]
    public string ImageUrl { get; set; } = string.Empty;
}
