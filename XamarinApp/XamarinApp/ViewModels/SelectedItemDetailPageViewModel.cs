using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.ViewModels
{
  public class SelectedItemDetailPageViewModel : ViewModelBase, INavigationAware
  {
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private ObservableCollection<Result> _getSelectedData;
    public ObservableCollection<Result> getSelectedData
    {
      get
      {
        return _getSelectedData;
      }
      set
      {
        SetProperty(ref _getSelectedData, value);
      }
    }
    public SelectedItemDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
    {
      _navigation = navigationService;
      _pageDialogService = pageDialogService;
    }
    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
      var result = parameters.GetValue<List<Result>>("selectedData");
      ObservableCollection<Result> data = new ObservableCollection<Result>(result as List<Result>);

      getSelectedData = data;
    }
  }
}
