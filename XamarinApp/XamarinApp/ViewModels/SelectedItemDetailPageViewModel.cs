using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
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
    private readonly ApiDataPageViewModel _apiDataPageView;
    public ICommand SelectedDataCommand { get; set; }
    private List<Result> _getSelectedData;
    public List<Result> getSelectedData
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
    private List<Result> _transferData;
    public List<Result> transferData
    {
      get
      {
        return _transferData;
      }
      set
      {
        SetProperty(ref _transferData, value);
      }
    }
    public SelectedItemDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, ApiDataPageViewModel apiDataPageView)
    {
      _navigation = navigationService;
      _pageDialogService = pageDialogService;
      _apiDataPageView = apiDataPageView;
      SelectedDataCommand = new Command(OnDataClicked);
    }

    private  void OnDataClicked()
    {
      getSelectedData=transferData;
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
      throw new NotImplementedException();
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
      transferData = parameters.GetValue<List<Result>>("selectedData");
    }
  }
}
