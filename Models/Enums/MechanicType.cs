using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.Enums
{
    public enum MechanicType
    {
        [Display(Name = "Internal General")]
        InternalGeneral = 1,

        [Display(Name = "Internal Electrical")]
        InternalElectrical = 2,

        [Display(Name = "Internal Plumbing")]
        InternalPlumbing = 3,

        [Display(Name = "Internal HVAC")]
        InternalHVAC = 4,

        [Display(Name = "External Electrical")]
        ExternalElectrical = 5,

        [Display(Name = "External Plumbing")]
        ExternalPlumbing = 6,

        [Display(Name = "External HVAC")]
        ExternalHVAC = 7,

        [Display(Name = "External Cleaning")]
        ExternalCleaning = 8,

        [Display(Name = "External Security")]
        ExternalSecurity = 9,

        [Display(Name = "External IT")]
        ExternalIT = 10,

        [Display(Name = "External General")]
        ExternalGeneral = 11
    }
}