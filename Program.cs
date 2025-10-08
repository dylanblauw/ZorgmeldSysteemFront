using ZorgmeldSysteem.Blazor.Components;
using ZorgmeldSysteem.Blazor.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

// Configureer HttpClient voor je API met de JUISTE API poort
builder.Services.AddHttpClient<TicketApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7159/"); // API HTTPS poort
});
builder.Services.AddHttpClient<ObjectApiService>(client =>
{
    client.BaseAddress = new Uri("https://localhost:7159/");
});
var app = builder.Build();

// Configure the HTTP request pipeline.
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