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
    public ICommand ApiCommand { get; set; }
    public DelegateCommand EventAggregatorCommand { get; set; }

    public MainPageViewModel(INavigationService navigationService)
    {
      Navigation = navigationService;
      LoginCommand = new Command(OnLoginClicked);
      ApiCommand = new Command(OnApiClicked);
      EventAggregatorCommand = new DelegateCommand(OnEventClicked);
    }

    private async void OnEventClicked()
    {
      await Navigation.NavigateAsync("AddNotesPage");
    }

    private async void OnApiClicked()
    {
      await Navigation.NavigateAsync("ApiDataPage");
    }

    private async void OnLoginClicked()
    {
      await Navigation.NavigateAsync("LoginPage");
    }
  }
}
