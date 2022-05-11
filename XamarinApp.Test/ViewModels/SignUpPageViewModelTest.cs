using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System.Threading.Tasks;
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
    private readonly MockRepository _mockRepository;
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<ISignUpUserService> _signUpUserService; 
    private readonly SignUpPageViewModel viewModel;

    public SignUpPageViewModelTest()
    {
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _navigationService = _mockRepository.Create<INavigationService>();
      _pageDialogService = _mockRepository.Create<IPageDialogService>();
      _signUpUserService = _mockRepository.Create<ISignUpUserService>();
      viewModel = new SignUpPageViewModel(_navigationService.Object, _pageDialogService.Object, _signUpUserService.Object);
    }

    [Fact]
    public void When_User_Click_SignIn_Navigate_to_LoginPage()
    {
      //Arrange
      _navigationService.Setup(n => n.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(n => n.NavigateAsync(PageNames.LoginPage));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Enters_Valid_Email_and_Password_Navigate_to_LoginPage()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _navigationService.Setup(n => n.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(true);

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      viewModel.ConfirmPassWord = user.Password;
      viewModel.SignUpCommand.Execute();

      //Assert
      _navigationService.Verify(n => n.NavigateAsync(PageNames.LoginPage));
      _signUpUserService.Verify(x => x.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Enters_InValid_Email_and_Password_Show_Alert()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.InValidCredential, AppResource.Ok)).Returns(() => Task.CompletedTask);

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      viewModel.ConfirmPassWord = user.Password;
      viewModel.SignUpCommand.Execute();
      
      //Assert
      _pageDialogService.Verify(n => n.DisplayAlertAsync(AppResource.Alert, AppResource.InValidCredential, AppResource.Ok));
      _signUpUserService.Verify(x => x.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password)));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void on_diffeent_Password_and_confirmPassword_should_Show_an_alert()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();
      var fixture = _fixture.Create<string>();
      _signUpUserService.Setup(n => n.SaveUserAsync(It.Is<UserModel>(x => x.UserName == user.UserName && x.Password == user.Password))).ReturnsAsync(false);
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.PasswordNotMatching, AppResource.Ok)).Returns(() => Task.CompletedTask);

      //Act
      viewModel.UserName = user.UserName;
      viewModel.PassWord = user.Password;
      viewModel.ConfirmPassWord = fixture;
      viewModel.SignUpCommand.Execute();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.PasswordNotMatching, AppResource.Ok));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
