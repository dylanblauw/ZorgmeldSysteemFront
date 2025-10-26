using System.Net.Http.Json;
using ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;

namespace ZorgmeldSysteem.Blazor.Services.Api
{
    public class MechanicApiService
    {
        private readonly HttpClient _httpClient;

        public MechanicApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Haal alle monteurs op
        public async Task<List<MechanicDto>> GetAllMechanicsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MechanicDto>>("api/mechanic") ?? new List<MechanicDto>();
        }

        // Haal specifieke monteur op
        public async Task<MechanicDto?> GetMechanicByIdAsync(int mechanicId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<MechanicDto>($"api/mechanic/{mechanicId}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Haal alleen actieve monteurs op
        public async Task<List<MechanicDto>> GetActiveMechanicsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MechanicDto>>("api/mechanic/active") ?? new List<MechanicDto>();
        }

        // Haal monteurs op per type
        public async Task<List<MechanicDto>> GetMechanicsByTypeAsync(int type)
        {
            return await _httpClient.GetFromJsonAsync<List<MechanicDto>>($"api/mechanic/type/{type}") ?? new List<MechanicDto>();
        }

        // Maak nieuwe monteur aan
        public async Task<MechanicDto?> CreateMechanicAsync(CreateMechanicDto createDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/mechanic", createDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MechanicDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Update monteur
        public async Task<MechanicDto?> UpdateMechanicAsync(int mechanicId, UpdateMechanicDto updateDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/mechanic/{mechanicId}", updateDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<MechanicDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Verwijder monteur
        public async Task<bool> DeleteMechanicAsync(int mechanicId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/mechanic/{mechanicId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }
    }
}