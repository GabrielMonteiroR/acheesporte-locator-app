using acheesporte_locator_app.Dtos.AvailabilityTimes;

namespace acheesporte_locator_app.Interfaces;

public interface IAvailableTimesService
{
    Task<List<VenueAvailabilityTimeDto>> GetAvailableTimesByVenueIdAsync(int venueId, bool? isReserved = null);
    Task<VenueAvailabilityTimeDto> CreateAvailabilityTimeAsync(CreateVenueAvailabilityTimeDto dto);
    Task<bool> DeleteAvailabilityTimeAsync(int id);
    Task<VenueAvailabilityTimeDto> UpdateAvailabilityTimeAsync(int id, UpdateVenueAvailabilityTimeDto dto);

}