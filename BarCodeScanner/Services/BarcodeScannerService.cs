using System.Collections.Immutable;
using BarCodeScanner.Models;
using BarCodeScanner.Services.Abstractions;
using ZXing.Net.Maui;


namespace BarCodeScanner.Services
{
    public class BarcodeScannerService : IBarcodeScannerService
    {
        public async Task<IEnumerable<BarcodeInfo>> ScanBarcodeAsync()
        {
            var tcs = new TaskCompletionSource<IEnumerable<BarcodeResult>>();

            var scannerPage = new ContentPage
            {
                Content = new ZXing.Net.Maui.Controls.CameraBarcodeReaderView
                {
                    HorizontalOptions = LayoutOptions.Fill,
                    VerticalOptions = LayoutOptions.Fill
                }
            };

            var barcodeReader = (ZXing.Net.Maui.Controls.CameraBarcodeReaderView)scannerPage.Content;
            barcodeReader.Options = new BarcodeReaderOptions
            {
                Formats = BarcodeFormats.All,
                AutoRotate = true,
                Multiple = true
            };

            barcodeReader.BarcodesDetected += (s, e) =>
            {
                if (e.Results.Any())
                {
                    tcs.TrySetResult(e.Results);

                }
            };

            await Application.Current.MainPage.Navigation.PushModalAsync(scannerPage);

            var result = await tcs.Task;

            await Application.Current.MainPage.Navigation.PopModalAsync();

            return result.Select(x => new BarcodeInfo
            {
                Raw = x.Raw,
                Format = x.Format.ToString(),
                PointsOfInterest = x.PointsOfInterest,
                Value = x.Value,
                Metadata = x.Metadata.ToImmutableDictionary(x => x.Key.ToString(), x => x.Value)
            });
        }
    }
}
