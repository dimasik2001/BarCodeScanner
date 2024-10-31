
using BarCodeScanner.Models;

namespace BarCodeScanner.Services.Abstractions
{
    public interface IBarcodeScannerService
    {
        Task<IEnumerable<BarcodeInfo>> ScanBarcodeAsync();
    }
}