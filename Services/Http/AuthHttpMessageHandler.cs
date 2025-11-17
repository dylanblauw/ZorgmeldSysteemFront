using ZorgmeldSysteem.Blazor.Services.Auth;

namespace ZorgmeldSysteem.Blazor.Services.Http
{
    /// <summary>
    /// HTTP Message Handler die automatisch JWT token toevoegt aan alle requests
    /// </summary>
    public class AuthHttpMessageHandler : DelegatingHandler
    {
        private readonly CustomAuthenticationStateProvider _authStateProvider;

        public AuthHttpMessageHandler(CustomAuthenticationStateProvider authStateProvider)
        {
            _authStateProvider = authStateProvider;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // Haal token op
            var token = await _authStateProvider.GetTokenAsync();

            // Voeg token toe aan Authorization header (alleen als token bestaat)
            if (!string.IsNullOrEmpty(token))
            {
                request.Headers.Authorization =
                    new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            }

            // Verstuur request
            return await base.SendAsync(request, cancellationToken);
        }
    }
}