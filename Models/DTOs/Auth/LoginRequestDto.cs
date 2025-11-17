using System.ComponentModel.DataAnnotations;

namespace ZorgmeldSysteem.Blazor.Models.DTOs.Auth
{
    public class LoginRequestDto
    {
        [Required(ErrorMessage = "Email is verplicht")]
        [EmailAddress(ErrorMessage = "Ongeldig email formaat")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Wachtwoord is verplicht")]
        public string Password { get; set; } = string.Empty;
    }
}