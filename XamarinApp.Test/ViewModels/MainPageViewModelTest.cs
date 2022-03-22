using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class MainPageViewModelTest
  {
    private readonly Mock<INavigationService> _navigationService;
    private readonly MainPageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();


    public MainPageViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      viewModel = new MainPageViewModel(_navigationService.Object);
    }

    [Fact]
    public void When_User_Click_SignIn_Navigate_to_SignInPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync("LoginPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.LoginCommand.Execute(new());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("LoginPage"));
    }

    [Fact]
    public void When_User_Click_API_Navigate_to_ApiDataPage()
    {
      //Act
      _navigationService.Setup(x => x.NavigateAsync("ApiDataPage")).ReturnsAsync(_fixture.Create<NavigationResult>());
      viewModel.ApiCommand.Execute(new());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("ApiDataPage"));
    }
  }
}
