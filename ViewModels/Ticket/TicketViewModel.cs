using ZorgmeldSysteem.Blazor.Models.DTOs.Ticket;
using ZorgmeldSysteem.Blazor.Models.Enums;

namespace ZorgmeldSysteem.Blazor.ViewModels.Ticket
{
    public class TicketsViewModel
    {
        // Data
        public List<TicketDto> AllTickets { get; set; } = new();

        // Filters
        public string SearchQuery { get; set; } = string.Empty;
        public string FilterStatus { get; set; } = string.Empty;
        public string FilterPriority { get; set; } = string.Empty;
        public string CodeFilter { get; set; } = string.Empty;

        // State
        public bool IsLoading { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        // Events
        public event Action? OnStateChanged;

        // Methods
        public async Task LoadTicketsAsync(Func<Task<List<TicketDto>>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                AllTickets = await loadFunc();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van tickets: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public IQueryable<TicketDto> GetFilteredTickets()
        {
            var filtered = AllTickets.AsQueryable();

            // Search filter
            if (!string.IsNullOrWhiteSpace(SearchQuery))
            {
                filtered = filtered.Where(t =>
                    t.TicketCode.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    t.Description.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase) ||
                    t.CompanyName.Contains(SearchQuery, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Code filter (voor QuickGrid kolom)
            if (!string.IsNullOrWhiteSpace(CodeFilter))
            {
                filtered = filtered.Where(t =>
                    t.TicketCode.Contains(CodeFilter, StringComparison.OrdinalIgnoreCase)
                );
            }

            // Status filter
            if (!string.IsNullOrWhiteSpace(FilterStatus))
            {
                if (int.TryParse(FilterStatus, out int statusValue))
                {
                    filtered = filtered.Where(t => (int)t.Status == statusValue);
                }
            }

            // Priority filter
            if (!string.IsNullOrWhiteSpace(FilterPriority))
            {
                if (int.TryParse(FilterPriority, out int priorityValue))
                {
                    filtered = filtered.Where(t => (int)t.Priority == priorityValue);
                }
            }

            return filtered;
        }

        public void ClearFilters()
        {
            SearchQuery = string.Empty;
            FilterStatus = string.Empty;
            FilterPriority = string.Empty;
            CodeFilter = string.Empty;
            NotifyStateChanged();
        }

        public bool HasActiveFilters()
        {
            return !string.IsNullOrWhiteSpace(SearchQuery) ||
                   !string.IsNullOrWhiteSpace(FilterStatus) ||
                   !string.IsNullOrWhiteSpace(FilterPriority) ||
                   !string.IsNullOrWhiteSpace(CodeFilter);
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}
























