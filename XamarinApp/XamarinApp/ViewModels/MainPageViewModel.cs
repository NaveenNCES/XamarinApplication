using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private readonly INavigationService Navigation;
        public ICommand LoginCommand { get; set; }

        public MainPageViewModel(INavigationService navigationService)            
        {
            Navigation = navigationService;
            LoginCommand = new Command(OnLoginClicked);
        }

        private async void OnLoginClicked()
        {
            await Navigation.NavigateAsync("LoginPage");
        }
    }
}
