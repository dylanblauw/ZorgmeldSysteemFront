using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.Enums
{
    public enum TicketStatus
    {
        [Display(Name = "Open")]
        Open = 1,

        [Display(Name = "Assigned")]
        Assigned = 2,

        [Display(Name = "In Progress")]
        InProgress = 3,

        [Display(Name = "Pending Customer")]
        PendingCustomer = 4,

        [Display(Name = "Pending Supplier")]
        PendingSupplier = 5,

        [Display(Name = "Pending Approval")]
        PendingApproval = 6,

        [Display(Name = "Awaiting Parts")]
        AwaitingParts = 7,

        [Display(Name = "Scheduled")]
        Scheduled = 8,

        [Display(Name = "On Hold")]
        OnHold = 9,

        [Display(Name = "Solved")]
        Solved = 10,

        [Display(Name = "Closed")]
        Closed = 11,

        [Display(Name = "Cancelled")]
        Cancelled = 12
    }
}