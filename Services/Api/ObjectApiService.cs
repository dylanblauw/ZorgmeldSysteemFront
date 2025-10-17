using System.Net.Http.Json;
using ZorgmeldSysteem.Blazor.Models.DTOs.Object;

namespace ZorgmeldSysteem.Blazor.Services.Api
{
    public class ObjectApiService
    {
        private readonly HttpClient _httpClient;

        public ObjectApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Haal alle objecten op
        public async Task<List<ObjectDto>> GetAllObjectsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ObjectDto>>("api/object") ?? new List<ObjectDto>();
        }

        // Haal één object op
        public async Task<ObjectDto?> GetObjectByIdAsync(int objectId)
        {
            return await _httpClient.GetFromJsonAsync<ObjectDto>($"api/object/{objectId}");
        }

        // Haal object op via code
        public async Task<ObjectDto?> GetObjectByCodeAsync(string objectCode)
        {
            return await _httpClient.GetFromJsonAsync<ObjectDto>($"api/object/code/{objectCode}");
        }

        // Haal objecten op per bedrijf
        public async Task<List<ObjectDto>> GetObjectsByCompanyAsync(int companyId)
        {
            return await _httpClient.GetFromJsonAsync<List<ObjectDto>>($"api/object/company/{companyId}") ?? new List<ObjectDto>();
        }

        // Haal unieke locaties op per bedrijf
        public async Task<List<string>> GetLocationsByCompanyAsync(int companyId)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"api/object/company/{companyId}/locations") ?? new List<string>();
        }

        // Haal objecten op die onderhoud nodig hebben
        public async Task<List<ObjectDto>> GetObjectsDueForMaintenanceAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ObjectDto>>("api/object/maintenance/due") ?? new List<ObjectDto>();
        }

        // Maak nieuw object aan
        public async Task<ObjectDto?> CreateObjectAsync(CreateObjectDto createDto)
        {
            var response = await _httpClient.PostAsJsonAsync("api/object", createDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ObjectDto>();
        }

        // Update bestaand object
        public async Task<ObjectDto?> UpdateObjectAsync(int objectId, UpdateObjectDto updateDto)
        {
            var response = await _httpClient.PutAsJsonAsync($"api/object/{objectId}", updateDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<ObjectDto>();
        }

        // Verwijder object
        public async Task DeleteObjectAsync(int objectId)
        {
            var response = await _httpClient.DeleteAsync($"api/object/{objectId}");
            response.EnsureSuccessStatusCode();
        }
    }
}