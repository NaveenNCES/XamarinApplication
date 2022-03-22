using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class SignUpPageViewModel : BindableBase
  {
    private readonly INavigationService Navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly ISignUpUserService _signUpUserService;

    public ICommand SignUpCommand { get; set; }
    public ICommand LoginCommand { get; set; }
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
      Navigation = navigationService;
      _pageDialogService = pageDialogService;
      _signUpUserService = signUpUserService;
      SignUpCommand = new Command(OnSignUpClicked);
      LoginCommand = new Command(OnSignInClicked);
    }
    private async void OnSignInClicked()
    {
      await Navigation.NavigateAsync("LoginPage");
    }
    private async void OnSignUpClicked()
    {
      if(PassWord != ConfirmPassWord)
      {
        await _pageDialogService.DisplayAlertAsync("Failed", "Password and Confirm Password doesn't match", "OK");
      }
      else
      {
        var user = new UserModel { Password = PassWord, UserName = UserName };

        var result = await _signUpUserService.SaveUser(user);

        if(result != false)
        {
          await Navigation.NavigateAsync("LoginPage");
        }
        else
        {
          await _pageDialogService.DisplayAlertAsync("Failed", "Please enter valid Details", "OK");
        }
      }   
    }
  }
}
