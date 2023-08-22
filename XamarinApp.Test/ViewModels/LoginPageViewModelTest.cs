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
using Prism.AppModel;
using System.Threading.Tasks;
using FluentAssertions;

namespace XamarinApp.Test.ViewModels
{
  public class LoginPageViewModelTest
  {
    private readonly MockRepository _mockRepository;
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<ILoginService> _loginService;
    private readonly Mock<ILogger> _logger ;
    private readonly Fixture _fixture = new Fixture();
    private readonly LoginPageViewModel viewModel;
    private readonly Mock<ILogManager> _logManager;
    private readonly Mock<IGoogleManager> _googleManager;
    //private readonly Mock<IMessagingCenter> _messagingCenter;

    public LoginPageViewModelTest()
    {
      Xamarin.Forms.Mocks.MockForms.Init();
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _navigationService = _mockRepository.Create<INavigationService>();
      _pageDialogService = _mockRepository.Create<IPageDialogService>();
      _loginService = _mockRepository.Create<ILoginService>();
      _logManager = _mockRepository.Create<ILogManager>();
      _logger = _mockRepository.Create<ILogger>();
      _googleManager = _mockRepository.Create<IGoogleManager>();
      //_messagingCenter = _mockRepository.Create<IMessagingCenter>();
      _logManager.Setup(x => x.GetLog(It.IsAny<string>())).Returns(_logger.Object);
 
      viewModel = new LoginPageViewModel(_navigationService.Object, _pageDialogService.Object, _loginService.Object,
        _logManager.Object,_googleManager.Object);
    }

    [Fact]
    public void GIven_Valid_Userdetail_Navigate_to_MainPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      _navigationService.Setup(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey(AppResource.Name)))).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayPromptAsync(AppResource.Question, AppResource.GetName, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(()=>"Name");
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _logger.Setup(x => x.Info(AppResource.Log_UserDetailsAreValidAndNavigating));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;
      
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey("Name"))));
      _pageDialogService.Verify(x => x.DisplayPromptAsync(AppResource.Question, AppResource.GetName, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _logger.Verify(x => x.Info(AppResource.Log_UserDetailsAreValidAndNavigating));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
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
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      _navigationService.Setup(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey(AppResource.Name)))).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _logger.Setup(x => x.Info(AppResource.Log_UserDetailsAreValidAndNavigating));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      messageSent.Should().Be(true);
      _navigationService.Verify(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey("Name"))));
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _logger.Verify(x => x.Info(AppResource.Log_UserDetailsAreValidAndNavigating));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Checking_MessageCenter_Not_Sent_message()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      bool messageSent = false;
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      _navigationService.Setup(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey(AppResource.Name)))).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _logger.Setup(x => x.Info(AppResource.Log_UserDetailsAreValidAndNavigating));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      messageSent.Should().Be(false);
      _navigationService.Verify(x => x.NavigateAsync(PageNames.MainPage, It.Is<NavigationParameters>(x => x.ContainsKey("Name"))));
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _logger.Verify(x => x.Info(AppResource.Log_UserDetailsAreValidAndNavigating));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void GIven_InValid_Userdetail_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok)).Returns(()=>Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void GIven_InValid_UserName_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == null && x.Password == user.Password))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok)).Returns(() => Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.Password = user.Password;
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void GIven_InValid_Password_Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == null))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok)).Returns(() => Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void GIven_Empty_Username_and_Password__Show_AlertBox()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == null && x.Password == null))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok)).Returns(() => Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void GIven_Valid_Userdetail_InValid_Password_Show_Alert()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == "2000"))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok)).Returns(() => Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = "2000";

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == "2000")));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void GIven_InValid_Userdetail_Valid_Password_Show_Alert()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == "naveen" && x.Password == user.Password))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok)).Returns(() => Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.UserName = "naveen";
      viewModel.Password = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidUser, AppResource.Ok));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == "naveen" && x.Password == user.Password)));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Click_SignUp_Navigate_to_SignUpPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _navigationService.Setup(x => x.NavigateAsync(PageNames.SignUpPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No")).ReturnsAsync(true);

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;
      viewModel.SignUPCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.SignUpPage));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No"));
      _logManager.Verify(x => x.GetLog(It.IsAny<string>()));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Click_SignUp_Navigate_to_SignUpPage_Throws_Exception()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      var errorMessage = _fixture.Create<string>();
      _navigationService.Setup(x => x.NavigateAsync(PageNames.SignUpPage)).Throws(new Exception(errorMessage));
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No")).ReturnsAsync(true);
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok)).Returns(() => Task.CompletedTask);

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;
      viewModel.SignUPCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.SignUpPage));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Request, AppResource.PageDialogRequest, "Yes", "No"));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok));
      _logManager.Verify(x => x.GetLog(It.IsAny<string>()));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void LoginService_Should_return_Exception()
    {
      //Arrange
      var errorMessage = _fixture.Create<string>();
      var user = _fixture.Create<UserModel>();
      _loginService.Setup(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ThrowsAsync(new Exception(errorMessage));
      _pageDialogService.Setup(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>())).ReturnsAsync(() => "Name");
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok)).Returns(() => Task.CompletedTask);
      _logger.Setup(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));

      //Act
      viewModel.UserName = user.UserName;
      viewModel.Password = user.Password;

      viewModel.LoginCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok));
      _pageDialogService.Verify(x => x.DisplayPromptAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<int>(), It.IsAny<KeyboardType>(), It.IsAny<string>()));
      _loginService.Verify(x => x.LoginUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _logger.Verify(x => x.Info(AppResource.Log_UserGivenDetailsArePassing));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void On_Google_Login_Clicked_should_Navigate_to_mainPage()
    {
      //Arrange
      var googleUser = _fixture.Create<GoogleUser>();
      var fixture = _fixture.Create<string>();
      _navigationService.Setup(x => x.NavigateAsync(PageNames.MainPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _googleManager.Setup(x => x.Login(viewModel.OnLoginComplete));

      //Act
      viewModel.OnLoginComplete(googleUser, fixture);
      viewModel.GoogleCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.MainPage));
      _googleManager.Verify(x => x.Login(viewModel.OnLoginComplete));
      _logManager.Verify(x => x.GetLog(It.IsAny<string>()));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
