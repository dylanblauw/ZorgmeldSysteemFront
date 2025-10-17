using ZorgmeldSysteem.Blazor.Components;
using ZorgmeldSysteem.Blazor.Services.Api;
using ZorgmeldSysteem.Blazor.Services.Helpers;
using ZorgmeldSysteem.Blazor.Services.Business;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// API Base URL configuratie
var apiBaseUrl = "https://localhost:7159";

// API Services met HttpClient
builder.Services.AddHttpClient<TicketApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

builder.Services.AddHttpClient<ObjectApiService>(client =>
{
    client.BaseAddress = new Uri(apiBaseUrl);
});

// Helper Services (Singleton - geen state)
builder.Services.AddSingleton<TicketDisplayService>();

// Business Services (Singleton - geen state)
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