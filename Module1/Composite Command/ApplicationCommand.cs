using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Composite_Command
{
  public interface IApplicationCommand
  {
    CompositeCommand SaveAllCommand { get; set; }
  }

  public class ApplicationCommands : IApplicationCommand
  {
    public CompositeCommand SaveAllCommand { get; set; } = new CompositeCommand();
  }
}
