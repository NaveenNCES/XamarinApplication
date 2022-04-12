using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class SignUpPageViewModelTest
  {
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<ISignUpUserService> _signUpUserService; 
    private readonly SignUpPageViewModel viewModel;

    public SignUpPageViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _signUpUserService = new Mock<ISignUpUserService>();
      viewModel = new SignUpPageViewModel(_navigationService.Object, _pageDialogService.Object, _signUpUserService.Object);
    }

    [Fact]
    public void When_User_Click_SignIn_Navigate_to_LoginPage()
    {
      //Act
      _navigationService.Setup(n => n.NavigateAsync("LoginPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(n => n.NavigateAsync("LoginPage"));
    }

    [Fact]
    public void When_User_Enters_Valid_Email_and_Password_Navigate_to_LoginPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      viewModel.ConfirmPassWord = user.Password;
      _navigationService.Setup(n => n.NavigateAsync("LoginPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.SignUpCommand.Execute();

      //Assert
      _navigationService.Verify(n => n.NavigateAsync("LoginPage"));
    }

    [Fact]
    public void When_User_Enters_InValid_Email_and_Password_Show_Alert()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      viewModel.ConfirmPassWord = user.Password;
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.SignUpCommand.Execute();

      //Assert
      _pageDialogService.Verify(n => n.DisplayAlertAsync("Failed", "Please enter valid Details", "OK"));
    }
  }
}
