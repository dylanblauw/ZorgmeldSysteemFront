# Zorgmeld Systeem

Een ticket management systeem voor zorgmeldingen met een .NET 8 Web API en Blazor Server frontend.

##Projectstructuur

```
ZorgmeldSysteem/
├── ZorgmeldSysteem.WebApi/          # REST API
├── ZorgmeldSysteem.Blazor.Models/     # Business logic & DTOs
├── ZorgmeldSysteem.Domain/          # Entities & Enums
├── ZorgmeldSysteem.Infrastructure/  # Database & Repositories
└── ZorgmeldSysteem.Blazor/         # Frontend
```

## Aan de slag

### Vereisten

- .NET 8.0 SDK
- SQL Server (of SQL Server Express)
- Visual Studio 2022 (of VS Code)

### Installatie

1. **Clone het repository**
   ```bash
   git clone https://gitlab.com/jouwgebruiker/zorgmeldsysteem.git
   cd zorgmeldsysteem
   ```

2. **Database configureren**
   - Pas de connection string aan in `appsettings.json`
   - Voer database migraties uit (indien nodig)

3. **API starten**
   ```bash
   cd ZorgmeldSysteem.WebApi
   dotnet run
   ```

4. **Blazor frontend starten**
   ```bash
   cd ZorgmeldSysteem.Blazor
   dotnet run
   ```

## Configuratie

### API URL aanpassen

In `ZorgmeldSysteem.Blazor/Program.cs`:
```csharp
client.BaseAddress = new Uri("https://localhost:7159/");
```


## Functionaliteiten

- ✅ Ticket aanmaken
- ✅ Bedrijven beheren
- ✅ Monteurs beheren
- ✅ Objecten beheren
- 🔄 Ticket lijst (in ontwikkeling)
- 🔄 Ticket details (in ontwikkeling)

##  Technologieën

- **Backend**: .NET 8 Web API
- **Frontend**: Blazor Server
- **Database**: SQL Server
- **ORM**: Entity Framework Core

##  Team

Ontwikkeld door Edwin van der Wal
