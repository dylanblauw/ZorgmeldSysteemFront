using QRCoder;

namespace ZorgmeldSysteem.Blazor.Services.Business
{
    /// <summary>
    /// Interface voor QR Code service - maakt testing makkelijker
    /// </summary>
    public interface IQRCodeService
    {
        string GeneratePreviewQRCode(string objectCode);
        string GenerateObjectQRCode(int objectId);
        string GetBase64DataFromDataUrl(string dataUrl);
    }

    /// <summary>
    /// Service voor het genereren van QR codes voor objecten
    /// </summary>
    public class QRCodeService : IQRCodeService
    {
        private readonly int _qrCodePixelsPerModule;
        private readonly QRCodeGenerator.ECCLevel _errorCorrectionLevel;

        /// <summary>
        /// Constructor met default waardes
        /// </summary>
        public QRCodeService() : this(20, QRCodeGenerator.ECCLevel.Q)
        {
        }

        /// <summary>
        /// Constructor met configureerbare waardes
        /// </summary>
        /// <param name="pixelsPerModule">Grootte van QR code (default: 20)</param>
        /// <param name="errorCorrectionLevel">Error correction level (default: Q = 25%)</param>
        public QRCodeService(int pixelsPerModule, QRCodeGenerator.ECCLevel errorCorrectionLevel)
        {
            if (pixelsPerModule <= 0)
                throw new ArgumentException("Pixels per module moet groter dan 0 zijn", nameof(pixelsPerModule));

            _qrCodePixelsPerModule = pixelsPerModule;
            _errorCorrectionLevel = errorCorrectionLevel;
        }

        /// <summary>
        /// Genereert een preview QR code (voor opslaan in database)
        /// </summary>
        /// <param name="objectCode">De object code</param>
        /// <returns>Base64 data URL van de QR code</returns>
        /// <exception cref="ArgumentException">Als objectCode leeg is</exception>
        /// <exception cref="InvalidOperationException">Als QR code generatie faalt</exception>
        public string GeneratePreviewQRCode(string objectCode)
        {
            if (string.IsNullOrWhiteSpace(objectCode))
                throw new ArgumentException("Object code mag niet leeg zijn", nameof(objectCode));

            string qrData = $"PREVIEW:{objectCode}";
            return GenerateQRCodeInternal(qrData);
        }

        /// <summary>
        /// Genereert een definitieve QR code (na opslaan in database)
        /// </summary>
        /// <param name="objectId">Het database ID van het object</param>
        /// <returns>Base64 data URL van de QR code</returns>
        /// <exception cref="ArgumentException">Als objectId kleiner of gelijk aan 0 is</exception>
        /// <exception cref="InvalidOperationException">Als QR code generatie faalt</exception>
        public string GenerateObjectQRCode(int objectId)
        {
            if (objectId <= 0)
                throw new ArgumentException("Object ID moet groter dan 0 zijn", nameof(objectId));

            string qrData = $"OBJECT:{objectId}";
            return GenerateQRCodeInternal(qrData);
        }

        /// <summary>
        /// Haalt de base64 data uit een data URL
        /// </summary>
        /// <param name="dataUrl">Data URL in formaat: data:image/png;base64,{data}</param>
        /// <returns>Alleen de base64 data</returns>
        /// <exception cref="ArgumentException">Als dataUrl ongeldig is</exception>
        public string GetBase64DataFromDataUrl(string dataUrl)
        {
            if (string.IsNullOrWhiteSpace(dataUrl))
                throw new ArgumentException("Data URL mag niet leeg zijn", nameof(dataUrl));

            if (!dataUrl.Contains(','))
                throw new ArgumentException("Ongeldige data URL formaat - moet een komma bevatten", nameof(dataUrl));

            var parts = dataUrl.Split(',');
            if (parts.Length != 2)
                throw new ArgumentException("Ongeldige data URL formaat - verwacht 'data:mime;base64,{data}'", nameof(dataUrl));

            return parts[1];
        }

        /// <summary>
        /// Interne methode voor QR code generatie
        /// </summary>
        private string GenerateQRCodeInternal(string qrData)
        {
            if (string.IsNullOrWhiteSpace(qrData))
                throw new ArgumentException("QR data mag niet leeg zijn", nameof(qrData));

            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, _errorCorrectionLevel))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrCodeImage = qrCode.GetGraphic(_qrCodePixelsPerModule);
                    string base64String = Convert.ToBase64String(qrCodeImage);
                    return $"data:image/png;base64,{base64String}";
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(
                    $"Fout bij genereren QR-code voor data '{qrData}': {ex.Message}",
                    ex);
            }
        }
    }
}