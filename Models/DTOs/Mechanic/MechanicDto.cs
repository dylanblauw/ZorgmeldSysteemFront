using ZorgmeldSysteem.Blazor.Models.Enums;

namespace ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic
{
    public class MechanicDto
    {
        public int MechanicID { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phonenumber { get; set; } = string.Empty;
        public MechanicType Type { get; set; }
        public bool IsActive { get; set; }
        public int? CompanyID { get; set; }
        public string? CompanyName { get; set; }

        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; } = string.Empty;
    }
}