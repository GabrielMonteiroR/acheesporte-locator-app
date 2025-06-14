using System.Text.Json.Serialization;

namespace acheesporte_locator_app.Dtos;

public class SignUpRequestDto
{
    [JsonPropertyName("firstName")]
    [JsonRequired]
    public string FirstName { get; set; } = string.Empty;

    [JsonPropertyName("lastName")]
    [JsonRequired]
    public string LastName { get; set; } = string.Empty;

    [JsonPropertyName("email")]
    [JsonRequired]
    public string Email { get; set; } = string.Empty;

    [JsonPropertyName("password")]
    [JsonRequired]
    public string Password { get; set; } = string.Empty;

    [JsonPropertyName("phone")]
    [JsonRequired]
    public string Phone { get; set; } = string.Empty;

    [JsonPropertyName("cpf")]
    [JsonRequired]
    public string Cpf { get; set; } = string.Empty;

    [JsonPropertyName("image")]
    public string? ProfileImageUrl { get; set; }
}
