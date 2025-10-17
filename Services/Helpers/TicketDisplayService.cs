using ZorgmeldSysteem.Blazor.Models.Enums;

namespace ZorgmeldSysteem.Blazor.Services.Helpers
{
    public class TicketDisplayService
    {
        /// <summary>
        /// Geeft de Nederlandse naam van een ticket status
        /// </summary>
        public string GetStatusDisplayName(TicketStatus status)
        {
            return status switch
            {
                TicketStatus.Open => "Open",
                TicketStatus.Assigned => "Toegewezen",
                TicketStatus.InProgress => "In Behandeling",
                TicketStatus.Solved => "Opgelost",
                TicketStatus.Closed => "Gesloten",
                TicketStatus.Cancelled => "Geannuleerd",
                _ => status.ToString()
            };
        }

        /// <summary>
        /// Geeft de Nederlandse naam van een prioriteit
        /// </summary>
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

        /// <summary>
        /// Geeft de CSS class voor een status badge
        /// </summary>
        public string GetStatusBadgeClass(TicketStatus status)
        {
            return status switch
            {
                TicketStatus.Open => "bg-warning text-dark",
                TicketStatus.Assigned => "bg-info",
                TicketStatus.InProgress => "bg-primary",
                TicketStatus.Solved => "bg-success",
                TicketStatus.Closed => "bg-secondary",
                TicketStatus.Cancelled => "bg-danger",
                _ => "bg-secondary"
            };
        }

        /// <summary>
        /// Geeft de CSS class voor een prioriteit badge
        /// </summary>
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

        /// <summary>
        /// Geeft het Bootstrap icoon voor een prioriteit
        /// </summary>
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

        /// <summary>
        /// Controleert of een status als "actief" beschouwd moet worden
        /// </summary>
        public bool IsActiveStatus(TicketStatus status)
        {
            return status == TicketStatus.Open ||
                   status == TicketStatus.Assigned ||
                   status == TicketStatus.InProgress;
        }

        /// <summary>
        /// Controleert of een prioriteit als "urgent" beschouwd moet worden
        /// </summary>
        public bool IsUrgentPriority(Priority priority)
        {
            return priority == Priority.Urgent || priority == Priority.Critical;
        }
    }
}










