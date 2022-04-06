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
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class LoginPageViewModel : ViewModelBase
  {
    private static Services.Interfaces.ILogger deplogger = DependencyService.Get<ILogManager>().GetLog();
    private readonly ILogManager _logManager;
    private readonly Services.Interfaces.ILogger _logger;
    private readonly INavigationService Navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly IUserLoginService _userLoginService;

    public ICommand LoginCommand { get; set; }
    public ICommand SignUPCommand { get; set; }
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
      Navigation = navigationService;
      _pageDialogService = pageDialogService;
      _userLoginService = userLoginService;
      _logger = logManager.GetLog();
      _logManager = logManager;
      LoginCommand = new Command(OnLoginClicked);
      SignUPCommand = new Command(OnSignUpClicked);
    }

    private async void OnSignUpClicked()
    {
      var answer = await _pageDialogService.DisplayAlertAsync("Request", "Do You want to Sign Up", "Yes", "No");
      if(answer)
        await Navigation.NavigateAsync("SignUpPage");
    }
    public async void OnLoginClicked()
    {
      deplogger.Info("na");
      _logger.Info("Logging");

      Name = await _pageDialogService.DisplayPromptAsync("Question", "Whats ur name");

      var user = new UserModel { Password = PassWord, UserName = UserName };

      var result = await _userLoginService.LoginUser(user);

      var data = new NavigationParameters();
      data.Add("Name", Name);

      if (result == true)
      {
        await Navigation.NavigateAsync("MainPage", data);
        MessagingCenter.Send<LoginPageViewModel, string>(this, "Hi", "Naveen");
      }
      else
      {
        await _pageDialogService.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK");
      }
    }
  }
}
