using BarCodeScanner.Models;
using BarCodeScanner.Services.Abstractions;
using BarCodeScanner.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BarCodeScanner.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        private readonly IGoogleAuthService _authGoogleService;
        private readonly IMicrosoftAuthService _microsoftAuthService;
        private readonly IStorageService _storageService;

        public LoginViewModel(
            IGoogleAuthService authService, 
            IMicrosoftAuthService microsoftAuthService,
            [FromKeyedServices("secure")] IStorageService storageService)
        {
            _authGoogleService = authService;
            _microsoftAuthService = microsoftAuthService;
            _storageService = storageService;
        }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => SetProperty(ref _userName, value);
        }


        public ICommand SignInGoogleCommand { get => new Command(async () => await SignInGoogleAsync()); }

        private async Task SignInGoogleAsync()
        {
            try
            {
                var token = await _authGoogleService.SignInWithGoogleAsync();
                var userInfo = await _authGoogleService.GetUserInfoAsync(token.AccessToken);
                if (userInfo != null)
                {
                    await _storageService.SetAsync("token", token);
                    await Shell.Current.GoToAsync("///workspace", new Dictionary<string, object> { ["user"] = userInfo });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(ex.Message, "", "Ok");
            }

        } 
        public ICommand SignInMicrosoftCommand { get => new Command(async () => await SignInMicrosoftAsync()); }

        private async Task SignInMicrosoftAsync()
        {
            try
            {
                var userInfo = await _microsoftAuthService.SignInWithMicrosoftAsync();
                if (userInfo != null)
                {
                   // await _storageService.SetAsync("token", token);
                    await Shell.Current.GoToAsync("///profile", new Dictionary<string, object> { ["user"] = userInfo });
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert(ex.Message, "", "Ok");
            }

        }


        public ICommand SkipLoginCommand { get => new Command(async () => await SkipLoginAsync()); }
        private async Task SkipLoginAsync()
        {
            await Shell.Current.GoToAsync("///workspace");
        }
    }
}
