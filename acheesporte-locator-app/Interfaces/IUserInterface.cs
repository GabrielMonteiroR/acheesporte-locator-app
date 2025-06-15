using acheesporte_locator_app.Dtos;

namespace acheesporte_locator_app.Interfaces;

public interface IUserInterface
{
    Task<SignInResponseDto> SignInUserAsync(SignInRequestDto requestDto);
    Task<SignUpResponseDto> SignUpUserAsync(SignUpRequestDto request);
    Task<CurrentUserDto> GetCurrentUserAsync();
}
