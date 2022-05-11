using AutoFixture;
using FluentAssertions;
using Moq;
using Prism.Commands;
using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XamarinApp.Composite_Command;
using XamarinApp.IEventAgregator;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class AddNotesPageViewModelTest
  {
    private readonly MockRepository _mockRepository;
    private readonly Mock<IApplicationCommand> _applicationCommand;
    private readonly Mock<IEventAggregator> _eventAgregator;
    private readonly Mock<NoteSentEvent> mockEvent;
    private readonly AddNotesPageViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();

    public AddNotesPageViewModelTest()
    {
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _applicationCommand = _mockRepository.Create<IApplicationCommand>();
      _eventAgregator = _mockRepository.Create<IEventAggregator>();
      mockEvent = _mockRepository.Create<NoteSentEvent>();
      CompositeCommand composite = _fixture.Create<CompositeCommand>();
      _applicationCommand.Setup(x => x.SaveAllCommand).Returns(composite);      
      //_eventAgregator.Setup(x => x.GetEvent<NoteSentEvent>()).Returns(mockEvent.Object);
      var noteSentEventObject = _fixture.Create<NoteSentEvent>();
      _eventAgregator.Setup(x => x.GetEvent<NoteSentEvent>()).Returns(noteSentEventObject);
      viewModel = new AddNotesPageViewModel(_eventAgregator.Object, _applicationCommand.Object);      
    }

    [Fact]
    public void Will_User_GivenNotes_Published()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      _eventAgregator.Setup(x => x.GetEvent<NoteSentEvent>().Publish(fixture));

      //Act
      viewModel.Notes = fixture;
      viewModel.SendNoteCommand.Execute();

      //Assert
      _eventAgregator.Verify(x => x.GetEvent<NoteSentEvent>().Publish(fixture), Times.Once);
      _applicationCommand.Verify(x => x.SaveAllCommand);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Check_Composite_Command_Call()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      _eventAgregator.Setup(x => x.GetEvent<NoteSentEvent>().Publish(fixture));

      //Act
      viewModel.Notes = fixture;
      viewModel.ApplicationCommand.SaveAllCommand.Execute(new object());

      //Assert
      _eventAgregator.Verify(x => x.GetEvent<NoteSentEvent>().Publish(fixture), Times.Once);
      _applicationCommand.Verify(x => x.SaveAllCommand);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Will_User_GivenNotes_Subscribed()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      var expectedResult = _fixture.CreateMany<string>(1).ToList();

      //Act
      viewModel.Notes = expectedResult[0];
      viewModel.ApplicationCommand.SaveAllCommand.Execute(new object());

      //Assert
      viewModel.SavedNotes.Should().BeEquivalentTo(expectedResult);
      _eventAgregator.Verify(x => x.GetEvent<NoteSentEvent>());
      _applicationCommand.Verify(x => x.SaveAllCommand);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
