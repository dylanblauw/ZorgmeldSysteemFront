using ZorgmeldSysteem.Blazor.Models.DTOs.Object;

namespace ZorgmeldSysteem.Blazor.ViewModels.Object
{
    public class ObjectDetailsViewModel
    {
        // ==================== DATA ====================

        public ObjectDto? Object { get; set; }
        public string QRCodeBase64 { get; set; } = string.Empty;

        // ==================== STATE ====================

        public bool IsLoading { get; set; }
        public bool IsSaving { get; set; }
        public bool IsEditMode { get; set; }
        public bool IsDeleting { get; set; }
        public bool ShowDeleteConfirmation { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
        public string SuccessMessage { get; set; } = string.Empty;

        // ==================== EVENTS ====================

        public event Action? OnStateChanged;

        // ==================== PUBLIC METHODS ====================

        public async Task LoadObjectAsync(Func<Task<ObjectDto?>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                Object = await loadFunc();

                if (Object == null)
                {
                    ErrorMessage = "Object niet gevonden.";
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij laden: {ex.Message}";
            }
            finally
            {
                IsLoading = false;
                NotifyStateChanged();
            }
        }

        public async Task<bool> SaveObjectAsync(Func<Task<ObjectDto?>> saveFunc)
        {
            IsSaving = true;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                var result = await saveFunc();

                if (result != null)
                {
                    Object = result;
                    SuccessMessage = "Object succesvol bijgewerkt!";
                    IsEditMode = false;
                    return true;
                }
                else
                {
                    ErrorMessage = "Kon object niet opslaan.";
                    return false;
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij opslaan: {ex.Message}";
                return false;
            }
            finally
            {
                IsSaving = false;
                NotifyStateChanged();
            }
        }

        public async Task<bool> DeleteObjectAsync(Func<Task<bool>> deleteFunc)
        {
            IsDeleting = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                bool success = await deleteFunc();

                if (!success)
                {
                    ErrorMessage = "Kon object niet verwijderen.";
                }

                return success;
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Fout bij verwijderen: {ex.Message}";
                return false;
            }
            finally
            {
                IsDeleting = false;
                ShowDeleteConfirmation = false;
                NotifyStateChanged();
            }
        }

        public void EnableEditMode()
        {
            IsEditMode = true;
            SuccessMessage = string.Empty;
            ErrorMessage = string.Empty;
            NotifyStateChanged();
        }

        public void CancelEdit()
        {
            IsEditMode = false;
            ErrorMessage = string.Empty;
            SuccessMessage = string.Empty;
            NotifyStateChanged();
        }

        public void ShowDeleteDialog()
        {
            ShowDeleteConfirmation = true;
            NotifyStateChanged();
        }

        public void CancelDelete()
        {
            ShowDeleteConfirmation = false;
            NotifyStateChanged();
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

        // ==================== PRIVATE METHODS ====================

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}
