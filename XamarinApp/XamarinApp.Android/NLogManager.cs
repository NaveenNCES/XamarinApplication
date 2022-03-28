using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NLog;
using NLog.Config;
using NLog.Targets;
using Xamarin.Forms;
using XamarinApp.Droid;
using XamarinApp.Services.Interfaces;
using Environment = System.Environment;
using ILogger = XamarinApp.Services.Interfaces.ILogger;

[assembly: Dependency(typeof(NLogManager))]
namespace XamarinApp.Droid
{
  class NLogManager : ILogManager
  {
    public NLogManager()
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

    public ILogger GetLog([System.Runtime.CompilerServices.CallerFilePath] string callerFilePath = "")
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
