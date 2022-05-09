using AutoFixture;
using Xamarin.Forms;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class GesturePageViewModelTest
  {
    private readonly GesturePageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();

    public GesturePageViewModelTest()
    {
      viewModel = new GesturePageViewModel();
    }

    [Fact]
    public void Will_Return_TapCount_as_String()
    {
      //Arrange
      var fixture = _fixture.Create<SwipeDirection>();
      
      //Act
      viewModel.TapCommand.Execute(new object());

      //Assert
      Assert.Equal(1, viewModel.TapCount);
    }

    [Fact]
    public void Will_Return_Gesture_as_String()
    {
      //Arrange
      var fixture = _fixture.Create<SwipeDirection>();
      string expectedResult = $"Swip {fixture}";
      //Act
      viewModel.TapCommand.Execute(fixture);

      //Assert
      Assert.Equal(expectedResult, viewModel.CountLable);
    }
  }
}
