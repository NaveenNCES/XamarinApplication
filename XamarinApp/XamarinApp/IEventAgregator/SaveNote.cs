using Prism.Events;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace XamarinApp.IEventAgregator
{
  public class SaveNote
  {
    //private ObservableCollection<string> _savedNote = new ObservableCollection<string>();

    public ObservableCollection<string> SavedNotes { get; set; }

    public SaveNote(IEventAggregator ea)
    {
      ea.GetEvent<NoteSentEvent>().Subscribe(NotesReceived);
    }

    public void NotesReceived(string parameter)
    {
      SavedNotes.Add(parameter);
    }
  }
}
