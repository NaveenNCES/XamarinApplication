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
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class MainPageViewModelTest
  {
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IModuleManager> _module;
    private readonly Mock<IUserDialogs> _userDialogs;
    private readonly MainPageViewModel viewModel;
    private readonly Mock<IGoogleManager> _googleManager;
    private readonly Fixture _fixture = new Fixture();


    public MainPageViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _module = new Mock<IModuleManager>();
      _userDialogs = new Mock<IUserDialogs>();
      _googleManager = new Mock<IGoogleManager>();
      viewModel = new MainPageViewModel(_navigationService.Object,_module.Object,_pageDialogService.Object,_userDialogs.Object,_googleManager.Object);
    }

    [Fact]
    public void When_User_Click_SignIn_Navigate_to_SignInPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync(PageNames.LoginPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.LoginCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.LoginPage));
    }

    [Fact]
    public void When_User_Click_API_Navigate_to_ApiDataPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync(PageNames.ApiDataPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.ApiCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.ApiDataPage));
    }

    [Fact]
    public void When_User_Click_GesturePage_Navigate_to_GesturePage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync(PageNames.GesturePage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.GestureCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.GesturePage));
    }

    [Fact]
    public void When_User_Click_AddNotesPage_Navigate_to_AddNotesPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync(PageNames.AddNotesPage)).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.EventAggregatorCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.AddNotesPage));
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
    }

    [Fact]
    public void OnNavigatedto_Returns_MessagingCenter_Value()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      var data = (INavigationParametersInternal)new NavigationParameters();
      data.Add("__NavigationMode", NavigationMode.New);
      MessagingCenter.Subscribe<MainPageViewModelTest, string>(this, "Hi", (sender, args) =>
      {
        viewModel.Message = args;
      });

      //Act
      MessagingCenter.Send(this, "Hi", fixture);
      viewModel.OnNavigatedTo(data as INavigationParameters);

      //Arrange
      Assert.Equal(fixture, viewModel.Message);
    }

    [Fact]
    public void On_Module_Click_Navigate_to_ViewA()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync(PageNames.ModuleViewA)).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.ModuleCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.ModuleViewA));
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
    }

    [Fact]
    public void On_Xamarin_Essentials_Clicked_Should_Navigate_to_XamarinEssentials_Page()
    {
      //Act
      viewModel.EssentialCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.XamarinEssentials));
    }

    [Fact]
    public void On_GoogleLogoutClicked_should_navigate_to_LoginPage()
    {
      //Act
      viewModel.GoogleLogOutCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.LoginPage));
    }
  }
}
