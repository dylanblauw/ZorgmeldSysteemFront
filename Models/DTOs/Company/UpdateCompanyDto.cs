namespace ZorgmeldSysteem.Blazor.Models.DTOs.Company;

public class UpdateCompanyDto
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Phonenumber { get; set; }
    public string? Street { get; set; }
    public string? HouseNumber { get; set; }
    public string? HouseNumberAddition { get; set; }
    public string? PostalCode { get; set; }
    public string? City { get; set; }
    public string? Province { get; set; }
    public string? Country { get; set; }

    public string? Contact { get; set; }
    public bool? IsExternal { get; set; }
    public string ChangedBy { get; set; } = string.Empty;
}