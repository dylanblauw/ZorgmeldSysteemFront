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
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(provider =>
    provider.GetRequiredService<CustomAuthenticationStateProvider>());

// ==================== HTTP MESSAGE HANDLER ====================
builder.Services.AddScoped<AuthHttpMessageHandler>();

// ==================== API BASE URL ====================
var apiBaseUrl = "https://localhost:7159";

// ==================== API SERVICES MET AUTH ====================
// AuthApiService (GEEN auth handler nodig voor login zelf)
builder.Services.AddHttpClient<AuthApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Andere API services MET auth handler (voegen automatisch token toe)
builder.Services.AddHttpClient<TicketApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddHttpClient<ObjectApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddHttpClient<MechanicApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>();

builder.Services.AddHttpClient<CompanyApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
})
.AddHttpMessageHandler<AuthHttpMessageHandler>();

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
app.UseAntiforgery();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();