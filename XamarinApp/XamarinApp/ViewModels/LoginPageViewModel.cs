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
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
    public class LoginPageViewModel : ViewModelBase
    {
        private readonly INavigationService Navigation;
        private readonly IPageDialogService _pageDialogService;
        private readonly IUserLoginService _userLoginService;

        public ICommand LoginCommand { get; set; }
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
        public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,IUserLoginService userLoginService)
        {
            Navigation = navigationService;
            _pageDialogService = pageDialogService;
            _userLoginService = userLoginService;
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked()
        {
            var user = new UserModel { Password = PassWord, UserName = UserName };

            var result = _userLoginService.LoginUser(user);

            if (result == true)
            {
                await Navigation.NavigateAsync("MainPage");
            }
            else
            {
                await _pageDialogService.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK");
            }

        }


    }
}
