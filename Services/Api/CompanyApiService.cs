using System.Net.Http.Json;
using ZorgmeldSysteem.Blazor.Models.DTOs.Company;

namespace ZorgmeldSysteem.Blazor.Services.Api
{
    public class CompanyApiService
    {
        private readonly HttpClient _httpClient;

        public CompanyApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Haal alle bedrijven op
        public async Task<List<CompanyDto>> GetAllCompaniesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CompanyDto>>("api/company") ?? new List<CompanyDto>();
        }

        // Haal specifiek bedrijf op
        public async Task<CompanyDto?> GetCompanyByIdAsync(int companyId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<CompanyDto>($"api/company/{companyId}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Haal alleen externe bedrijven op
        public async Task<List<CompanyDto>> GetExternalCompaniesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CompanyDto>>("api/company/external") ?? new List<CompanyDto>();
        }

        // Maak nieuw bedrijf aan
        public async Task<CompanyDto?> CreateCompanyAsync(CreateCompanyDto createDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/company", createDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CompanyDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Update bedrijf
        public async Task<CompanyDto?> UpdateCompanyAsync(int companyId, UpdateCompanyDto updateDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/company/{companyId}", updateDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<CompanyDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Verwijder bedrijf
        public async Task<bool> DeleteCompanyAsync(int companyId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/company/{companyId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}