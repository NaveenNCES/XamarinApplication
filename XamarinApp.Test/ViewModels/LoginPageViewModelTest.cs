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
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class LoginPageViewModelTest
  {
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IUserLoginService> _loginService;
    private readonly Fixture _fixture = new Fixture();
    private readonly LoginPageViewModel viewModel;

    public LoginPageViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _loginService = new Mock<IUserLoginService>();
      viewModel = new LoginPageViewModel(_navigationService.Object, _pageDialogService.Object, _loginService.Object);
    }

    [Fact]
    public void GIven_Valid_Userdetail_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      
      viewModel.LoginCommand.Execute(new());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("MainPage"));
    }

    [Fact]
    public void GIven_InValid_Userdetail_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      _navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(user)).ReturnsAsync(false);
      viewModel.LoginCommand.Execute(new());

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void GIven_InValid_UserName_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.PassWord = user.Password;
      _navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == "" && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.LoginCommand.Execute(new());

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void GIven_InValid_Password_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      _navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == ""))).ReturnsAsync(false);
      viewModel.LoginCommand.Execute(new());

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void GIven_Empty_Username_and_Password__Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == "" && x.Password == ""))).ReturnsAsync(false);
      viewModel.LoginCommand.Execute(new());

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void GIven_Valid_Userdetail_InValid_Password_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == "2000"))).ReturnsAsync(false);
      viewModel.UserName = user.UserName;
      viewModel.PassWord = "2000";

      viewModel.LoginCommand.Execute(new());

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void GIven_InValid_Userdetail_Valid_Password_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      //_navigationService.Setup(x => x.NavigateAsync("MainPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      //_pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == "naveen" && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.UserName = "naveen";
      viewModel.PassWord = user.Password;

      viewModel.LoginCommand.Execute(new());

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void When_User_Click_SignUp_Navigate_to_SignUpPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      //_navigationService.Setup(x => x.NavigateAsync("SignUpPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
      viewModel.SignUPCommand.Execute(new());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("SignUpPage"));
      ;
    }
  }
}
