using BarCodeScanner.Models;
using BarCodeScanner.Services;
using BarCodeScanner.Services.Abstractions;

namespace BarCodeScanner
{
    public partial class MainPage : ContentPage
    {
        private readonly IStorageService _storageService;
        private readonly IGoogleAuthService _authService;

        public MainPage([FromKeyedServices("secure")] IStorageService storageService, IGoogleAuthService authService)
        {
            
            InitializeComponent();
            _storageService = storageService;
            _authService = authService;
        }

        protected override async void OnNavigatedTo(NavigatedToEventArgs args)
        {
            try
            {
                var user = await RetrieveUserDataAsync();
                if (user != null)
                {

                    await Shell.Current.GoToAsync("///workspace", new Dictionary<string, object> { ["user"] = user });

                }
                else
                {
                    await Shell.Current.GoToAsync("///login");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.GoToAsync("///workspace");
                await Shell.Current.DisplayAlert("Conection Error", ex.Message, "OK");
            }
           
            base.OnNavigatedTo(args);
        }

        async Task<User> RetrieveUserDataAsync()
        {
            var token = await _storageService.GetAsync<Token>("token");
            if (token == null)
            {
                return null;
            }
            var user = await _authService.GetUserInfoAsync(token.AccessToken);
            if (user == null)
            {
               var newToken = await _authService.RefreshTokenAsync(token.RefreshToken);
               user = await _authService.GetUserInfoAsync(newToken.AccessToken);
            }
            return user;
        }
    }

}
