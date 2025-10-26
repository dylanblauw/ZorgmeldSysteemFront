using ZorgmeldSysteem.Blazor.Models.Enums;

namespace ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;

public class UpdateMechanicDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phonenumber { get; set; }
    public MechanicType? Type { get; set; }
    public bool? IsActive { get; set; }
    public int? CompanyID { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}