using AutoFixture;
using FluentAssertions;
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
      viewModel.TapCount.Should().BeGreaterThan(0);
    }

    [Fact]
    public void Will_Return_Gesture_as_String_Right()
    {
      //Arrange
      var fixture = _fixture.Create<SwipeDirection>();
      string expectedResult = $"Swip {fixture}";

      //Act
      viewModel.TapCommand.Execute(fixture);

      //Assert
      viewModel.CountLable.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Will_Return_Gesture_as_String_Left()
    {
      //Arrange
      var fixture = _fixture.Create<SwipeDirection>();
      fixture = SwipeDirection.Left;
      string expectedResult = $"Swip {fixture}";

      //Act
      viewModel.TapCommand.Execute(fixture);

      //Assert
      viewModel.CountLable.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Will_Return_Gesture_as_String_Up()
    {
      //Arrange
      var fixture = _fixture.Create<SwipeDirection>();
      fixture = SwipeDirection.Up;
      string expectedResult = $"Swip {fixture}";

      //Act
      viewModel.TapCommand.Execute(fixture);

      //Assert
      viewModel.CountLable.Should().BeEquivalentTo(expectedResult);
    }

    [Fact]
    public void Will_Return_Gesture_as_String_Down()
    {
      //Arrange
      var fixture = _fixture.Create<SwipeDirection>();
      fixture = SwipeDirection.Down;
      string expectedResult = $"Swip {fixture}";
      //Act
      viewModel.TapCommand.Execute(fixture);

      //Assert
      viewModel.CountLable.Should().BeEquivalentTo(expectedResult);
    }
  }
}
