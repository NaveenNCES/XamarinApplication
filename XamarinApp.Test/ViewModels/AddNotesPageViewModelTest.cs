using AutoFixture;
using Moq;
using Prism.Commands;
using Prism.Events;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamarinApp.Composite_Command;
using XamarinApp.IEventAgregator;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class AddNotesPageViewModelTest
  {
    private readonly Mock<IApplicationCommand> _applicationCommand;
    private readonly Mock<IEventAggregator> _eventAgregator;
    private readonly Mock<NoteSentEvent> mockEvent;
    private readonly AddNotesPageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();
    private readonly CompositeCommand compositeCommand = new CompositeCommand();

    public AddNotesPageViewModelTest()
    {
      _applicationCommand = new Mock<IApplicationCommand>();
      _eventAgregator = new Mock<IEventAggregator>();
      mockEvent = new Mock<NoteSentEvent>();
      CompositeCommand composite = _fixture.Create<CompositeCommand>();
      _applicationCommand.Setup(x => x.SaveAllCommand).Returns(composite);      
      _eventAgregator.Setup(x => x.GetEvent<NoteSentEvent>()).Returns(mockEvent.Object);
      viewModel = new AddNotesPageViewModel(_eventAgregator.Object, _applicationCommand.Object);      
    }

    [Fact]
    public void Will_User_GivenNotes_Published()
    {
      //Arrange
      var fixture = _fixture.Create<string>();

      //Act
      viewModel.Notes = fixture;
      viewModel.SendNoteCommand.Execute();

      //Assert
      mockEvent.Verify(x => x.Publish(viewModel.Notes), Times.Once);
    }

    [Fact]
    public void Check_Composite_Command_Call()
    {
      //Arrange
      var fixture = _fixture.Create<string>();

      //Act
      viewModel.Notes = fixture;
      viewModel.ApplicationCommand.SaveAllCommand.Execute(new object());

      //Assert
      mockEvent.Verify(x => x.Publish(viewModel.Notes), Times.Once);
    }

    [Fact]
    public void Will_User_GivenNotes_Subscribed()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      //var mockedEvent = new Mock<NoteSentEvent>();
      var expectedAction = new Action<object>(obj => { });
      //Action<object> receivedEvent = null;

      //Act
      var executedDelegates = new List<string>();
      var actionDelegateReference =
          new Mock<NoteSentEvent>((Action<object>)delegate { executedDelegates.Add("Action"); });

      var filterDelegateReference = new Mock<NoteSentEvent>((Predicate<object>)delegate
      {
        executedDelegates.Add(
            "Filter");
        return true;
      });
      var a = new EventSubscription<object>((IDelegateReference)actionDelegateReference, (IDelegateReference)filterDelegateReference.Object);
      var publishAction = a.GetExecutionStrategy();
      Assert.NotNull(publishAction);

      publishAction.Invoke(null);

      Assert.Equal(2, executedDelegates.Count);
      Assert.Equal("Filter", executedDelegates[0]);
      Assert.Equal("Action", executedDelegates[1]);
      
    }
  }
}
