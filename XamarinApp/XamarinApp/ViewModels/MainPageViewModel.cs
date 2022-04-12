using Module1.ViewModels;
using Prism.Commands;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Input;
using Windows.UI.Xaml;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Resx;

namespace XamarinApp.ViewModels
{
  public class MainPageViewModel : ViewModelBase, INavigatedAware
  {
    private readonly IModuleManager _moduleManager;
    private readonly INavigationService Navigation;
    private readonly IPageDialogService _pageDialogService;
    public DelegateCommand LoginCommand { get; set; }
    public DelegateCommand ApiCommand { get; set; }
    public DelegateCommand GestureCommand { get; set; }
    public DelegateCommand ChangeLanguageCommand { get; set; }
    public DelegateCommand EventAggregatorCommand { get; set; }
    public DelegateCommand ModuleCommand { get; set; }
    public DelegateCommand EssentialCommand { get; set; }

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
    /////////Language/////////
    private ObservableCollection<MyLanguage> _supportedLanguage;

    public ObservableCollection<MyLanguage> SupportedLanguage
    {
      get { return _supportedLanguage; }
      set { SetProperty(ref _supportedLanguage, value); }
    }

    private MyLanguage _selectedLanguage;
    public MyLanguage SelectedLanguage
    {
      get { return _selectedLanguage; }
      set { SetProperty(ref _selectedLanguage, value); }
    }
    //////////////////////
    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IModuleManager moduleManager)
    {
      Navigation = navigationService;
      _pageDialogService = pageDialogService;
      _moduleManager = moduleManager;
      LoginCommand = new DelegateCommand(OnLoginClicked);
      ApiCommand = new DelegateCommand(OnApiClicked);
      GestureCommand = new DelegateCommand(OnGestureClicked);
      ModuleCommand = new DelegateCommand(OnModuleClicked);
      EventAggregatorCommand = new DelegateCommand(OnEventClicked);
      ChangeLanguageCommand = new DelegateCommand(PerformOperation);
      EssentialCommand = new DelegateCommand(EssentialClicked);
      ///////Language//////
      SupportedLanguage = new ObservableCollection<MyLanguage>()
      {
        new MyLanguage{Name=AppResource.English,CI="en"},
        new MyLanguage{Name=AppResource.Spanish,CI="es"},
        new MyLanguage{Name=AppResource.French,CI="fr"},
        new MyLanguage{Name=AppResource.Tamil,CI="ta"}
      };

      SelectedLanguage = SupportedLanguage.FirstOrDefault(x => x.CI == LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName);
    }

    private void EssentialClicked()
    {
      Navigation.NavigateAsync("XamarinEssentials");
    }

    private async void OnModuleClicked()
    {
      _moduleManager.LoadModule("Module1Module");
      await Navigation.NavigateAsync("ViewA");
    }

    private void PerformOperation()
    {
      //CultureInfo[] cultures = CultureInfo.GetCultures(CultureTypes.UserCustomCulture | CultureTypes.SpecificCultures);
      //var getCulture = CultureInfo.CurrentUICulture.Name;

      LocalizationResourceManager.Current.SetCulture(CultureInfo.GetCultureInfo(SelectedLanguage.CI));
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
