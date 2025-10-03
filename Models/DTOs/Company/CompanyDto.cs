namespace ZorgmeldSysteem.Blazor.Models.DTOs.Company;

public class CompanyDto
{
    public int CompanyID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phonenumber { get; set; } = string.Empty;

    public string Street { get; set; } = string.Empty;
    public string HouseNumber { get; set; } = string.Empty;
    public string HouseNumberAddition { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;

    public string Contact { get; set; } = string.Empty;
    public bool IsExternal { get; set; }

}