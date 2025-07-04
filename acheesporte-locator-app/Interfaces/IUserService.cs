﻿using acheesporte_locator_app.Dtos;
using acheesporte_locator_app.Dtos.Users;

namespace acheesporte_locator_app.Interfaces;

public interface IUserService
{
    Task<SignInResponseDto> SignInUserAsync(SignInRequestDto requestDto);
    Task<SignUpResponseDto> SignUpUserAsync(SignUpRequestDto request);
    Task<CurrentUserDto> GetCurrentUserAsync();
    Task<UserResponseDto> GetUserByIdAsync(int id);
    Task<UserResponseDto> UpdateUserAsync(int id, UpdateUserRequestDto dto);
    Task<UserResponseDto> UpdateUserProfileImageAsync(int userId, string imageUrl);
}
