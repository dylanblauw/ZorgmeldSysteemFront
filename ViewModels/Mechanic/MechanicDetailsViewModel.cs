using ZorgmeldSysteem.Blazor.Models.DTOs.Mechanic;

namespace ZorgmeldSysteem.Blazor.ViewModels.Mechanic
{
    public class MechanicDetailsViewModel
    {
        // ==================== DATA ====================

        public MechanicDto? Mechanic { get; set; }

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

        public async Task LoadMechanicAsync(Func<Task<MechanicDto?>> loadFunc)
        {
            IsLoading = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                Mechanic = await loadFunc();

                if (Mechanic == null)
                {
                    ErrorMessage = "Monteur niet gevonden.";
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

        public async Task<bool> SaveMechanicAsync(Func<Task<MechanicDto?>> saveFunc)
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
                    Mechanic = result;
                    SuccessMessage = "Monteur succesvol bijgewerkt!";
                    IsEditMode = false;
                    return true;
                }
                else
                {
                    ErrorMessage = "Kon monteur niet opslaan.";
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

        public async Task<bool> DeleteMechanicAsync(Func<Task<bool>> deleteFunc)
        {
            IsDeleting = true;
            ErrorMessage = string.Empty;
            NotifyStateChanged();

            try
            {
                bool success = await deleteFunc();

                if (!success)
                {
                    ErrorMessage = "Kon monteur niet verwijderen.";
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

        public void SetError(string message)
        {
            ErrorMessage = message;
            NotifyStateChanged();
        }

        // ==================== PRIVATE METHODS ====================

        private void NotifyStateChanged() => OnStateChanged?.Invoke();
    }
}