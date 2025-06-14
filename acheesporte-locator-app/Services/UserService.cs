using acheesporte_locator_app.Config;
using acheesporte_locator_app.Dtos;
using acheesporte_locator_app.Interfaces;
using System.Net.Http.Json;

namespace acheesporte_locator_app.Services;

public class UserService : IUserInterface
{
    private readonly HttpClient _httpClient;
    private readonly ApiSettings _apiSettings;

    public UserService(HttpClient httpClient, ApiSettings apiSettings)
    {
        _httpClient = httpClient;
        _apiSettings = apiSettings;
    }

    public async Task<SignInResponseDto> SignInUserAsync(SignInRequestDto request)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(_apiSettings.SignInEndpoint, request);
            if (response.IsSuccessStatusCode)
            {
                var loginResponse = await response.Content.ReadFromJsonAsync<SignInResponseDto>();

                if (loginResponse is null || string.IsNullOrEmpty(loginResponse.Token))
                {
                    throw new Exception("Login failed. Invalid response from server.");
                }
                await SecureStorage.SetAsync("auth_token", loginResponse.Token);
                return loginResponse;
            }

            var backendError = await response.Content.ReadAsStringAsync();
            throw new Exception($"Login failed: {backendError}");
        }
        catch (Exception ex)
        {
            throw new Exception("An error occurred while logging in.", ex);
        }
    }
}
