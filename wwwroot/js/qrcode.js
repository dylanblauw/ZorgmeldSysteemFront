//// QR Code JavaScript functies voor ZorgmeldSysteem
//console.log('QR Code JavaScript geladen');

//// Download functie voor QR-code
//window.downloadQRCode = function (base64Data, filename) {
//    console.log('downloadQRCode aangeroepen');

//    try {
//        const byteCharacters = atob(base64Data);
//        const byteNumbers = new Array(byteCharacters.length);

//        for (let i = 0; i < byteCharacters.length; i++) {
//            byteNumbers[i] = byteCharacters.charCodeAt(i);
//        }

//        const byteArray = new Uint8Array(byteNumbers);
//        const blob = new Blob([byteArray], { type: 'image/png' });

//        const url = window.URL.createObjectURL(blob);
//        const link = document.createElement('a');
//        link.href = url;
//        link.download = filename;

//        document.body.appendChild(link);
//        link.click();

//        document.body.removeChild(link);
//        window.URL.revokeObjectURL(url);

//        console.log('QR Code gedownload:', filename);
//    } catch (error) {
//        console.error('Fout bij downloaden QR code:', error);
//    }
//};

//// Print functie voor QR-code
//window.printQRCode = function (base64Image) {
//    console.log('printQRCode aangeroepen');

//    try {
//        const printWindow = window.open('', '_blank');

//        if (!printWindow) {
//            alert('Pop-up geblokkeerd! Sta pop-ups toe voor deze website.');
//            return;
//        }

//        printWindow.document.write(`
//            <!DOCTYPE html>
//            <html>
//            <head>
//                <title>Print QR-Code</title>
//                <style>
//                    body {
//                        margin: 0;
//                        padding: 20px;
//                        display: flex;
//                        justify-content: center;
//                        align-items: center;
//                        min-height: 100vh;
//                    }
//                    img {
//                        max-width: 500px;
//                        width: 100%;
//                        height: auto;
//                    }
//                    @media print {
//                        body {
//                            padding: 0;
//                        }
//                        img {
//                            max-width: 100%;
//                        }
//                    }
//                </style>
//            </head>
//            <body>
//                <img src="${base64Image}" alt="QR Code" onload="window.print();" />
//            </body>
//            </html>
//        `);

//        printWindow.document.close();
//        console.log('Print venster geopend');
//    } catch (error) {
//        console.error('Fout bij printen QR code:', error);
//    }
//};

//console.log('QR Code functies beschikbaar:', {
//    downloadQRCode: typeof window.downloadQRCode,
//    printQRCode: typeof window.printQRCode
//});