using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using ZorgmeldSysteem.Blazor.Models.DTOs.Auth;
using ZorgmeldSysteem.Blazor.Services.Auth;

namespace ZorgmeldSysteem.Blazor.Services.Auth
{
    /// <summary>
    /// Helper service voor het checken van gebruikersrechten
    /// </summary>
    public class AuthorizationService
    {
        private readonly AuthenticationStateProvider _authStateProvider;
        private readonly CustomAuthenticationStateProvider _customAuthProvider;

        public AuthorizationService(
            AuthenticationStateProvider authStateProvider,
            CustomAuthenticationStateProvider customAuthProvider)
        {
            _authStateProvider = authStateProvider;
            _customAuthProvider = customAuthProvider;
        }

        /// <summary>
        /// Haal huidige user info op
        /// </summary>
        public async Task<AuthResponseDto?> GetCurrentUserAsync()
        {
            return await _customAuthProvider.GetCurrentUserAsync();
        }

        /// <summary>
        /// Check of user is ingelogd
        /// </summary>
        public async Task<bool> IsAuthenticatedAsync()
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return authState.User.Identity?.IsAuthenticated ?? false;
        }

        /// <summary>
        /// Check of user een specifieke rol heeft
        /// </summary>
        public async Task<bool> IsInRoleAsync(params string[] roles)
        {
            var authState = await _authStateProvider.GetAuthenticationStateAsync();
            return roles.Any(role => authState.User.IsInRole(role));
        }

        /// <summary>
        /// Check of user Admin is
        /// </summary>
        public async Task<bool> IsAdminAsync()
        {
            return await IsInRoleAsync("Admin" , "Fixility Admin");
        }

        /// <summary>
        /// Check of user Manager is
        /// </summary>
        public async Task<bool> IsManagerAsync()
        {
            return await IsInRoleAsync("Manager" , "Bedrijfsbeheerder");
        }

        /// <summary>
        /// Check of user Mechanic is
        /// </summary>
        public async Task<bool> IsMechanicAsync()
        {
            return await IsInRoleAsync("Mechanic", "Monteur");
        }

        /// <summary>
        /// Check of user Reporter is
        /// </summary>
        public async Task<bool> IsReporterAsync()
        {
            return await IsInRoleAsync("Reporter" , "Melder");
        }

        /// <summary>
        /// Check of user Admin of Manager is
        /// </summary>
        public async Task<bool> IsAdminOrManagerAsync()
        {
            return await IsInRoleAsync("Admin", "Fixility Admin", "Manager", "Beheerder", "Bedrijfsbeheerder");
        }

        /// <summary>
        /// Haal UserID op
        /// </summary>
        public async Task<int?> GetUserIdAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.UserID;
        }

        /// <summary>
        /// Haal CompanyIDs op waar user toegang tot heeft
        /// </summary>
        public async Task<List<int>> GetUserCompanyIdsAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.CompanyIDs ?? new List<int>();
        }

        /// <summary>
        /// Check of user toegang heeft tot een specifiek bedrijf
        /// </summary>
        public async Task<bool> HasAccessToCompanyAsync(int companyId)
        {
            // Admin heeft toegang tot alles
            if (await IsAdminAsync())
                return true;

            var companyIds = await GetUserCompanyIdsAsync();
            return companyIds.Contains(companyId);
        }

        /// <summary>
        /// Check of een mechanic toegang heeft tot een ticket
        /// </summary>
        public async Task<bool> CanAccessTicketAsync(int? ticketMechanicId, int? ticketCompanyId)
        {
            // Admin heeft toegang tot alles
            if (await IsAdminAsync())
                return true;

            var userId = await GetUserIdAsync();

            // Mechanic: alleen eigen tickets
            if (await IsMechanicAsync())
            {
                return ticketMechanicId.HasValue && ticketMechanicId.Value == userId;
            }

            // Manager/Reporter: tickets van eigen bedrijf
            if (ticketCompanyId.HasValue)
            {
                return await HasAccessToCompanyAsync(ticketCompanyId.Value);
            }

            return false;
        }

        /// <summary>
        /// Haal UserLevel waarde op (1=Admin, 2=Manager, 3=Mechanic, 4=Reporter)
        /// </summary>
        public async Task<int?> GetUserLevelAsync()
        {
            var user = await GetCurrentUserAsync();
            return user?.UserLevel;
        }
    }
}