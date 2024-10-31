using BarCodeScanner.Models;
using BarCodeScanner.Services.Abstractions;
using BarCodeScanner.ViewModels.Base;
using CommunityToolkit.Maui.Alerts;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ZXing;

namespace BarCodeScanner.ViewModels
{

    public class BarcodeScannerViewModel : BaseViewModel
    {
        private readonly IBarcodeScannerService _barcodeScannerService;
        private readonly IStorageService _storageService;

        public BarcodeScannerViewModel(IBarcodeScannerService barcodeScannerService, IStorageService storageService)
        {
            _barcodeScannerService = barcodeScannerService;
            _storageService = storageService;
        }

        public ObservableCollection<BarcodeInfo> ScannedCodes { get; set; } = new();


        public ICommand CopyCommand { get => new Command<string>(async (t) => await CopyAsync(t)); }
        private async Task CopyAsync(string value)
        {
            await Clipboard.SetTextAsync(value);
            Toast.Make("Value copied").Show();
        }

        public ICommand RemoveCommand { get => new Command<BarcodeInfo>(async (x) => await Remove(x)); }
        private async Task Remove(BarcodeInfo item)
        {
            var userAnswer = await Shell.Current.DisplayActionSheet($"Delete {item.Value}?", "OK", "Cancel");
            if (userAnswer != "OK")
            {
                return;
            }
            ScannedCodes.Remove(item);
            _storageService.Remove<BarcodeInfo>(item.Id.ToString());
        }

        public ICommand OpenUrlCommand { get => new Command<string>(async (value) => await OpenUrlAsync(value)); }
        private async Task OpenUrlAsync(string value)
        {
            bool result = Uri.TryCreate(value, UriKind.Absolute, out var uriResult)
                && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
            if (result)
            {
                await Launcher.OpenAsync(uriResult);

            }
        }

        public ICommand ScanBarcodeCommand { get => new Command(async () => await ScanBarcodeAsync()); }

        public async Task ScanBarcodeAsync()
        {
            var result = await _barcodeScannerService.ScanBarcodeAsync();
            foreach (var item in result)
            {
                item.Id = Guid.NewGuid();
                await _storageService.SetAsync(item.Id.ToString(), item);
                ScannedCodes.Add(item);
            }
        }
        public override async Task InitializeAsync()
        {
            var resultDictionary = await _storageService.GetAllAsync<BarcodeInfo>();
            foreach (var kvp in resultDictionary)
            {
                ScannedCodes.Add(kvp.Value);
            }
        }
    }
}
