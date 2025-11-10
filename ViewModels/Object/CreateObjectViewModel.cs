using ZorgmeldSysteem.Blazor.Models.DTOs.Object;
using ZorgmeldSysteem.Blazor.Models.DTOs.Company;

namespace ZorgmeldSysteem.Blazor.ViewModels.Object
{
    public class CreateObjectViewModel
    {
        // Data
        public CreateObjectDto NewObject { get; set; } = new();
        public ObjectDto? CreatedObject { get; set; }
        public List<CompanyDto> Companies { get; set; } = new();

        // State
        public bool IsLoading { get; set; }
        public bool IsSaving { get; set; }
        public bool ObjectCreated { get; set; }
        public bool ShowPreview { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string QRCodeBase64 { get; set; } = string.Empty;

        // Events
        public event Action? OnStateChanged;

        // Methods
        public async Task LoadCompaniesAsync(Func<Task<List<CompanyDto>>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                Companies = await loadFunc();
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden van bedrijven: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public void ShowPreviewMode()
        {
            ShowPreview = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();
        }

        public void BackToForm()
        {
            ShowPreview = false;
            QRCodeBase64 = string.Empty;
            NotifyStateChanged();
        }

        public async Task SaveObjectAsync(Func<Task<ObjectDto?>> saveFunc)
        {
            IsSaving = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                CreatedObject = await saveFunc();

                if (CreatedObject != null)
                {
                    ObjectCreated = true;
                    ShowPreview = false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij aanmaken object: {ex.Message}";
            }
            finally
            {
                IsSaving = false;
                NotifyStateChanged();
            }
        }

        public void SetQRCode(string base64)
        {
            QRCodeBase64 = base64;
            NotifyStateChanged();
        }

        public void SetError(string message)
        {
            ErrorMessage = message;
            NotifyStateChanged();
        }

        public void ResetForm()
        {
            NewObject = new CreateObjectDto();
            CreatedObject = null;
            ObjectCreated = false;
            ShowPreview = false;
            QRCodeBase64 = string.Empty;
            ErrorMessage = string.Empty;
            NotifyStateChanged();
        }

        public string GetCompanyName(int companyId)
        {
            var company = Companies.FirstOrDefault(c => c.CompanyID == companyId);
            return company?.Name ?? "Onbekend";
        }

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}