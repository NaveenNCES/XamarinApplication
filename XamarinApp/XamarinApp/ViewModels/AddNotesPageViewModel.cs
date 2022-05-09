using Prism.Commands;
using Prism.Events;
using System.Collections.ObjectModel;
using XamarinApp.Composite_Command;
using XamarinApp.IEventAgregator;

namespace XamarinApp.ViewModels
{
  public class AddNotesPageViewModel : ViewModelBase
  {
    private IEventAggregator _eventaggregator;
    private ObservableCollection<string> _savedNote = new ObservableCollection<string>();
    private string _notes;
    private IApplicationCommand _applicationCommand;
    public DelegateCommand SendNoteCommand { get; private set; }

    /// <summary>
    /// SendingNote
    /// </summary>
    public string Notes
    {
      get { return _notes; }
      set{ SetProperty(ref _notes, value); }
    }

    /// <summary>
    /// SavedNotes
    /// </summary>
    public ObservableCollection<string> SavedNotes
    {
      get { return _savedNote; }
      set { SetProperty(ref _savedNote, value); }
    }

    /// <summary>
    /// Composite Command
    /// </summary>

    public IApplicationCommand ApplicationCommand
    {
      get { return _applicationCommand; }
      set { SetProperty(ref _applicationCommand, value); }
    }
    
    public AddNotesPageViewModel(IEventAggregator eventaggregator,IApplicationCommand applicationCommand)
    {
      _eventaggregator = eventaggregator;
      _applicationCommand = applicationCommand;
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
