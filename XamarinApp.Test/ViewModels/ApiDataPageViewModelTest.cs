using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using Xunit;

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

      //Act
      _randomApiService.Setup(x => x.getRandomApiData()).ReturnsAsync(apiData);
      viewModel.ApiCommand.Execute(new());

      //Assert
      Assert.Equal(apiData, viewModel.ApiData);
    }

    [Fact]
    public void Will_Navigate_to_DetailedPage()
    {
      //Arrange
      var apiData = _fixture.Build<ApiModel.Result>().CreateMany(50).ToList();
      var list = apiData.Where(x => x ==  apiData.FirstOrDefault()).ToList();
      var specificData = list.FirstOrDefault();

      //Act
      viewModel.ApiData = apiData;
      viewModel.getSpecificData = specificData;
      viewModel.ItemTappedCommand.Execute(new());

      //Assert
      _navigationService.Verify(x => x.NavigateAsync("SelectedItemDetailPage", It.Is<NavigationParameters>(x => x.ContainsKey("selectedData"))));
    }
  }
}
