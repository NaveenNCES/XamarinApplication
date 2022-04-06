using AutoFixture;
using Moq;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
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
    private readonly Fixture _fixture = new Fixture();

    public SelectedItemViewModelTest()
    {
      _navigationService = new Mock<INavigationService>();
      _pageDialogService = new Mock<IPageDialogService>();
      viewModel = new SelectedItemDetailPageViewModel(_navigationService.Object, _pageDialogService.Object);
    }

    [Fact]
    public void Should_Get_Navigation_ParameterValues()
    {
      //Arrange
      var fixture = _fixture.Build<Result>().CreateMany(1).ToList();

      //Act
      var data = new NavigationParameters();
      data.Add("selectedData", fixture);
      viewModel.OnNavigatedTo(data);

      var result = viewModel.getSelectedData;

      //Assert
      Assert.Equal(fixture, result);
    }

  }
}
