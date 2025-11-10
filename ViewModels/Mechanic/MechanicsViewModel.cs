using ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;
using ZorgmeldSysteem.Blazor.Models.Enums;

namespace ZorgmeldSysteem.Blazor.ViewModels.Mechanic
{

    public class MechanicsViewModel
    {
        public List<MechanicDto> AllMechanics { get; set; } = new();

        public bool IsLoading { get; set; }

        public string ErrorMessage { get; set; } = string.Empty;

        public string SuccessMessage { get; set; } = string.Empty;

        public bool IsDeleting { get; set; }

        public MechanicDto? MechanicToDelete { get; set; }

        public string SearchQuery { get; set; } = string.Empty;

        public string FilterType { get; set; } = string.Empty;

        public string FilterIsActive { get; set; } = string.Empty;


        public event Action? OnStateChanged;


        public async Task LoadMechanicsAsync(Func<Task<List<MechanicDto>>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                AllMechanics = await loadFunc();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van monteurs: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        //geeft list van monteurs obv zoekterm en filters
        public List<MechanicDto> GetFilteredMechanics()
        {
            var filtered = AllMechanics.AsEnumerable();

            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                filtered = filtered.Where(m =>
                    m.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    m.Email.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    m.Phonenumber.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    (m.CompanyName != null && m.CompanyName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase))
                );
            }

            // Filter op type
            if (!string.IsNullOrWhiteSpace(FilterType))
            {
                // Probeer te parsen als int, anders probeer als enum naam
                if (int.TryParse(FilterType, out int typeValue))
                {
                    var type = (MechanicType)typeValue;
                    filtered = filtered.Where(m => m.Type == type);
                }
                else if (Enum.TryParse<MechanicType>(FilterType, out var type))
                {
                    filtered = filtered.Where(m => m.Type == type);
                }
            }

            // Filter op actief/inactief
            if (!string.IsNullOrWhiteSpace(FilterIsActive))
            {
                var isActive = bool.Parse(FilterIsActive);
                filtered = filtered.Where(m => m.IsActive == isActive);
            }

            return filtered.OrderBy(m => m.Name).ToList();
        }
        public void ClearFilters()
        {
            SearchQuery = string.Empty;
            FilterType = string.Empty;
            FilterIsActive = string.Empty;
            NotifyStateChanged();
        }
        public bool HasActiveFilters()
        {
            return !string.IsNullOrWhiteSpace(SearchQuery)
                || !string.IsNullOrWhiteSpace(FilterType)
                || !string.IsNullOrWhiteSpace(FilterIsActive);
        }

        public void ShowDeleteConfirmation(MechanicDto mechanic)
        {
            MechanicToDelete = mechanic;
            NotifyStateChanged();
        }

        public void CancelDelete()
        {
            MechanicToDelete = null;
            NotifyStateChanged();
        }

        public async Task<bool> DeleteMechanicAsync(Func<int, Task<bool>> deleteFunc)
        {
            if (MechanicToDelete == null) return false;

            IsDeleting = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                bool success = await deleteFunc(MechanicToDelete.MechanicID);

                if (success)
                {
                    SuccessMessage = $"Monteur '{MechanicToDelete.Name}' succesvol verwijderd.";
                    AllMechanics.Remove(MechanicToDelete);
                    MechanicToDelete = null;
                    return true;
                }
                else
                {
                    ErrorMessage = "Fout bij verwijderen van monteur.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij verwijderen: {ex.Message}";
                return false;
            }
            finally
            {
                IsDeleting = false;
                NotifyStateChanged();
            }
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();

    }
}