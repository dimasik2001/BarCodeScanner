using BarCodeScanner.ViewModels;

namespace BarCodeScanner.Views;

public partial class ProfilePage : ContentPage
{
	public ProfilePage(ProfileViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}