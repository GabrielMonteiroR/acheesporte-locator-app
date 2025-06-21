namespace acheesporte_locator_app.Dtos.VenueTypeDtos;

public class VenueTypeListResponse
{
    public string Message { get; set; } = string.Empty;
    public List<VenueTypeResponseDto> VenueTypesList { get; set; } = new();
}
