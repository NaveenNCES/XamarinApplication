using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.PageName;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.Test.ViewModels
{
  public class ApiDataPageViewModelTest
  {
    private readonly MockRepository _mockRepository;
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IRandomApiService> _randomApiService;
    private readonly ApiDataPageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();

    public ApiDataPageViewModelTest()
    {
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _navigationService = _mockRepository.Create<INavigationService>();
      _pageDialogService = _mockRepository.Create<IPageDialogService>();
      _randomApiService = _mockRepository.Create<IRandomApiService>();
      viewModel = new ApiDataPageViewModel(_navigationService.Object, _pageDialogService.Object, _randomApiService.Object);
    }

    [Fact]
    public void Will_Save_RandomApiService_Data_to_ApiData()
    {
      //Arrange
      var apiData = _fixture.Build<ApiModel.Result>().CreateMany(50).ToList();
      var data = (INavigationParametersInternal)new NavigationParameters();
      data.Add("__NavigationMode", NavigationMode.New);
      _randomApiService.Setup(x => x.GetRandomApiDataAsync()).ReturnsAsync(apiData);

      //Act
      viewModel.OnNavigatedTo(data as INavigationParameters);

      //Assert
      Assert.Equal(apiData, viewModel.ApiData);
      _randomApiService.Verify(x => x.GetRandomApiDataAsync());
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Will_Navigate_to_DetailedPage()
    {
      //Arrange
      var apiData = _fixture.Build<ApiModel.Result>().CreateMany(50).ToList();
      var dataParameter = _fixture.Create<NavigationParameters>();
      dataParameter.Add(NavigationKeys.selectedData, apiData);
      var dataNavigationPrameter = (INavigationParametersInternal)new NavigationParameters();
      ObservableCollection<Result> data = new ObservableCollection<Result>(apiData as List<Result>);
      var list = apiData.Where(x => x ==  apiData.FirstOrDefault()).ToList();
      var specificData = list.FirstOrDefault();
      dataNavigationPrameter.Add("__NavigationMode", NavigationMode.New);
      _randomApiService.Setup(x => x.GetRandomApiDataAsync()).ReturnsAsync(apiData);
      _navigationService.Setup(x => x.NavigateAsync(PageNames.SelectedItemDetailPage, It.Is<NavigationParameters>(x => x.ContainsKey(NavigationKeys.selectedData)))).ReturnsAsync(_fixture.Create<NavigationResult>());

      //Act
      viewModel.OnNavigatedTo(dataNavigationPrameter as INavigationParameters);
      viewModel.ItemTappedCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.SelectedItemDetailPage, It.Is<NavigationParameters>(x => x.ContainsKey("selectedData"))));
      _randomApiService.Verify(x => x.GetRandomApiDataAsync());
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Should_Throw_Exception_in_Navigation_SelectedItemDetailPage()
    {
      //Arrange
      var apiData = _fixture.Build<ApiModel.Result>().CreateMany(50).ToList();
      var errorMessage = _fixture.Create<string>();
      var dataParameter = _fixture.Create<NavigationParameters>();
      dataParameter.Add(NavigationKeys.selectedData, apiData);
      var dataNavigationPrameter = (INavigationParametersInternal)new NavigationParameters();
      ObservableCollection<Result> data = new ObservableCollection<Result>(apiData as List<Result>);
      var list = apiData.Where(x => x ==  apiData.FirstOrDefault()).ToList();
      var specificData = list.FirstOrDefault();
      dataNavigationPrameter.Add("__NavigationMode", NavigationMode.New);
      _randomApiService.Setup(x => x.GetRandomApiDataAsync()).ReturnsAsync(apiData);
      _navigationService.Setup(x => x.NavigateAsync(PageNames.SelectedItemDetailPage, It.Is<NavigationParameters>(x => x.ContainsKey(NavigationKeys.selectedData)))).Throws(new Exception(errorMessage));
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok)).Returns(() => Task.CompletedTask);
      //Act
      viewModel.OnNavigatedTo(dataNavigationPrameter as INavigationParameters);
      viewModel.ItemTappedCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.SelectedItemDetailPage, It.Is<NavigationParameters>(x => x.ContainsKey("selectedData"))));
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, errorMessage, AppResource.Ok));
      _randomApiService.Verify(x => x.GetRandomApiDataAsync());
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnAppearing_should_display_alert()
    {
      //Arrange
      _pageDialogService.Setup(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.ApiDataDisplaying, AppResource.Ok)).Returns(()=>null);

      //Act
      viewModel.OnAppearing();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.ApiDataDisplaying, AppResource.Ok));
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
