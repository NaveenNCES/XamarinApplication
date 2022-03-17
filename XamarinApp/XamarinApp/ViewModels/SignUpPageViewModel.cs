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

namespace XamarinApp.ViewModels
{
  public class SignUpPageViewModel : BindableBase
  {
    private readonly INavigationService Navigation;
    private readonly IPageDialogService _pageDialogService;
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
    public SignUpPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
    {
      Navigation = navigationService;
      _pageDialogService = pageDialogService;
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

        await App.Database.SaveUser(new UserModel
        {
          UserName = user.UserName,
          Password = user.Password,
        });

        await Navigation.NavigateAsync("LoginPage");
      }   

    }
  }
}
