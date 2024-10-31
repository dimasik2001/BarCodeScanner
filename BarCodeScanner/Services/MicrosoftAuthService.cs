using BarCodeScanner.Models;
using BarCodeScanner.Services.Abstractions;
using Microsoft.Identity.Client;

namespace BarCodeScanner.Services
{
    public class MicrosoftAuthService : IMicrosoftAuthService
    {
        private readonly string _microsoftClientId = "b3850369-7f7e-4ae0-a5fa-cf2ce9989889";
        private readonly string _microsoftTenant = "https://dimasikandlizungmail.ciamlogin.com/";
        private readonly string _redirectUrl = "msalb3850369-7f7e-4ae0-a5fa-cf2ce9989889://auth";
        public async Task<User> SignInWithMicrosoftAsync()
        {
            var pca = PublicClientApplicationBuilder.Create(_microsoftClientId)
                .WithAuthority(_microsoftTenant)
                .WithRedirectUri(_redirectUrl)
                
                .Build();

            var scopes = new string[] { "openid", "offline_access" };

            AuthenticationResult authResult = null;

            try
            {
                var accounts = await pca.GetAccountsAsync();
                authResult = await pca.AcquireTokenSilent(scopes, accounts.FirstOrDefault())
                                      .ExecuteAsync().ConfigureAwait(false);
            }
            catch (MsalUiRequiredException)
            {
                authResult = await pca.AcquireTokenInteractive(scopes)
                    .WithParentActivityOrWindow(PlatformConfig.Instance.ParentWindow)
                                      .ExecuteAsync().ConfigureAwait(false);
            }

            if (authResult != null)
            {
                return new User
                {
                    Name = authResult.Account.Username,                 
                };
            }

            return null;
        }
    }
}
