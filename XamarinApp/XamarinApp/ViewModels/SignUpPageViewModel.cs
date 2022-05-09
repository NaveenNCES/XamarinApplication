using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.PageName;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class SignUpPageViewModel : ViewModelBase
  {
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly ISignUpUserService _signUpUserService;

    public DelegateCommand SignUpCommand { get; }
    public DelegateCommand LoginCommand { get; }
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
      SignUpCommand = new DelegateCommand(async () => await OnSignUpClicked());
      LoginCommand = new DelegateCommand(async () => await OnSignInClicked());
    }
    private async Task OnSignInClicked()
    {
      await _navigation.NavigateAsync(PageNames.LoginPage);
    }
    private async Task OnSignUpClicked()
    {
      if(PassWord != ConfirmPassWord)
      {
        await _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.PasswordNotMatching, AppResource.Ok);
      }
      else
      {
        var user = new UserModel { Password = PassWord, UserName = UserName };

        var result = await _signUpUserService.SaveUserAsync(user);

        if(result != false)
        {
          await _navigation.NavigateAsync(PageNames.LoginPage);
        }
        else
        {
          await _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.InValidCredential, AppResource.Ok);
        }
      }   
    }
  }
}
