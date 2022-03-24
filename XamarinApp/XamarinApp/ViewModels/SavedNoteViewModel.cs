using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using XamarinApp.IEventAgregator;

namespace XamarinApp.ViewModels
{
  public class SavedNoteViewModel : ViewModelBase
  {
    private IEventAggregator _ea;
    private ObservableCollection<string> _savedNote = new ObservableCollection<string>();

    public ObservableCollection<string> SavedNotes
    {
      get { return _savedNote; }
      set { SetProperty(ref _savedNote, value); }
    }
    public SavedNoteViewModel(IEventAggregator ea)
    {
      ea.GetEvent<NoteSentEvent>().Subscribe(NotesReceived);
    }

    private void NotesReceived(string parameter)
    {
      SavedNotes.Add(parameter);
    }
  }
}
