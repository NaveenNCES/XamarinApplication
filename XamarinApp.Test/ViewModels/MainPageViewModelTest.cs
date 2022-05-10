using Acr.UserDialogs;
using AutoFixture;
using Moq;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using System.Linq;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using XamarinApp.Models;
using XamarinApp.PageName;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class MainPageViewModelTest
  {
    private readonly MockRepository _mockRepository;
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IModuleManager> _module;
    private readonly Mock<IUserDialogs> _userDialogs;
    private readonly MainPageViewModel viewModel;
    private readonly Mock<IGoogleManager> _googleManager;
    private readonly Fixture _fixture = new Fixture();


    public MainPageViewModelTest()
    {
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _navigationService = _mockRepository.Create<INavigationService>();
      _pageDialogService = _mockRepository.Create<IPageDialogService>();
      _module = _mockRepository.Create<IModuleManager>();
      _userDialogs = _mockRepository.Create<IUserDialogs>();
      _googleManager = _mockRepository.Create<IGoogleManager>();
      viewModel = new MainPageViewModel(_navigationService.Object,_module.Object,_pageDialogService.Object,_userDialogs.Object,_googleManager.Object);
    }

    [Fact]
    public void When_User_Click_SignIn_Navigate_to_SignInPage()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.LoginPage));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Click_API_Navigate_to_ApiDataPage()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.ApiDataPage)).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.ApiCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.ApiDataPage));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Click_GesturePage_Navigate_to_GesturePage()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.GesturePage)).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.GestureCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.GesturePage));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void When_User_Click_AddNotesPage_Navigate_to_AddNotesPage()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.AddNotesPage)).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.EventAggregatorCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.AddNotesPage));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnNavigationParameter_will_return_Name_of_the_User()
    {
      //Arrange
      var Name = _fixture.Create<string>();
      var data = new NavigationParameters();
      data.Add("Name", Name);

      //Act
      viewModel.OnNavigatedTo(data);

      //Assert
      Assert.Equal(Name, viewModel.Name);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnNavigatedto_Returns_MessagingCenter_Value()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      var data = (INavigationParametersInternal)new NavigationParameters();
      data.Add("__NavigationMode", NavigationMode.New);
      MessagingCenter.Subscribe<MainPageViewModelTest, string>(this, AppResource.MessageCenterKey, (sender, args) =>
      {
        viewModel.Message = args;
      });

      //Act
      MessagingCenter.Send(this, AppResource.MessageCenterKey, fixture);
      viewModel.OnNavigatedTo(data as INavigationParameters);

      //Arrange
      Assert.Equal(fixture, viewModel.Message);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void On_Module_Click_Navigate_to_ViewA()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.ModuleViewA)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _module.Setup(x => x.LoadModule(PageNames.Module1));

      //Act
      viewModel.ModuleCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.ModuleViewA));
      _module.Verify(x => x.LoadModule(PageNames.Module1));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void On_Language_change_click_will_change_language()
    {
      //Arrange
      viewModel.SelectedLanguage = viewModel.SupportedLanguage.FirstOrDefault(x => x.CI == "ta");

      //Act
      viewModel.ChangeLanguageCommand.Execute();
      var result = LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName;

      //Assert
      Assert.Equal("ta", result);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void On_Xamarin_Essentials_Clicked_Should_Navigate_to_XamarinEssentials_Page()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.XamarinEssentials)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _userDialogs.Setup(x => x.ShowLoading("Loading...", null));
      _userDialogs.Setup(x => x.HideLoading());

      //Act
      viewModel.EssentialCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.XamarinEssentials));
      _userDialogs.Verify(x => x.ShowLoading("Loading...", null));
      _userDialogs.Verify(x => x.HideLoading());
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void On_GoogleLogoutClicked_should_navigate_to_LoginPage()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      _googleManager.Setup(x => x.Logout());

      //Act
      viewModel.GoogleLogOutCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.LoginPage));
      _googleManager.Verify(x => x.Logout());
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void On_Renderer_Button_Clicked_Should_Navigate_to_CustomRenderer_Page()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.CameraPage)).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.RendererCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.CameraPage));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
