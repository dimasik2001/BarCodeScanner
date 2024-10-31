using BarCodeScanner.Models;
using BarCodeScanner.Services.Abstractions;
using BarCodeScanner.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace BarCodeScanner.ViewModels
{
    [QueryProperty(nameof(User), "user")]
    public class ProfileViewModel : BaseViewModel
    {
        private string _id;
        private string _email;
        private bool _verifiedEmail;
        private string _name;
        private string _givenName;
        private string _familyName;
        private string _picture;
        private readonly IStorageService _storageService;

        public ProfileViewModel([FromKeyedServices("secure")] IStorageService storageService)
        {
            _storageService = storageService;
        }

        private User _user;
        public User User
        {
            get => _user;
            set
            {
                _user = value;
                SetProps(value);
            }
        }

        private void SetProps(User value)
        {
            if (value is null)
            {
                HasUser = false;
                return;
            }
            Id = value.Id;
            Email = value.Email;
            VerifiedEmail = value.VerifiedEmail;
            Name = value.Name;
            GivenName = value.GivenName;
            FamilyName = value.FamilyName;
            Picture = value.Picture;
            HasUser = true;
        }

        public string Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        public bool VerifiedEmail
        {
            get => _verifiedEmail;
            set => SetProperty(ref _verifiedEmail, value);
        }

        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        public string GivenName
        {
            get => _givenName;
            set => SetProperty(ref _givenName, value);
        }

        public string FamilyName
        {
            get => _familyName;
            set => SetProperty(ref _familyName, value);
        }

        public string Picture
        {
            get => _picture;
            set => SetProperty(ref _picture, value);
        }



        private bool _hasUser;
        public bool HasUser
        {
            get => _hasUser;
            set { SetProperty(ref _hasUser, value); }
        }


        public string VerifiedEmailStatus => VerifiedEmail ? "Email Verified" : "Email Not Verified";

        public Color VerifiedEmailColor => VerifiedEmail ? Colors.Green : Colors.Red;

        public ICommand SignOutCommand { get => new Command(async () => await SignOut()); }
        private async Task SignOut()
        {
            _storageService.Remove<Token>("token");
            User = null;
            await Shell.Current.GoToAsync("///home");
        }
    }
}
