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

        // Haal één object op via ID (belangrijk voor QR scanning!)
        public async Task<ObjectDto?> GetObjectByIdAsync(int objectId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<ObjectDto>($"api/object/{objectId}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Haal alle objecten op
        public async Task<List<ObjectDto>> GetAllObjectsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<ObjectDto>>("api/object") ?? new List<ObjectDto>();
        }

        // Haal objecten op per bedrijf
        public async Task<List<ObjectDto>> GetObjectsByCompanyAsync(int companyId)
        {
            return await _httpClient.GetFromJsonAsync<List<ObjectDto>>($"api/object/company/{companyId}") ?? new List<ObjectDto>();
        }

        // Haal locaties op per bedrijf
        public async Task<List<string>> GetLocationsByCompanyAsync(int companyId)
        {
            return await _httpClient.GetFromJsonAsync<List<string>>($"api/object/company/{companyId}/locations") ?? new List<string>();
        }

        // Maak nieuw object aan
        public async Task<ObjectDto?> CreateObjectAsync(CreateObjectDto createDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/object", createDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ObjectDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Update object
        public async Task<ObjectDto?> UpdateObjectAsync(int objectId, UpdateObjectDto updateDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/object/{objectId}", updateDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<ObjectDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Verwijder object
        public async Task<bool> DeleteObjectAsync(int objectId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/object/{objectId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}
