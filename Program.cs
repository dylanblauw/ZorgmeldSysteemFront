using Microsoft.AspNetCore.Components.Authorization;
using ZorgmeldSysteem.Blazor.Components;
using ZorgmeldSysteem.Blazor.Services.Api;
using ZorgmeldSysteem.Blazor.Services.Auth;
using ZorgmeldSysteem.Blazor.Services.Business;
using ZorgmeldSysteem.Blazor.Services.Helpers;
using ZorgmeldSysteem.Blazor.Services.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// ==================== AUTHENTICATION ====================
builder.Services.AddAuthentication();
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

// ==================== AUTHORIZATION ====================
builder.Services.AddScoped<AuthorizationService>();

// ==================== HTTP MESSAGE HANDLER ====================
builder.Services.AddScoped<AuthHttpMessageHandler>();

// ==================== API BASE URL ====================
var apiBaseUrl = builder.Environment.IsDevelopment()
    ? "https://localhost:7159"  // Lokaal ontwikkelen
    : "https://zorgmeldsysteem-backend.fly.dev";  // Productie op Fly.io

// ==================== API SERVICES MET AUTH + SSL FIX ====================
// AuthApiService (GEEN auth handler nodig voor login zelf)
builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        builder.Environment.IsDevelopment()
            ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            : null
});

// Andere API services MET auth handler (voegen automatisch token toe)
builder.Services.AddHttpClient<TicketApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        builder.Environment.IsDevelopment()
            ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            : null
});

builder.Services.AddHttpClient<ObjectApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        builder.Environment.IsDevelopment()
            ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            : null
});

builder.Services.AddHttpClient<MechanicApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        builder.Environment.IsDevelopment()
            ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            : null
});

builder.Services.AddHttpClient<CompanyApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>()
.ConfigurePrimaryHttpMessageHandler(() => new HttpClientHandler
{
    ServerCertificateCustomValidationCallback =
        builder.Environment.IsDevelopment()
            ? HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            : null
});

// ==================== HELPER SERVICES ====================
builder.Services.AddSingleton<TicketDisplayService>();
builder.Services.AddSingleton<MechanicDisplayService>();

// ==================== BUSINESS SERVICES ====================
builder.Services.AddSingleton<QRCodeService>();

var app = builder.Build();

// Configure the HTTP request pipeline
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

// ==================== MIDDLEWARE ====================
app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();