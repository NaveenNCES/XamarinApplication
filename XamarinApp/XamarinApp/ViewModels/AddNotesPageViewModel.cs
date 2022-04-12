using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XamarinApp.Composite_Command;
using XamarinApp.IEventAgregator;

namespace XamarinApp.ViewModels
{
  public class AddNotesPageViewModel : ViewModelBase
  {
    private IEventAggregator _eventaggregator;
    ////////////////SendingNote
    private string _notes;

    public string Notes
    {
      get { return _notes; }
      set{ SetProperty(ref _notes, value); }
    }

    public DelegateCommand SendNoteCommand { get; private set; }

    //////////////SavedNotes
    private ObservableCollection<string> _savedNote = new ObservableCollection<string>();

    public ObservableCollection<string> SavedNotes
    {
      get { return _savedNote; }
      set { SetProperty(ref _savedNote, value); }
    }
    /////////Composite Command
    private IApplicationCommand _applicationCommand;
    public IApplicationCommand ApplicationCommand
    {
      get { return _applicationCommand; }
      set { SetProperty(ref _applicationCommand, value); }
    }
    
    public AddNotesPageViewModel(IEventAggregator eventaggregator,IApplicationCommand applicationCommand)
    {
      _eventaggregator = eventaggregator;
      ApplicationCommand = applicationCommand;
      SendNoteCommand = new DelegateCommand(SendNote);
      ApplicationCommand.SaveAllCommand.RegisterCommand(SendNoteCommand);
      _eventaggregator.GetEvent<NoteSentEvent>().Subscribe(NotesReceived);
    }
   
    private void NotesReceived(string parameter)
    {
      SavedNotes.Add(parameter);
    }

    private void SendNote()
    {
      _eventaggregator.GetEvent<NoteSentEvent>().Publish(Notes);      
    }
  }
}
