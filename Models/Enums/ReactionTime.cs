using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.Enums
{
    public enum ReactionTime
    {
        [Display(Name = "Can Wait")]
        CanWait = 1,

        [Display(Name = "Within Week")]
        WithinWeek = 2,

        [Display(Name = "Within 24 Hours")]
        Within24Hours = 3,

        [Display(Name = "Within 4 Hours")]
        Within4Hours = 4,

        [Display(Name = "Immediate")]
        Immediate = 5
    }
}