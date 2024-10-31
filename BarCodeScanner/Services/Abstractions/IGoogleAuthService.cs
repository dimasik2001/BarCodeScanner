using BarCodeScanner.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BarCodeScanner.Services.Abstractions
{
    public interface IGoogleAuthService
    {
        Task<User?> GetUserInfoAsync(string accessToken);
        Task<Token> RefreshTokenAsync(string refreshToken);
        Task<Token> SignInWithGoogleAsync();
    }
}
