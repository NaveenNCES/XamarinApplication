using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.PageName;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class LoginPageViewModel : ViewModelBase
  {    
    private readonly ILogger _logger;
    private readonly INavigationService _navigationService;
    private readonly IPageDialogService _pageDialogService;
    private readonly ILoginService _userLoginService;

    public DelegateCommand LoginCommand { get; }
    public DelegateCommand SignUPCommand { get; }
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
    public string Password
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
      ILoginService userLoginService, ILogManager logManager)
    {
      _navigationService = navigationService;
      _pageDialogService = pageDialogService;
      _userLoginService = userLoginService;
      _logger = logManager.GetLog();
      LoginCommand = new DelegateCommand(async() =>await OnLoginClicked());
      SignUPCommand = new DelegateCommand(async() =>await OnSignUpClicked());
    }

    private async Task OnSignUpClicked()
    {
      try
      {
        IsLoading = true;
        IndicatorVisible = true;
        var answer = await _pageDialogService.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No");
        if (answer)
        {
          await _navigationService.NavigateAsync(PageNames.SignUpPage);          
        }
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
    public async Task OnLoginClicked()
    {
      try
      {
        IsLoading = true;
        IndicatorVisible = true;

        _logger.Info("User given details are passing");

        Name = await _pageDialogService.DisplayPromptAsync(AppResource.Question, AppResource.GetName);

        var user = new UserModel { Password = Password, UserName = UserName };

        var result = await _userLoginService.LoginUserAsync(user);

        var data = new NavigationParameters();
        data.Add(AppResource.Name, Name);

        if (result == true)
        {
          _logger.Info("User given details are valid and navigating to MainPage");

          await _navigationService.NavigateAsync(PageNames.MainPage, data);
          MessagingCenter.Send<LoginPageViewModel, string>(this, AppResource.MessageCenterKey, Name);          
        }
        else
        {
          await _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok);
        }
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
  }
}
