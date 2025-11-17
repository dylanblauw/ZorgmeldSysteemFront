using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.DTOs.Auth
{
    public class RegisterRequestDto
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig email formaat")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        [MinLength(8, ErrorMessage = "Wachtwoord moet minimaal 8 karakters zijn")]
        public string Password { get; set; } = string.Empty;

        [Required(ErrorMessage = "Voornaam is verplicht")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Achternaam is verplicht")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Bedrijf is verplicht")]
        public int CompanyId { get; set; }

        [Required(ErrorMessage = "Gebruikersrol is verplicht")]
        public int UserLevel { get; set; }
    }
}