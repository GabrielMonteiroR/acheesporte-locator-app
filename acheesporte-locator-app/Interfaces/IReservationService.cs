using acheesporte_locator_app.Dtos.Reservations;

namespace acheesporte_locator_app.Interfaces;

public interface IReservationService
{
    Task<List<ReservationResponseDto>> GetReservationsByVenueIdAsync(int venueId);
}
