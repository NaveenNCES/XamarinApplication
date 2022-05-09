using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;
using XamarinApp.Resx;
using XamarinApp.PageName;
using System;

namespace XamarinApp.Test.ViewModels
{
  public class LoginPageViewModelTest
  {    
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<ILoginService> _loginService;
    private readonly Mock<XamarinApp.Services.Interfaces.ILogger> _logger ;
    private readonly Fixture _fixture = new Fixture();
    private readonly LoginPageViewModel viewModel;
    private readonly Mock<ILogManager> _logManager;
    private readonly Mock<IGoogleManager> _googleManager;
    NLogManagerService log = new NLogManagerService();

    public LoginPageViewModelTest()
    {
      Xamarin.Forms.Mocks.MockForms.Init();
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _loginService = new Mock<ILoginService>();
      _logger = new Mock<ILogger>();
      _logManager = new Mock<ILogManager>();
      _googleManager = new Mock<IGoogleManager>();
      var a = log.GetLog();
      _logManager.Setup(x => x.GetLog(It.IsAny<string>())).Returns(a);
      viewModel = new LoginPageViewModel(_navigationService.Object, _pageDialogService.Object, _loginService.Object,
        _logManager.Object,_googleManager.Object);
    }

    [Fact]
    public void GIven_Valid_Userdetail_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;
      
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey("Name"))));
    }

    [Fact]
    public void Checking_MessageCenter_Sent_message_or_Not()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      var name = _fixture.Create<string>();
      bool messageSent = false;
      MessagingCenter.Subscribe<LoginPageViewModel, string>(this, AppResource.MessageCenterKey, (sender, args) =>
      {
        messageSent = true;
      });

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;

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

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;

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
      viewModel.Password = user.Password;
      _loginService.Setup(x => x.LoginUserAsync(user)).ReturnsAsync(false);
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK"));
    }

    [Fact]
    public void GIven_InValid_UserName_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.Password = user.Password;
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == "" && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK"));
    }

    [Fact]
    public void GIven_InValid_Password_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == ""))).ReturnsAsync(false);
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK"));
    }

    [Fact]
    public void GIven_Empty_Username_and_Password__Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == "" && x.Password == ""))).ReturnsAsync(false);
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK"));
    }

    [Fact]
    public void GIven_Valid_Userdetail_InValid_Password_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == "2000"))).ReturnsAsync(false);
      viewModel.UserName = user.UserName;
      viewModel.Password = "2000";

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK"));
    }

    [Fact]
    public void GIven_InValid_Userdetail_Valid_Password_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == "naveen" && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.UserName = "naveen";
      viewModel.Password = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, "OK"));
    }

    [Fact]
    public void When_User_Click_SignUp_Navigate_to_SignUpPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No")).ReturnsAsync(true);
      viewModel.SignUPCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.SignUpPage));
    }

    [Fact]
    public void LoginService_Should_return_Exception()
    {
      //Arrange
      var errorMessage = _fixture.Create<string>();
      var user = _fixture.Create<UserModel>();

      //Act
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ThrowsAsync(new Exception(errorMessage));
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok));

    }

    [Fact]
    public void On_Google_Login_Clicked_should_Navigate_to_mainPage()
    {
      //Arrange
      var googleUser = _fixture.Create<GoogleUser>();
      var fixture = _fixture.Create<string>();

      //Act
      viewModel.OnLoginComplete(googleUser, fixture);
      viewModel.GoogleCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.MainPage));
    }
  }
}
