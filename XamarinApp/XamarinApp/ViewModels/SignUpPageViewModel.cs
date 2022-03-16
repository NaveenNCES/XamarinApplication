using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Models;

namespace XamarinApp.ViewModels
{
    public class SignUpPageViewModel : BindableBase
    {
        private readonly INavigationService Navigation;
        private readonly IPageDialogService _pageDialogService;
        public ICommand SignUpCommand { get; set; }
        public UserModel _userModel { get; }
        private string _userName;
        private string _password;

        public string UserName
        {
            get
            {
                return _userName;
            }
            set
            {
                SetProperty(ref _userName, value);
            }
        }
        public string PassWord
        {
            get
            {
                return _password;
            }
            set
            {
                SetProperty(ref _password, value);
            }
        }

        public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
        {
            Navigation = navigationService;
            _pageDialogService = pageDialogService;
            SignUpCommand = new Command(OnSignUpClicked);
        }

        private async void OnSignUpClicked()
        {
            var user = new UserModel { Password = PassWord, UserName = UserName };

            await App.Database.SaveUser(new UserModel
            {
                UserName = user.UserName,
                Password = user.Password,
            });

            Navigation.NavigateAsync("LoginPage");

        }
    }
}
