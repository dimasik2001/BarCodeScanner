using BarCodeScanner.ViewModels;

namespace BarCodeScanner.Views;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginViewModel vm)
	{
		BindingContext = vm;
		InitializeComponent();
	}
}