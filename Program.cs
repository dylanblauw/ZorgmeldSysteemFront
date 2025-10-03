using ZorgmeldSysteem.Blazor.Components;

var builder = WebApplication.CreateBuilder(args);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// CORS toevoegen
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        policy.WithOrigins(
            "https://localhost:7159", 
            "http://localhost:5062"    
        )
        .AllowAnyHeader()
        .AllowAnyMethod();
    });
});

// Jouw services (Company, Mechanic, Ticket, etc.)
// builder.Services.AddScoped<ICompanyService, CompanyService>();
// etc...

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS gebruiken - VOOR UseAuthorization!
app.UseCors("AllowBlazor");

app.UseAuthorization();
app.MapControllers();

app.Run();