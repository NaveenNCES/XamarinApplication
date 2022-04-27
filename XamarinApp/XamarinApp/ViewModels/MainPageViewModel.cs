using Acr.UserDialogs;
using Prism.Commands;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.PageName;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class MainPageViewModel : ViewModelBase
  {
    private readonly IModuleManager _moduleManager;
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly IUserDialogs _userDialogs;
    private readonly IGoogleManager _googleManager;

    public DelegateCommand LoginCommand { get; }
    public DelegateCommand ApiCommand { get; }
    public DelegateCommand GestureCommand { get;}
    public DelegateCommand ChangeLanguageCommand { get; }
    public DelegateCommand EventAggregatorCommand { get; }
    public DelegateCommand ModuleCommand { get; }
    public DelegateCommand EssentialCommand { get; }
    public DelegateCommand GoogleLogOutCommand { get; }

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
    public MainPageViewModel(INavigationService navigationService, IModuleManager moduleManager, IPageDialogService pageDialogService,IUserDialogs userDialogs,IGoogleManager googleManager)
    {
      _navigation = navigationService;
      _moduleManager = moduleManager;
      _pageDialogService = pageDialogService;
      _userDialogs = userDialogs;
      _googleManager = googleManager;
      LoginCommand = new DelegateCommand(async () => await OnLoginClicked());
      ApiCommand = new DelegateCommand(async () => await OnApiClicked());
      GestureCommand = new DelegateCommand(async () => await OnGestureClicked());
      ModuleCommand = new DelegateCommand(async () => await OnModuleClicked());
      EventAggregatorCommand = new DelegateCommand(async () => await OnEventClicked());
      ChangeLanguageCommand = new DelegateCommand(PerformOperation);
      EssentialCommand = new DelegateCommand(async () => await EssentialClicked());
      GoogleLogOutCommand = new DelegateCommand(GoogleLogoutClicked);
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

    private void GoogleLogoutClicked()
    {
      _googleManager.Logout();

      _navigation.NavigateAsync(PageNames.LoginPage);
    }

    private async Task EssentialClicked()
    {
      _userDialogs.Loading("Loading...");

      await _navigation.NavigateAsync(PageNames.XamarinEssentials);

      _userDialogs.HideLoading();
    }

    private async Task OnModuleClicked()
    {
      _moduleManager.LoadModule(PageNames.Module1);
      await _navigation.NavigateAsync(PageNames.ModuleViewA);
    }

    private void PerformOperation()
    {
      LocalizationResourceManager.Current.SetCulture(CultureInfo.GetCultureInfo(SelectedLanguage.CI));
    }

    private async Task OnGestureClicked()
    {
      await _navigation.NavigateAsync(PageNames.GesturePage);
    }

    private async Task OnEventClicked()
    {
      await _navigation.NavigateAsync(PageNames.AddNotesPage);       
    }

    private async Task OnApiClicked()
    {
      await _navigation.NavigateAsync(PageNames.ApiDataPage);
    }

    private async Task OnLoginClicked()
    {
      try
      {
        IsLoading = true;
        IndicatorVisible = true;

        await _navigation.NavigateAsync(PageNames.LoginPage);
      }
      catch (Exception ex)
      {
        await _pageDialogService.DisplayAlertAsync(AppResource.Alert, ex.Message, AppResource.Ok);
      }

      finally
      {
        IsLoading = false;
        IndicatorVisible = false;
      }      
    }
    
    public override void OnNavigatedTo(INavigationParameters parameters)
    {
        Name = parameters.GetValue<string>(AppResource.Name);

        MessagingCenter.Subscribe<LoginPageViewModel, string>(this, AppResource.MessageCenterKey, (sender, args) =>
        {
          Message = args;
        });
    }
  }
}
