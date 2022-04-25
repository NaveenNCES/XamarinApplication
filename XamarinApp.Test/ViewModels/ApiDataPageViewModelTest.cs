using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
    private readonly Mock<INavigationService> _navigationService;
    private readonly Mock<IPageDialogService> _pageDialogService;
    private readonly Mock<IRandomApiService> _randomApiService;
    private readonly ApiDataPageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();

    public ApiDataPageViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      _randomApiService = new Mock<IRandomApiService>();
      viewModel = new ApiDataPageViewModel(_navigationService.Object, _pageDialogService.Object, _randomApiService.Object);
    }

    [Fact]
    public void Will_Save_RandomApiService_Data_to_ApiData()
    {
      //Arrange
      var apiData = _fixture.Build<ApiModel.Result>().CreateMany(50).ToList();
      var data = (INavigationParametersInternal)new NavigationParameters();
      data.Add("__NavigationMode", NavigationMode.New);
      //Act
      _randomApiService.Setup(x => x.GetRandomApiDataAsync()).ReturnsAsync(apiData);
      viewModel.OnNavigatedTo(data as INavigationParameters);

      //Assert
      Assert.Equal(apiData, viewModel.ApiData);
    }

    [Fact]
    public void Will_Navigate_to_DetailedPage()
    {
      //Arrange
      var apiData = _fixture.Build<ApiModel.Result>().CreateMany(50).ToList();
      var dataNavigationPrameter = (INavigationParametersInternal)new NavigationParameters();
      ObservableCollection<Result> data = new ObservableCollection<Result>(apiData as List<Result>);
      var list = apiData.Where(x => x ==  apiData.FirstOrDefault()).ToList();
      var specificData = list.FirstOrDefault();
      dataNavigationPrameter.Add("__NavigationMode", NavigationMode.New);

      //Act
      _randomApiService.Setup(x => x.GetRandomApiDataAsync()).ReturnsAsync(apiData);
      viewModel.OnNavigatedTo(dataNavigationPrameter as INavigationParameters);
      viewModel.ItemTappedCommand.Execute(new object());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync(PageNames.SelectedItemDetailPage, It.Is<NavigationParameters>(x => x.ContainsKey("selectedData"))));
    }

    [Fact]
    public void OnAppearing_should_display_alert()
    {
      //Act
      viewModel.OnAppearing();

      //Assert
      _pageDialogService.Verify(x => x.DisplayAlertAsync(AppResource.Alert, AppResource.ApiDataDisplaying, AppResource.Ok));
    }
  }
}
