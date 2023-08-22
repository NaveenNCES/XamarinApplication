using NLog;
using NLog.Config;
using NLog.Targets;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using Xamarin.Forms;

[assembly: Xamarin.Forms.Dependency(typeof(NLogManagerService))]
namespace XamarinApp.Services
{
  public class NLogManagerService : ILogManager
  {
    public NLogManagerService()
    {
      var config = new LoggingConfiguration();

      var consoleTarget = new ConsoleTarget();
      config.AddTarget("console", consoleTarget);

      var consoleRule = new LoggingRule("*", LogLevel.Trace, consoleTarget);
      config.LoggingRules.Add(consoleRule);

      var fileTarget = new FileTarget();
      string folder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
      fileTarget.FileName = Path.Combine(folder, "Log.txt");
      config.AddTarget("file", fileTarget);

      var fileRule = new LoggingRule("*", LogLevel.Warn, fileTarget);
      config.LoggingRules.Add(fileRule);

      LogManager.Configuration = config;
    }
    public Interfaces.ILogger GetLog([System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "")
    {
      string fileName = callerFilePath;

      if (fileName.Contains("/"))
      {
        fileName = fileName.Substring(fileName.LastIndexOf("/", StringComparison.CurrentCultureIgnoreCase) + 1);
      }

      var logger = LogManager.GetLogger(fileName);
      return new NLogLogger(logger);
    }
  }
}
