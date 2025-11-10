using ZorgmeldSysteem.Blazor.Models.DTOs.Company;

namespace ZorgmeldSysteem.Blazor.ViewModels.Company
{
    public class CompaniesViewModel
    {
        // ==================== DATA ====================

        public List<CompanyDto> AllCompanies { get; set; } = new();

        // ==================== STATE ====================

        public bool IsLoading { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;
        public bool IsDeleting { get; set; }
        public CompanyDto? CompanyToDelete { get; set; }

        // ==================== FILTERS ====================

        public string SearchQuery { get; set; } = string.Empty;
        public string FilterIsExternal { get; set; } = string.Empty;  // Intern/Extern filter

        // ==================== EVENTS ====================

        public event Action? OnStateChanged;

        // ==================== PUBLIC METHODS ====================

        public async Task LoadCompaniesAsync(Func<Task<List<CompanyDto>>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                AllCompanies = await loadFunc();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van bedrijven: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public List<CompanyDto> GetFilteredCompanies()
        {
            var filtered = AllCompanies.AsEnumerable();

            // Filter op zoekterm
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                filtered = filtered.Where(c =>
                    c.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    c.Email.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    c.City.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    c.Contact.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Filter op Extern/Intern
            if (!string.IsNullOrWhiteSpace(FilterIsExternal))
            {
                var isExternal = bool.Parse(FilterIsExternal);
                filtered = filtered.Where(c => c.IsExternal == isExternal);
            }

            return filtered.OrderBy(c => c.Name).ToList();
        }

        public void ClearFilters()
        {
            SearchQuery = string.Empty;
            FilterIsExternal = string.Empty;
            NotifyStateChanged();
        }

        public bool HasActiveFilters()
        {
            return !string.IsNullOrWhiteSpace(SearchQuery)
                || !string.IsNullOrWhiteSpace(FilterIsExternal);
        }

        public void ShowDeleteConfirmation(CompanyDto company)
        {
            CompanyToDelete = company;
            NotifyStateChanged();
        }

        public void CancelDelete()
        {
            CompanyToDelete = null;
            NotifyStateChanged();
        }

        public async Task<bool> DeleteCompanyAsync(Func<int, Task<bool>> deleteFunc)
        {
            if (CompanyToDelete == null) return false;

            IsDeleting = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                bool success = await deleteFunc(CompanyToDelete.CompanyID);

                if (success)
                {
                    SuccessMessage = $"Bedrijf '{CompanyToDelete.Name}' succesvol verwijderd.";
                    AllCompanies.Remove(CompanyToDelete);
                    CompanyToDelete = null;
                    return true;
                }
                else
                {
                    ErrorMessage = "Fout bij verwijderen van bedrijf.";
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

        // ==================== PRIVATE METHODS ====================

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}