using ZorgmeldSysteem.Blazor.Models.DTOs.Ticket;
using ZorgmeldSysteem.Blazor.Models.DTOs.Company;
using ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;
using ZorgmeldSysteem.Blazor.Models.DTOs.Object;

namespace ZorgmeldSysteem.Blazor.ViewModels.Ticket
{
    public class CreateTicketViewModel
    {
        // Data
        public CreateTicketDto NewTicket { get; set; } = new();
        public List<CompanyDto> Companies { get; set; } = new();
        public List<MechanicDto> Mechanics { get; set; } = new();
        public List<ObjectDto> AvailableObjects { get; set; } = new();
        public List<string> AvailableLocations { get; set; } = new();

        // State
        public bool IsLoading { get; set; }
        public bool IsSaving { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        // Events
        public event Action? OnStateChanged;

        // Computed Properties
        public List<ObjectDto> GetFilteredObjects()
        {
            if (string.IsNullOrWhiteSpace(NewTicket.Location))
            {
                return AvailableObjects;
            }

            return AvailableObjects
                .Where(obj => obj.Location.Equals(NewTicket.Location, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        public int GetFilteredObjectCount()
        {
            return GetFilteredObjects().Count;
        }

        // Methods
        public async Task LoadInitialDataAsync(
            Func<Task<List<CompanyDto>>> loadCompanies,
            Func<Task<List<MechanicDto>>> loadMechanics)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                var companiesTask = loadCompanies();
                var mechanicsTask = loadMechanics();

                await Task.WhenAll(companiesTask, mechanicsTask);

                Companies = await companiesTask;
                Mechanics = await mechanicsTask;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van data: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public async Task LoadCompanyDataAsync(
            int companyId,
            Func<int, Task<List<string>>> loadLocations,
            Func<int, Task<List<ObjectDto>>> loadObjects)
        {
            if (companyId <= 0)
            {
                AvailableLocations.Clear();
                AvailableObjects.Clear();
                NewTicket.Location = string.Empty;
                NotifyStateChanged();
                return;
            }

            try
            {
                var locationsTask = loadLocations(companyId);
                var objectsTask = loadObjects(companyId);

                await Task.WhenAll(locationsTask, objectsTask);

                AvailableLocations = await locationsTask;
                AvailableObjects = await objectsTask;
                NewTicket.Location = string.Empty;
                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van objecten: {ex.Message}";
                NotifyStateChanged();
            }
        }

        public void SetCompany(int companyId)
        {
            NewTicket.CompanyID = companyId;
            NotifyStateChanged();
        }

        public void SetLocation(string location)
        {
            NewTicket.Location = location;
            NotifyStateChanged();
        }

        public async Task<TicketDto?> SaveTicketAsync(Func<CreateTicketDto, Task<TicketDto?>> saveFunc)
        {
            IsSaving = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                return await saveFunc(NewTicket);
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij aanmaken ticket: {ex.Message}";
                return null;
            }
            finally
            {
                IsSaving = false;
                NotifyStateChanged();
            }
        }

        public async Task PreFillFromObjectAsync(
            int objectId,
            Func<int, Task<ObjectDto?>> loadObjectFunc,
            Func<int, Task<List<string>>> loadLocationsFunc,
            Func<int, Task<List<ObjectDto>>> loadObjectsFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                // Laad het object
                var objectDto = await loadObjectFunc(objectId);

                if (objectDto == null)
                {
                    ErrorMessage = "Object niet gevonden.";
                    return;
                }

                // Pre-fill Company
                NewTicket.CompanyID = objectDto.CompanyID;

                // Laad company-specifieke data
                var locationsTask = loadLocationsFunc(objectDto.CompanyID);
                var objectsTask = loadObjectsFunc(objectDto.CompanyID);

                await Task.WhenAll(locationsTask, objectsTask);

                AvailableLocations = await locationsTask;
                AvailableObjects = await objectsTask;

                // Pre-fill Location
                NewTicket.Location = objectDto.Location;

                // Pre-fill Object
                NewTicket.ObjectId = objectDto.ObjectID;

                // Pre-fill Priority & ReactionTime
                NewTicket.Priority = objectDto.DefaultPriority;
                NewTicket.ReactionTime = objectDto.DefaultReactionTime;

                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van object: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}