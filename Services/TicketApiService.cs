using System.Net.Http.Json;
using ZorgmeldSysteem.Blazor.Models.DTOs.Ticket;
using ZorgmeldSysteem.Blazor.Models.DTOs.Company;
using ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;
using ZorgmeldSysteem.Blazor.Models.DTOs.Object;

namespace ZorgmeldSysteem.Blazor.Services
{
    public class TicketApiService
    {
        private readonly HttpClient _httpClient;

        public TicketApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Ticket endpoints
        public async Task<TicketDto?> CreateTicketAsync(CreateTicketDto createDto)
        {
            HttpResponseMessage response = await _httpClient.PostAsJsonAsync("api/ticket", createDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketDto>();
        }

        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TicketDto>>("api/ticket") ?? new List<TicketDto>();
        }

        // Haal specifiek ticket op
        public async Task<TicketDto?> GetTicketByIdAsync(int ticketId)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<TicketDto>($"api/ticket/{ticketId}");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Update een ticket
        public async Task<TicketDto?> UpdateTicketAsync(int ticketId, UpdateTicketDto updateDto)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.PutAsJsonAsync($"api/ticket/{ticketId}", updateDto);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<TicketDto>();
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        // Delete een ticket
        public async Task<bool> DeleteTicketAsync(int ticketId)
        {
            try
            {
                HttpResponseMessage response = await _httpClient.DeleteAsync($"api/ticket/{ticketId}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException)
            {
                return false;
            }
        }

        // Company endpoints
        public async Task<List<CompanyDto>> GetAllCompaniesAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<CompanyDto>>("api/company") ?? new List<CompanyDto>();
        }

        // Mechanic endpoints
        public async Task<List<MechanicDto>> GetActiveMechanicsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<MechanicDto>>("api/mechanic/active") ?? new List<MechanicDto>();
        }

        // Dashboard statistieken
        public async Task<int> GetOpenTicketsCountAsync()
        {
            List<TicketDto> tickets = await _httpClient.GetFromJsonAsync<List<TicketDto>>("api/ticket/open") ?? new List<TicketDto>();
            return tickets.Count;
        }

        public async Task<int> GetTotalTicketsCountAsync()
        {
            List<TicketDto> tickets = await GetAllTicketsAsync();
            return tickets.Count;
        }

        public async Task<int> GetTotalCompaniesCountAsync()
        {
            List<CompanyDto> companies = await GetAllCompaniesAsync();
            return companies.Count;
        }

        public async Task<int> GetActiveMechanicsCountAsync()
        {
            List<MechanicDto> mechanics = await GetActiveMechanicsAsync();
            return mechanics.Count;
        }

        // Handige filter methodes
        public async Task<List<TicketDto>> GetTicketsByStatusAsync(string status)
        {
            return await _httpClient.GetFromJsonAsync<List<TicketDto>>($"api/ticket/status/{status}") ?? new List<TicketDto>();
        }

        public async Task<List<TicketDto>> GetTicketsByCompanyAsync(int companyId)
        {
            return await _httpClient.GetFromJsonAsync<List<TicketDto>>($"api/ticket/company/{companyId}") ?? new List<TicketDto>();
        }

        public async Task<List<TicketDto>> GetTicketsByMechanicAsync(int mechanicId)
        {
            return await _httpClient.GetFromJsonAsync<List<TicketDto>>($"api/ticket/mechanic/{mechanicId}") ?? new List<TicketDto>();
        }
    }
}