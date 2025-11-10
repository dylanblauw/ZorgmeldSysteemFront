using ZorgmeldSysteem.Blazor.Models.DTOs.Object;

namespace ZorgmeldSysteem.Blazor.ViewModels.Object
{
    public class ObjectsViewModel
    {
        // Data
        public List<ObjectDto> AllObjects { get; set; } = new();

        // Filters
        public string SearchQuery { get; set; } = string.Empty;
        public string FilterCompany { get; set; } = string.Empty;
        public string FilterLocation { get; set; } = string.Empty;

        // State
        public bool IsLoading { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        // Events
        public event Action? OnStateChanged;

        // Methods
        public async Task LoadObjectsAsync(Func<Task<List<ObjectDto>>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                AllObjects = await loadFunc();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van objecten: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public List<ObjectDto> GetFilteredObjects()
        {
            var filtered = AllObjects.AsEnumerable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                filtered = filtered.Where(o =>
                    o.Name.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    o.ObjectCode.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    o.Location.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    o.CompanyName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Company filter
            if (!string.IsNullOrWhiteSpace(FilterCompany))
            {
                filtered = filtered.Where(o => o.CompanyName == FilterCompany);
            }

            // Location filter
            if (!string.IsNullOrWhiteSpace(FilterLocation))
            {
                filtered = filtered.Where(o => o.Location == FilterLocation);
            }

            return filtered.ToList();
        }

        public List<string> GetUniqueCompanies()
        {
            return AllObjects
                .Select(o => o.CompanyName)
                .Distinct()
                .OrderBy(c => c)
                .ToList();
        }

        public List<string> GetAvailableLocations()
        {
            if (string.IsNullOrEmpty(FilterCompany))
                return new List<string>();

            return AllObjects
                .Where(o => o.CompanyName == FilterCompany)
                .Select(o => o.Location)
                .Distinct()
                .OrderBy(l => l)
                .ToList();
        }

        public void ClearFilters()
        {
            SearchQuery = string.Empty;
            FilterCompany = string.Empty;
            FilterLocation = string.Empty;
            NotifyStateChanged();
        }

        public void OnCompanyFilterChanged()
        {
            // Reset location filter when company changes
            FilterLocation = string.Empty;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}