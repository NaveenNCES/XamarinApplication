using NLog;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Resx;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class LoginPageViewModel : ViewModelBase
  {    
    private readonly ILogManager _logManager;
    private readonly Services.Interfaces.ILogger _logger;
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly IUserLoginService _userLoginService;

    public DelegateCommand LoginCommand { get; set; }
    public DelegateCommand SignUPCommand { get; set; }
    private string _userName;
    private string _password;

    private string _name;

    public string Name
    {
      get { return _name; }
      set { SetProperty(ref _name, value); }
    }

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
    public LoginPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,
      IUserLoginService userLoginService, Services.Interfaces.ILogger logger, ILogManager logManager)
    {
      _navigation = navigationService;
      _pageDialogService = pageDialogService;
      _userLoginService = userLoginService;
      _logger = logManager.GetLog();
      _logManager = logManager;
      LoginCommand = new DelegateCommand(OnLoginClicked);
      SignUPCommand = new DelegateCommand(OnSignUpClicked);
    }

    private async void OnSignUpClicked()
    {
      var answer = await _pageDialogService.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No");
      if (answer)
      {
         await _navigation.NavigateAsync("SignUpPage");
      }
    }
    public async void OnLoginClicked()
    {
      _logger.Info("User given details are passing");

      Name = await _pageDialogService.DisplayPromptAsync(AppResource.Question, AppResource.GetName);

      var user = new UserModel { Password = PassWord, UserName = UserName };

      var result = await _userLoginService.LoginUserAsync(user);

      var data = new NavigationParameters();
      data.Add("Name", Name);

      if (result == true)
      {
        _logger.Info("User given details are valid and navigating to MainPage");

        await _navigation.NavigateAsync("MainPage", data);
        MessagingCenter.Send<LoginPageViewModel, string>(this, "Hi", Name);
      }
      else
      {
        await _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK");
      }
    }
  }
}
