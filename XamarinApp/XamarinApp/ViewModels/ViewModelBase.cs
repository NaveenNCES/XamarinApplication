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
  public class ViewModelBase : BindableBase, INavigatedAware,IPageLifecycleAware
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

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {
    }

    public virtual void OnAppearing()
    {
    }

    public virtual void OnDisappearing()
    {
    }
  }
}
