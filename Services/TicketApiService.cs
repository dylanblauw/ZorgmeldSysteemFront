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
            var response = await _httpClient.PostAsJsonAsync("api/ticket", createDto);
            response.EnsureSuccessStatusCode();
            return await response.Content.ReadFromJsonAsync<TicketDto>();
        }

        public async Task<List<TicketDto>> GetAllTicketsAsync()
        {
            return await _httpClient.GetFromJsonAsync<List<TicketDto>>("api/ticket") ?? new List<TicketDto>();
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
            var tickets = await _httpClient.GetFromJsonAsync<List<TicketDto>>("api/ticket/open") ?? new List<TicketDto>();
            return tickets.Count;
        }

        public async Task<int> GetTotalTicketsCountAsync()
        {
            var tickets = await GetAllTicketsAsync();
            return tickets.Count;
        }

        public async Task<int> GetTotalCompaniesCountAsync()
        {
            var companies = await GetAllCompaniesAsync();
            return companies.Count;
        }

        public async Task<int> GetActiveMechanicsCountAsync()
        {
            var mechanics = await GetActiveMechanicsAsync();
            return mechanics.Count;
        }
    }
}