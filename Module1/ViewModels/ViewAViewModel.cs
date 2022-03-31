using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Module1.ViewModels
{
  public class ViewAViewModel : BindableBase
  {
    private string _title;
    public string Title
    {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }

    private bool _canUpdate = true;
    public bool CanUpdate
    {
      get { return _canUpdate; }
      set { SetProperty(ref _canUpdate, value); }
    }

    private string _updatedText;
    public string UpdatedText
    {
      get { return _updatedText; }
      set { SetProperty(ref _updatedText, value); }
    }
    public DelegateCommand UpdateCommand { get; private set; }
    public ViewAViewModel()
    {
      Title = "View A";
      UpdateCommand = new DelegateCommand(Update);
    }

    private void Update()
    {
      UpdatedText = $"Updated: {DateTime.Now}";
    }
  }
}
