using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using System.Security.Claims;
using System.Text.Json;
using ZorgmeldSysteem.Blazor.Models.DTOs.Auth;

namespace ZorgmeldSysteem.Blazor.Services.Auth
{
    public class CustomAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IJSRuntime _jsRuntime;
        private AuthResponseDto? _currentUser;
        private bool _isInitialized = false;

        public CustomAuthenticationStateProvider(IJSRuntime jsRuntime)
        {
            _jsRuntime = jsRuntime;
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // Check of we in prerendering zijn
            if (!_isInitialized)
            {
                try
                {
                    // Probeer localStorage te lezen
                    await InitializeAsync();
                }
                catch
                {
                    // Als het faalt (prerendering), return anonymous user
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }
            }

            if (_currentUser == null || string.IsNullOrEmpty(_currentUser.Token))
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Check of token is verlopen
            if (_currentUser.TokenExpiration < DateTime.UtcNow)
            {
                await LogoutAsync();
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }

            // Maak claims van user data
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, _currentUser.UserID.ToString()),
                new Claim(ClaimTypes.Email, _currentUser.Email),
                new Claim(ClaimTypes.Name, _currentUser.FullName),
                new Claim(ClaimTypes.GivenName, _currentUser.FirstName),
                new Claim(ClaimTypes.Surname, _currentUser.LastName),
                new Claim(ClaimTypes.Role, _currentUser.UserLevelName),
                new Claim("UserLevel", _currentUser.UserLevel.ToString())
            };

            var identity = new ClaimsIdentity(claims, "jwt");
            var user = new ClaimsPrincipal(identity);

            return new AuthenticationState(user);
        }

        /// <summary>
        /// Initialize auth state from localStorage (alleen na prerendering)
        /// </summary>
        private async Task InitializeAsync()
        {
            try
            {
                var userJson = await _jsRuntime.InvokeAsync<string>("localStorage.getItem", "currentUser");

                if (!string.IsNullOrEmpty(userJson))
                {
                    _currentUser = JsonSerializer.Deserialize<AuthResponseDto>(userJson);
                }

                _isInitialized = true;
            }
            catch (InvalidOperationException)
            {
                // Prerendering - JavaScript niet beschikbaar
                _isInitialized = false;
                throw;
            }
        }

        /// <summary>
        /// Login gebruiker en sla token + user data op
        /// </summary>
        public async Task LoginAsync(AuthResponseDto authResponse)
        {
            _currentUser = authResponse;
            _isInitialized = true;

            try
            {
                // Sla user data op in localStorage
                var userJson = JsonSerializer.Serialize(authResponse);
                await _jsRuntime.InvokeVoidAsync("localStorage.setItem", "currentUser", userJson);
            }
            catch (InvalidOperationException)
            {
                // JavaScript nog niet beschikbaar - dat is OK, we hebben _currentUser in memory
            }

            // Update auth state
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Logout gebruiker en verwijder token + user data
        /// </summary>
        public async Task LogoutAsync()
        {
            _currentUser = null;
            _isInitialized = true;

            try
            {
                // Verwijder user data uit localStorage
                await _jsRuntime.InvokeVoidAsync("localStorage.removeItem", "currentUser");
            }
            catch (InvalidOperationException)
            {
                // JavaScript nog niet beschikbaar - dat is OK
            }

            // Update auth state
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }

        /// <summary>
        /// Haal JWT token op voor API calls
        /// </summary>
        public async Task<string?> GetTokenAsync()
        {
            if (_currentUser != null && !string.IsNullOrEmpty(_currentUser.Token))
            {
                return _currentUser.Token;
            }

            // Probeer token uit localStorage te halen
            if (!_isInitialized)
            {
                try
                {
                    await InitializeAsync();
                }
                catch
                {
                    return null;
                }
            }

            return _currentUser?.Token;
        }

        /// <summary>
        /// Haal huidige ingelogde gebruiker op
        /// </summary>
        public async Task<AuthResponseDto?> GetCurrentUserAsync()
        {
            if (_currentUser != null)
                return _currentUser;

            if (!_isInitialized)
            {
                try
                {
                    await InitializeAsync();
                }
                catch
                {
                    return null;
                }
            }

            return _currentUser;
        }
    }
}