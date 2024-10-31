using BarCodeScanner.Models;
using BarCodeScanner.ViewModels.Base;
using System;
using System.Collections;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.ViewModels
{
    [QueryProperty(nameof(User), "user")]

    public class ProfileBarcodeScannerCompositeViewModel : BaseViewModel
    {
        public BarcodeScannerViewModel BarcodeScanner { get; }
        public ProfileViewModel Profile { get; }
        public User User
        {
            get => Profile.User;
            set
            {
                Profile.User = value;
            }
        }
        
        public ProfileBarcodeScannerCompositeViewModel(BarcodeScannerViewModel barcodeScannerViewModel, ProfileViewModel profileViewModel)
        {
            BarcodeScanner= barcodeScannerViewModel;
            Profile = profileViewModel;
            barcodeScannerViewModel.PropertyChanged += InnerViewModelPropertyChanged;
            profileViewModel.PropertyChanged += InnerViewModelPropertyChanged;
        }

        private void InnerViewModelPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            InvokePropertyChanged(sender, new PropertyChangedEventArgs(e.PropertyName));
        }
        public override async Task InitializeAsync()
        {
            await BarcodeScanner.InitializeAsync();
            await Profile.InitializeAsync();
        }
    }
}
