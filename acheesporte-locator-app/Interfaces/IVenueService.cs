﻿using acheesporte_locator_app.Dtos.VenueDtos;

namespace acheesporte_locator_app.Interfaces;

public interface IVenueService
{
    Task<List<VenueResponseDto>> GetVenuesByOwnerAsync();
    Task CreateVenueAsync(CreateVenueRequestDto dto);
    Task UpdateVenueAsync(int venueId, UpdateVenueRequestDto dto);
    Task<VenueResponseDto> GetVenueByIdAsync(int venueId);

}
