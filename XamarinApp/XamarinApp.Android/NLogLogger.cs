using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using NLog;
using System;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using XamarinApp.Services.Interfaces;
using XamarinApp.Droid;
using ILogger = XamarinApp.Services.Interfaces.ILogger;

[assembly: Dependency(typeof(NLogLogger))]
namespace XamarinApp.Droid
{
  public class NLogLogger : ILogger
  {
    private Logger log;

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
