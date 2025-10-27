// QR Code Scanner met ZXing
let codeReader = null;
let selectedDeviceId = null;
let dotNetHelper = null;

export async function startScanning(dotNetReference) {
    dotNetHelper = dotNetReference;

    try {
        // Dynamisch ZXing library laden
        if (typeof ZXing === 'undefined') {
            const script = document.createElement('script');
            script.src = 'https://unpkg.com/@zxing/library@latest';
            document.head.appendChild(script);
            
            await new Promise((resolve, reject) => {
                script.onload = resolve;
                script.onerror = () => reject(new Error('Kon ZXing library niet laden'));
            });
        }

        // BrowserMultiFormatReader gebruiken voor QR codes
        codeReader = new ZXing.BrowserQRCodeReader();

        // Lijst met beschikbare camera's ophalen
        const videoInputDevices = await codeReader.listVideoInputDevices();

        if (videoInputDevices.length === 0) {
            throw new Error('Geen camera gevonden');
        }

        // Bij voorkeur de achtercamera gebruiken (voor mobiel)
        selectedDeviceId = videoInputDevices.find(device => 
            device.label.toLowerCase().includes('back') || 
            device.label.toLowerCase().includes('rear')
        )?.deviceId || videoInputDevices[0].deviceId;

        // Start het scannen
        codeReader.decodeFromVideoDevice(selectedDeviceId, 'qr-video', (result, err) => {
            if (result) {
                // QR code succesvol gescand
                const code = result.text;
                dotNetHelper.invokeMethodAsync('OnQRCodeDetected', code);
                stopScanning(); // Stop automatisch na succesvolle scan
            }
            
            if (err && !(err instanceof ZXing.NotFoundException)) {
                console.error('Scan error:', err);
            }
        });

    } catch (error) {
        console.error('Error starting scanner:', error);
        if (dotNetHelper) {
            dotNetHelper.invokeMethodAsync('OnScanError', error.message);
        }
    }
}

export function stopScanning() {
    if (codeReader) {
        codeReader.reset();
        codeReader = null;
    }

    // Stop alle video streams
    const video = document.getElementById('qr-video');
    if (video && video.srcObject) {
        const tracks = video.srcObject.getTracks();
        tracks.forEach(track => track.stop());
        video.srcObject = null;
    }
}
