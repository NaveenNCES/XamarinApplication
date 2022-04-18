using Prism.AppModel;
using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.ViewModels
{
  public class ViewModelBase : BindableBase
  {
    private bool _isLoading;
    public bool IsLoading
    {
      get { return _isLoading; }
      set { SetProperty(ref _isLoading, value); }
    }

    private bool _iIndicatorVisible;
    public bool IndicatorVisible
    {
      get { return _iIndicatorVisible; }
      set { SetProperty(ref _iIndicatorVisible, value); }
    }
  }
}
