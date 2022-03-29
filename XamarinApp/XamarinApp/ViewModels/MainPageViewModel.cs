using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Windows.Input;
using Windows.UI.Xaml;
using Xamarin.Forms;

namespace XamarinApp.ViewModels
{
  public class MainPageViewModel : ViewModelBase, INavigatedAware
  {
    private readonly INavigationService Navigation;
    private readonly IPageDialogService _pageDialogService;
    public ICommand LoginCommand { get; set; }
    public ICommand ApiCommand { get; set; }
    public ICommand GestureCommand { get; set; }
    public DelegateCommand EventAggregatorCommand { get; set; }

    private string _name;
    public string Name
    {
      get { return _name; }
      set { SetProperty(ref _name, value); }
    }

    public List<string> Demo { get; set; } = new List<string>();

    private string _message;
    public string Message
    {
      get { return _message; }
      set { SetProperty(ref _message, value); }
    }



    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
    {
      Navigation = navigationService;
      _pageDialogService = pageDialogService;
      LoginCommand = new Command(OnLoginClicked);
      ApiCommand = new Command(OnApiClicked);
      GestureCommand = new Command(OnGestureClicked);
      EventAggregatorCommand = new DelegateCommand(OnEventClicked);
      CultureInfo culture = new CultureInfo("ja-JP");
      Thread.CurrentThread.CurrentCulture = culture;
      Thread.CurrentThread.CurrentUICulture = culture;
    }

    private async void OnGestureClicked()
    {
      await Navigation.NavigateAsync("GesturePage");
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

    public void OnNavigatedFrom(INavigationParameters parameters)
    {     
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
      Name = parameters.GetValue<string>("Name");

      MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "Hi", (sender, args) =>
      {
        Message = args;
      });
    }
  }
}
