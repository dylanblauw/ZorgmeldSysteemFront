using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.Enums
{
    public enum UserLevel
    {
        [Display(Name = "Admin")]
        Admin = 1,

        [Display(Name = "Manager")]
        Manager = 2,

        [Display(Name = "Mechanic")]
        Mechanic = 3,

        [Display(Name = "Reporter")]
        Reporter = 4
    }
}