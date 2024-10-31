using BarCodeScanner.ViewModels;
using BarCodeScanner.ViewModels.Base;

namespace BarCodeScanner.Views;

public partial class ScannerPage : ContentPage
{
	public ScannerPage(ProfileBarcodeScannerCompositeViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
    protected override async void OnBindingContextChanged()
    {
        base.OnBindingContextChanged();
        await (BindingContext as BaseViewModel)?.InitializeAsync();
    }
}