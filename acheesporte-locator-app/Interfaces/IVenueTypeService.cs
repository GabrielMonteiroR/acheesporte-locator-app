using acheesporte_locator_app.Dtos.VenueTypeDtos;

namespace acheesporte_locator_app.Interfaces;

public interface IVenueTypeService
{
    Task<List<VenueTypeResponseDto>> GetVenueTypesAsync();
}
