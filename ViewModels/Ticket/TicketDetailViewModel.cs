using ZorgmeldSysteem.Blazor.Models.DTOs.Ticket;
using ZorgmeldSysteem.Blazor.Models.DTOs.Company;
using ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;
using ZorgmeldSysteem.Blazor.Models.DTOs.Object;

namespace ZorgmeldSysteem.Blazor.ViewModels.Ticket
{
    public class TicketDetailViewModel
    {
        // Data
        public TicketDto? Ticket { get; set; }
        public List<CompanyDto> Companies { get; set; } = new();
        public List<MechanicDto> Mechanics { get; set; } = new();
        public List<ObjectDto> AvailableObjects { get; set; } = new();
        public List<string> AvailableLocations { get; set; } = new();

        // State
        public bool IsLoading { get; set; }
        public bool IsSaving { get; set; }
        public bool IsEditMode { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        // Events
        public event Action? OnStateChanged;


        public List<ObjectDto> GetFilteredObjects()
        {
            return AvailableObjects;
        }

      
        public async Task LoadTicketAsync(Func<Task<TicketDto?>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                Ticket = await loadFunc();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van ticket: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public async Task LoadDropdownDataAsync(
            Func<Task<List<CompanyDto>>> loadCompanies,
            Func<Task<List<MechanicDto>>> loadMechanics)
        {
            try
            {
                var companiesTask = loadCompanies();
                var mechanicsTask = loadMechanics();

                await Task.WhenAll(companiesTask, mechanicsTask);

                Companies = await companiesTask;
                Mechanics = await mechanicsTask;
                NotifyStateChanged();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van data: {ex.Message}";
                NotifyStateChanged();
            }
        }

        public async Task LoadCompanyDataAsync(int companyId, Func<int, Task<List<string>>> loadLocations, Func<int, Task<List<ObjectDto>>> loadObjects)
        {
            if (companyId <= 0)
            {
                AvailableLocations.Clear();
                AvailableObjects.Clear();
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
                NotifyStateChanged();  // ← HET PROBLEEM: Location/Object wordt NIET gereset!
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van bedrijfsgegevens: {ex.Message}";
                NotifyStateChanged();
            }
        }
        //}
        //public async Task LoadCompanyDataAsync(
        //    int companyId,
        //    Func<int, Task<List<string>>> loadLocations,
        //    Func<int, Task<List<ObjectDto>>> loadObjects)
        //{
        //    if (companyId <= 0)
        //    {
        //        AvailableLocations.Clear();
        //        AvailableObjects.Clear();
        //        NotifyStateChanged();
        //        return;
        //    }

        //    try
        //    {
        //        var locationsTask = loadLocations(companyId);
        //        var objectsTask = loadObjects(companyId);

        //        await Task.WhenAll(locationsTask, objectsTask);

        //        AvailableLocations = await locationsTask;
        //        AvailableObjects = await objectsTask;
        //        NotifyStateChanged();
        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorMessage = $"Fout bij laden van bedrijfsgegevens: {ex.Message}";
        //        NotifyStateChanged();
        //    }
        //}

        public void EnableEditMode()
        {
            IsEditMode = true;
            SuccessMessage = string.Empty;
            NotifyStateChanged();
        }

        public void CancelEdit()
        {
            IsEditMode = false;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            NotifyStateChanged();
        }

        public async Task SaveTicketAsync(Func<Task<TicketDto?>> saveFunc)
        {
            IsSaving = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                var result = await saveFunc();
                if (result != null)
                {
                    Ticket = result;
                    SuccessMessage = "Ticket succesvol bijgewerkt!";
                    IsEditMode = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij opslaan: {ex.Message}";
            }
            finally
            {
                IsSaving = false;
                NotifyStateChanged();
            }
        }

        public void SetCompany(int companyId)
        {
            if (Ticket != null)
            {
                Ticket.CompanyID = companyId;
                NotifyStateChanged();
            }
        }

        public void SetLocation(string location)
        {
            if (Ticket != null)
            {
                Ticket.Location = location;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}