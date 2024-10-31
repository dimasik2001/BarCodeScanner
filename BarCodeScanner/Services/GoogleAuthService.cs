
using BarCodeScanner.Models;
using BarCodeScanner.Services.Abstractions;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text.Json;

namespace BarCodeScanner.Services
{
    public class GoogleAuthService : IGoogleAuthService
    {
        private const string _userInfoUrl = "https://www.googleapis.com/oauth2/v1/userinfo?alt=json";
        private const string _tokenUrl = "https://oauth2.googleapis.com/token";
        private readonly string _clientId = "264544973391-gigq14f5j1kovpkml285ic8c07lp1f77.apps.googleusercontent.com";
        private readonly string _redirectUri = @"com.googleusercontent.apps.264544973391-gigq14f5j1kovpkml285ic8c07lp1f77:/oauth2redirect";

        public async Task<Token> SignInWithGoogleAsync()
        {
            var authorizationUrl = $"https://accounts.google.com/o/oauth2/v2/auth?" +
                                   $"client_id={_clientId}&" +
                                   $"redirect_uri={_redirectUri}&" +
                                   $"response_type=code&" +
                                   $"scope=openid%20email%20profile&" +
                                   $"access_type=offline";

            var authCode = await StartAuthenticationInWebViewAsync(authorizationUrl);

            return await ExchangeCodeForTokenAsync(authCode);
        }



        private string ExtractCodeFromUrl(string url)
        {
            var uri = new Uri(url);
            var query = uri.Query.Substring(1);
            var codeParam = query.Split('&').FirstOrDefault(p => p.StartsWith("code="));
            return codeParam?.Split('=')[1];
        }

        public async Task<Token> ExchangeCodeForTokenAsync(string code)
        {
            using var client = new HttpClient();

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenUrl)
            {
                Content = new FormUrlEncodedContent(
                [
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("redirect_uri", _redirectUri),
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code)
                ])
            };

            var response = await client.SendAsync(tokenRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to refresh token: {response.ReasonPhrase}");
            }
            var payload = await response.Content.ReadAsStringAsync();
            
            return JsonConvert.DeserializeObject<Token>(payload);
        }

        public async Task<User?> GetUserInfoAsync(string accessToken)
        {
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);

            var response = await client.GetAsync(_userInfoUrl);
            if (response.IsSuccessStatusCode)
            {
                var payload = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<User>(payload);
            }
            return null;
        }

        public async Task<Token> RefreshTokenAsync(string refreshToken)
        {
            using var client = new HttpClient();

            var tokenRequest = new HttpRequestMessage(HttpMethod.Post, _tokenUrl)
            {
                Content = new FormUrlEncodedContent(
                [
                new KeyValuePair<string, string>("client_id", _clientId),
                new KeyValuePair<string, string>("refresh_token", refreshToken),
                new KeyValuePair<string, string>("grant_type", "refresh_token")
                ])
            };

            var response = await client.SendAsync(tokenRequest);
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to refresh token: {response.ReasonPhrase}");
            }

            var payload = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Token>(payload);
        }
        private async Task<string> StartAuthenticationInWebViewAsync(string authorizationUrl)
        {
            var tcs = new TaskCompletionSource<string>();
            var webView = new WebView
            {
                Source = authorizationUrl,
                UserAgent = "Mozilla/5.0 (compatible; MyApp)"
            };

            webView.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith(_redirectUri))
                {
                    var code = ExtractCodeFromUrl(e.Url);
                    tcs.SetResult(code);
                }
            };

            var page = new ContentPage { Content = webView };
            await Application.Current.MainPage.Navigation.PushModalAsync(page);
            var authCode = await tcs.Task;
            await Application.Current.MainPage.Navigation.PopModalAsync();

            return authCode;
        }
    }
}
