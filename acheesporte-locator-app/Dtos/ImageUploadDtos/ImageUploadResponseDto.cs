using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos.ImageUploadDtos;

public class ImageUploadResponseDto
{
    [JsonPropertyName("image")]
    public string Image { get; set; }
}
