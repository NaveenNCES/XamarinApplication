using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using XamarinApp.Models;
using XamarinApp.PageName;
using XamarinApp.Resx;
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
      _navigationService.Setup(n => n.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(n => n.NavigateAsync(PageNames.LoginPage));
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
      _navigationService.Setup(n => n.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);
      viewModel.SignUpCommand.Execute();

      //Assert
      _navigationService.Verify(n => n.NavigateAsync(PageNames.LoginPage));
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
      _pageDialogService.Verify(n => n.DisplayAlertAsync(AppResource.Alert, AppResource.InValidCredential, "OK"));
    }

    [Fact]
    public void on_diffeent_Password_and_confirmPassword_should_Show_an_alert()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      var fixture = _fixture.Create<string>();

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      viewModel.ConfirmPassWord = fixture;
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      viewModel.SignUpCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.PasswordNotMatching, AppResource.Ok));
    }
  }
}
