using AutoFixture;
using FluentAssertions;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
    private readonly MockRepository _mockRepository;
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly SelectedItemDetailPageViewModel viewModel;
    private readonly Mock<IPhoneDialer> _phoneDialor;
    private readonly Mock<IEmail> _email;
    private readonly Fixture _fixture = new Fixture();

    public SelectedItemViewModelTest()
    {
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _navigationService = _mockRepository.Create<INavigationService>();
      _pageDialogService = _mockRepository.Create<IPageDialogService>();
      _phoneDialor = _mockRepository.Create<IPhoneDialer>();
      _email = _mockRepository.Create<IEmail>();
      viewModel = new SelectedItemDetailPageViewModel(_navigationService.Object, _pageDialogService.Object,_phoneDialor.Object,_email.Object);
    }

    [Fact]
    public void Should_Get_Navigation_ParameterValues()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();

      //Act
      var data = _fixture.Create<NavigationParameters>();
      data.Add(NavigationKeys.selectedData, fixture);
      viewModel.OnNavigatedTo(data);

      var result = viewModel.getSelectedData;

      //Assert
      result.Should().BeEquivalentTo(fixture);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnEmail_Clicked_Should_Compose_Email()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();
      var email = _fixture.Create<EmailMessage>();
      _email.Setup(x => x.ComposeAsync(email)).Returns(()=>Task.CompletedTask);

      //Act
      var data = _fixture.Create<NavigationParameters>();
      data.Add(NavigationKeys.selectedData, fixture);
      viewModel.OnNavigatedTo(data);
      viewModel.EmailCommand.Execute();

      //Arrange
      _email.Verify(x => x.ComposeAsync(email), Times.Once);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnPhoneDialer_clicked()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();
      _phoneDialor.Setup(x => x.Open(fixture.FirstOrDefault().Phone));

      //Act
      var data = _fixture.Create<NavigationParameters>();
      data.Add(NavigationKeys.selectedData, fixture);
      viewModel.OnNavigatedTo(data);
      viewModel.PhoneDialerCommand.Execute();

      //Assert
      _phoneDialor.Verify(x => x.Open(viewModel.getSelectedData.FirstOrDefault().Phone),Times.Once);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
