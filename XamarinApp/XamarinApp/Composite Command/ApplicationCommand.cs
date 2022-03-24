using Prism.Commands;
using System;
using System.Collections.Generic;
using System.Text;

namespace XamarinApp.Composite_Command
{
  public interface IApplicationCommand
  {
    CompositeCommand SaveAllCommand { get; }
  }

  public class ApplicationCommands : IApplicationCommand
  {
    public CompositeCommand SaveAllCommand { get; } = new CompositeCommand();
  }
}
