using ZorgmeldSysteem.Blazor.Models.Enums;

namespace ZorgmeldSysteem.Blazor.Services.Helpers
{
    public class MechanicDisplayService
    {

        /// Geeft de Nederlandse naam van een monteur type

        public string GetMechanicTypeDisplayName(MechanicType type)
        {
            return type switch
            {
                MechanicType.InternalGeneral => "Intern - Algemeen",
                MechanicType.InternalElectrical => "Intern - Elektra",
                MechanicType.InternalPlumbing => "Intern - Loodgieter",
                MechanicType.InternalHVAC => "Intern - HVAC",
                MechanicType.ExternalElectrical => "Extern - Elektra",
                MechanicType.ExternalPlumbing => "Extern - Loodgieter",
                MechanicType.ExternalHVAC => "Extern - HVAC",
                MechanicType.ExternalCleaning => "Extern - Schoonmaak",
                MechanicType.ExternalSecurity => "Extern - Beveiliging",
                MechanicType.ExternalIT => "Extern - IT",
                MechanicType.ExternalGeneral => "Extern - Algemeen",
                _ => type.ToString()
            };
        }


        /// Geeft de CSS class voor een monteur type badge
 
        public string GetMechanicTypeBadgeClass(MechanicType type)
        {
            return type switch
            {
                // Intern = primary/info kleuren
                MechanicType.InternalGeneral => "bg-primary",
                MechanicType.InternalElectrical => "bg-info",
                MechanicType.InternalPlumbing => "bg-info",
                MechanicType.InternalHVAC => "bg-info",

                // Extern = secondary/warning kleuren
                MechanicType.ExternalElectrical => "bg-warning text-dark",
                MechanicType.ExternalPlumbing => "bg-warning text-dark",
                MechanicType.ExternalHVAC => "bg-warning text-dark",
                MechanicType.ExternalCleaning => "bg-secondary",
                MechanicType.ExternalSecurity => "bg-secondary",
                MechanicType.ExternalIT => "bg-secondary",
                MechanicType.ExternalGeneral => "bg-secondary",

                _ => "bg-secondary"
            };
        }




        public string GetMechanicTypeIcon(MechanicType type)
        {
            return type switch
            {
                MechanicType.InternalGeneral => "bi-tools",
                MechanicType.InternalElectrical => "bi-lightning-charge",
                MechanicType.InternalPlumbing => "bi-droplet",
                MechanicType.InternalHVAC => "bi-thermometer-half",
                MechanicType.ExternalElectrical => "bi-lightning-charge",
                MechanicType.ExternalPlumbing => "bi-droplet",
                MechanicType.ExternalHVAC => "bi-thermometer-half",
                MechanicType.ExternalCleaning => "bi-brush",
                MechanicType.ExternalSecurity => "bi-shield-lock",
                MechanicType.ExternalIT => "bi-pc-display",
                MechanicType.ExternalGeneral => "bi-tools",
                _ => "bi-person"
            };
        }


        /// Controleert of een monteur type intern is

        public bool IsInternalType(MechanicType type)
        {
            return type == MechanicType.InternalGeneral ||
                   type == MechanicType.InternalElectrical ||
                   type == MechanicType.InternalPlumbing ||
                   type == MechanicType.InternalHVAC;
        }


        /// Controleert of een monteur type extern is

        public bool IsExternalType(MechanicType type)
        {
            return !IsInternalType(type);
        }


        /// Geeft een korte beschrijving van het specialisme

        public string GetSpecialismShortName(MechanicType type)
        {
            return type switch
            {
                MechanicType.InternalGeneral or MechanicType.ExternalGeneral => "Algemeen",
                MechanicType.InternalElectrical or MechanicType.ExternalElectrical => "Elektra",
                MechanicType.InternalPlumbing or MechanicType.ExternalPlumbing => "Loodgieter",
                MechanicType.InternalHVAC or MechanicType.ExternalHVAC => "HVAC",
                MechanicType.ExternalCleaning => "Schoonmaak",
                MechanicType.ExternalSecurity => "Beveiliging",
                MechanicType.ExternalIT => "IT",
                _ => "Onbekend"
            };
        }
    }
}