using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using XamarinApp.Models;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class SignUpPageViewModel : BindableBase
  {
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly ISignUpUserService _signUpUserService;

    public DelegateCommand SignUpCommand { get; set; }
    public DelegateCommand LoginCommand { get; set; }
    public UserModel _userModel { get; }
    private string _userName;
    private string _password;
    private string _confirmPassword;

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
    public string ConfirmPassWord
    {
      get
      {
        return _confirmPassword;
      }
      set
      {
        SetProperty(ref _confirmPassword, value);
      }
    }
    public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,ISignUpUserService signUpUserService)
    {
      _navigation = navigationService;
      _pageDialogService = pageDialogService;
      _signUpUserService = signUpUserService;
      SignUpCommand = new DelegateCommand(OnSignUpClicked);
      LoginCommand = new DelegateCommand(OnSignInClicked);
    }
    private async void OnSignInClicked()
    {
      await _navigation.NavigateAsync("LoginPage");
    }
    private async void OnSignUpClicked()
    {
      if(PassWord != ConfirmPassWord)
      {
        await _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.PasswordNotMatching, "OK");
      }
      else
      {
        var user = new UserModel { Password = PassWord, UserName = UserName };

        var result = await _signUpUserService.SaveUserAsync(user);

        if(result != false)
        {
          await _navigation.NavigateAsync("LoginPage");
        }
        else
        {
          await _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.InValidCredential, "OK");
        }
      }   
    }
  }
}
