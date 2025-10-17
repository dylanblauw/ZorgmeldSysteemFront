using QRCoder;

namespace ZorgmeldSysteem.Blazor.Services.Business
{
    public class QRCodeService
    {
        public string GeneratePreviewQRCode(string objectCode)
        {
            if (string.IsNullOrWhiteSpace(objectCode))
                throw new ArgumentException("Object code mag niet leeg zijn");

            string qrData = $"PREVIEW:{objectCode}";
            return GenerateQRCodeInternal(qrData);
        }

        public string GenerateObjectQRCode(int objectId)
        {
            if (objectId <= 0)
                throw new ArgumentException("Object ID moet groter dan 0 zijn");

            string qrData = $"OBJECT:{objectId}";
            return GenerateQRCodeInternal(qrData);
        }

        public string GetBase64DataFromDataUrl(string dataUrl)
        {
            if (string.IsNullOrWhiteSpace(dataUrl) || !dataUrl.Contains(','))
                throw new ArgumentException("Ongeldige data URL");

            return dataUrl.Split(',')[1];
        }

        private string GenerateQRCodeInternal(string qrData)
        {
            try
            {
                using (QRCodeGenerator qrGenerator = new QRCodeGenerator())
                using (QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrData, QRCodeGenerator.ECCLevel.Q))
                using (PngByteQRCode qrCode = new PngByteQRCode(qrCodeData))
                {
                    byte[] qrCodeImage = qrCode.GetGraphic(20);
                    return $"data:image/png;base64,{Convert.ToBase64String(qrCodeImage)}";
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Fout bij genereren QR-code: {ex.Message}", ex);
            }
        }
    }
}