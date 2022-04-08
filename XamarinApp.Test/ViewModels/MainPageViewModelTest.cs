using AutoFixture;
using Moq;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Forms;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class MainPageViewModelTest
  {
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IModuleManager> _module;
    private readonly MainPageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();


    public MainPageViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _module = new Mock<IModuleManager>();
      viewModel = new MainPageViewModel(_navigationService.Object,_pageDialogService.Object,_module.Object);
    }

    [Fact]
    public void When_User_Click_SignIn_Navigate_to_SignInPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync("LoginPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.LoginCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("LoginPage"));
    }

    [Fact]
    public void When_User_Click_API_Navigate_to_ApiDataPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync("ApiDataPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.ApiCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("ApiDataPage"));
    }

    [Fact]
    public void When_User_Click_GesturePage_Navigate_to_GesturePage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync("GesturePage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.GestureCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("GesturePage"));
    }

    [Fact]
    public void When_User_Click_AddNotesPage_Navigate_to_AddNotesPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync("AddNotesPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.EventAggregatorCommand.Execute();

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("AddNotesPage"));
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
      var data = new NavigationParameters();
      MessagingCenter.Subscribe<MainPageViewModelTest, string>(this, "Hi", (sender, args) =>
      {
        viewModel.Message = args;
      });

      //Act
      MessagingCenter.Send(this, "Hi", fixture);
      viewModel.OnNavigatedTo(data);

      //Arrange
      Assert.Equal(fixture, viewModel.Message);
    }

    [Fact]
    public void On_Module_Click_Navigate_to_ViewA()
    {
      //Arrange
      _navigationService.Setup(x => x.NavigateAsync("ViewA")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.ModuleCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("ViewA"));
    }

    [Fact]
    public void On_Language_change_click_will_change_language()
    {
      //Arrange
      viewModel.SelectedLanguage = viewModel.SupportedLanguage.FirstOrDefault(x => x.CI == "ta");

      //Act
      viewModel.ChangeLanguageCommand.Execute(new object());
      var result = LocalizationResourceManager.Current.CurrentCulture.TwoLetterISOLanguageName;

      //Assert
      Assert.Equal("ta", result);
    }
  }
}
