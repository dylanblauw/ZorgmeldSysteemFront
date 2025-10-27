using ZorgmeldSysteem.Blazor.Models.Enums;
namespace ZorgmeldSysteem.Blazor.Services.Helpers
{
    public class TicketDisplayService
    {

        /// Geeft de Nederlandse naam van een ticket status

        public string GetStatusDisplayName(TicketStatus status)
        {
            return status switch
            {
                TicketStatus.Open => "Open",
                TicketStatus.Assigned => "Toegewezen",
                TicketStatus.InProgress => "In Behandeling",
                TicketStatus.PendingCustomer => "Wacht op Klant",
                TicketStatus.PendingSupplier => "Wacht op Leverancier",
                TicketStatus.PendingApproval => "Wacht op Goedkeuring",
                TicketStatus.AwaitingParts => "Wacht op Onderdelen",
                TicketStatus.Scheduled => "Ingepland",
                TicketStatus.OnHold => "In de Wacht",
                TicketStatus.Solved => "Opgelost",
                TicketStatus.Closed => "Gesloten",
                TicketStatus.Cancelled => "Geannuleerd",
                _ => $"Onbekend ({(int)status})"  // Toon het getal voor debugging
            };
        }


        /// Geeft de Nederlandse naam van een prioriteit

        public string GetPriorityDisplayName(Priority priority)
        {
            return priority switch
            {
                Priority.Low => "Laag",
                Priority.Normal => "Normaal",
                Priority.High => "Hoog",
                Priority.Urgent => "Urgent",
                Priority.Critical => "Kritiek",
                _ => priority.ToString()
            };
        }


        /// Geeft de CSS class voor een status badge

        public string GetStatusBadgeClass(TicketStatus status)
        {
            return status switch
            {
                TicketStatus.Open => "bg-warning text-dark",
                TicketStatus.Assigned => "bg-info",
                TicketStatus.InProgress => "bg-primary",
                TicketStatus.PendingCustomer => "bg-warning",
                TicketStatus.PendingSupplier => "bg-warning",
                TicketStatus.PendingApproval => "bg-warning",
                TicketStatus.AwaitingParts => "bg-info",
                TicketStatus.Scheduled => "bg-info",
                TicketStatus.OnHold => "bg-secondary",
                TicketStatus.Solved => "bg-success",
                TicketStatus.Closed => "bg-secondary",
                TicketStatus.Cancelled => "bg-danger",
                _ => "bg-secondary"
            };
        }

        /// Geeft de CSS class voor een prioriteit badge

        public string GetPriorityBadgeClass(Priority priority)
        {
            return priority switch
            {
                Priority.Low => "bg-secondary",
                Priority.Normal => "bg-info",
                Priority.High => "bg-warning text-dark",
                Priority.Urgent => "bg-danger",
                Priority.Critical => "bg-dark",
                _ => "bg-secondary"
            };
        }


        /// Geeft het Bootstrap icoon voor een prioriteit

        public string GetPriorityIcon(Priority priority)
        {
            return priority switch
            {
                Priority.Low => "bi-arrow-down",
                Priority.Normal => "bi-dash",
                Priority.High => "bi-arrow-up",
                Priority.Urgent => "bi-exclamation-triangle",
                Priority.Critical => "bi-exclamation-octagon",
                _ => "bi-dash"
            };
        }


        /// Controleert of een status als "actief" beschouwd moet worden

        public bool IsActiveStatus(TicketStatus status)
        {
            return status == TicketStatus.Open ||
                   status == TicketStatus.Assigned ||
                   status == TicketStatus.InProgress ||
                   status == TicketStatus.Scheduled;
        }


        /// Controleert of een prioriteit als "urgent" beschouwd moet worden

        public bool IsUrgentPriority(Priority priority)
        {
            return priority == Priority.Urgent || priority == Priority.Critical;
        }
    }
}