using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Mocks;
using XamarinApp.Services;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;
using NLog;
using System.Globalization;

namespace XamarinApp.Test.ViewModels
{
  public class LoginPageViewModelTest
  {    
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IUserLoginService> _loginService;
    private readonly Mock<XamarinApp.Services.Interfaces.ILogger> _logger ;
    private readonly Fixture _fixture = new Fixture();
    private readonly LoginPageViewModel viewModel;
    private readonly Mock<ILogManager> _logManager;
    NLogManagerService log = new NLogManagerService();

    public LoginPageViewModelTest()
    {
      Xamarin.Forms.Mocks.MockForms.Init();
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _loginService = new Mock<IUserLoginService>();
      _logger = new Mock<XamarinApp.Services.Interfaces.ILogger>();
      _logManager = new Mock<ILogManager>();
      var a = log.GetLog();
      _logManager.Setup(x => x.GetLog(It.IsAny<string>())).Returns(a);
      viewModel = new LoginPageViewModel(_navigationService.Object, _pageDialogService.Object, _loginService.Object,
        _logger.Object,_logManager.Object);
    }

    [Fact]
    public void GIven_Valid_Userdetail_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("MainPage", It.Is<NavigationParameters>(x => x.ContainsKey("Name"))));
    }

    [Fact]
    public void Checking_MessageCenter_Sent_message_or_Not()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      bool messageSent = false;
      MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "Hi", (sender, args) =>
      {
        messageSent = true;
      });

      //Act
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      Assert.True(messageSent);
    }

    [Fact]
    public void Checking_MessageCenter_Not_Sent_message()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      bool messageSent = false;
      MessagingCenter.Subscribe<LoginPageViewModel, string>(this, "Hi", (sender, args) =>
      {
        messageSent = true;
      });

      //Act
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      Assert.False(messageSent);
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
      viewModel.LoginCommand.Execute();

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
      viewModel.LoginCommand.Execute();

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
      viewModel.LoginCommand.Execute();

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
      viewModel.LoginCommand.Execute();

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

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync("Failed", "Incorrect Username or Password", "OK"));
    }

    [Fact]
    public void GIven_InValid_Userdetail_Valid_Password_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUser(It.Is<UserModel>(x => x.UserName == "naveen" && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.UserName = "naveen";
      viewModel.PassWord = user.Password;

      viewModel.LoginCommand.Execute();

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
      viewModel.SignUPCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("SignUpPage"));
    }
  }
}
