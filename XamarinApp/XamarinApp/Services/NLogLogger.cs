using NLog;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using ILogger = XamarinApp.Services.Interfaces.ILogger;

[assembly: Xamarin.Forms.Dependency(typeof(NLogLogger))]
namespace XamarinApp.Services
{
  public class NLogLogger : ILogger
  {
    private readonly Logger log;

    public NLogLogger(Logger log)
    {
      this.log = log;
    }

    public void Debug(string text, params object[] args)
    {
      log.Debug(text, args);
    }

    public void Error(string text, params object[] args)
    {
      log.Error(text, args);
    }

    public void Fatal(string text, params object[] args)
    {
      log.Fatal(text, args);
    }    

    public void Info(string text, params object[] args)
    {
      log.Info(text, args);
    }

    public void Trace(string text, params object[] args)
    {
      log.Trace(text, args);
    }

    public void Warn(string text, params object[] args)
    {
      log.Warn(text, args);
    }    
  }
}
