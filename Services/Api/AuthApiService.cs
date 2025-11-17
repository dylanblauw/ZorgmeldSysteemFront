using System.Net.Http.Json;
using ZorgmeldSysteem.Blazor.Models.DTOs.Auth;

namespace ZorgmeldSysteem.Blazor.Services.Api
{
    public class AuthApiService
    {
        private readonly HttpClient _httpClient;

        public AuthApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Login gebruiker en ontvang JWT token
        /// </summary>
        public async Task<AuthResponseDto?> LoginAsync(LoginRequestDto loginDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/login", loginDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                }

                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        /// <summary>
        /// Registreer nieuwe gebruiker
        /// </summary>
        public async Task<AuthResponseDto?> RegisterAsync(RegisterRequestDto registerDto)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/auth/register", registerDto);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                }

                return null;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        /// <summary>
        /// Test of de API bereikbaar is
        /// </summary>
        public async Task<bool> TestConnectionAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync("api/auth/test");
                return response.IsSuccessStatusCode;
            }
            catch
            {
                return false;
            }
        }
    }
}