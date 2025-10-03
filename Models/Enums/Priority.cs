using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.Enums
{
    public enum Priority
    {
        [Display(Name = "Low")]
        Low = 1,

        [Display(Name = "Normal")]
        Normal = 2,

        [Display(Name = "High")]
        High = 3,

        [Display(Name = "Urgent")]
        Urgent = 4,

        [Display(Name = "Critical")]
        Critical = 5
    }
}