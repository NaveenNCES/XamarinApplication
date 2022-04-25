using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
using XamarinApp.PageName;
using XamarinApp.ViewModels;
using Xunit;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.Test
{
  public class SelectedItemViewModelTest
  {
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly SelectedItemDetailPageViewModel viewModel;
    private readonly Mock<IPhoneDialer> _phoneDialor;
    private readonly Mock<IEmail> _email;
    private readonly Fixture _fixture = new Fixture();

    public SelectedItemViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _phoneDialor = new Mock<IPhoneDialer>();
      _email = new Mock<IEmail>();
      viewModel = new SelectedItemDetailPageViewModel(_navigationService.Object, _pageDialogService.Object,_phoneDialor.Object,_email.Object);
    }

    [Fact]
    public void Should_Get_Navigation_ParameterValues()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();

      //Act
      var data = new NavigationParameters();
      data.Add(NavigationKeys.selectedData, fixture);
      viewModel.OnNavigatedTo(data);

      var result = viewModel.getSelectedData;

      //Assert
      Assert.Equal(fixture, result);
    }

    [Fact]
    public void OnEmail_Clicked_Should_Compose_Email()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();

      //Act
      var data = new NavigationParameters();
      data.Add(NavigationKeys.selectedData, fixture);
      viewModel.OnNavigatedTo(data);
      viewModel.EmailCommand.Execute();

      //Arrange
      _email.Verify(x => x.ComposeAsync(It.IsAny<EmailMessage>()), Times.Once);
    }

    [Fact]
    public void OnPhoneDialer_clicked()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();

      //Act
      var data = new NavigationParameters();
      data.Add(NavigationKeys.selectedData, fixture);
      viewModel.OnNavigatedTo(data);
      viewModel.PhoneDialerCommand.Execute();

      //Assert
      _phoneDialor.Verify(x => x.Open(viewModel.getSelectedData.FirstOrDefault().Phone),Times.Once);
    }
  }
}
