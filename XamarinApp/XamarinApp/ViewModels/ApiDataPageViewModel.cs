using Prism.AppModel;
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
using Xamarin.CommunityToolkit.ObjectModel;
using Xamarin.Forms;
using XamarinApp.Services.Interfaces;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.ViewModels
{
  public class ApiDataPageViewModel : ViewModelBase, INavigatedAware, IPageLifecycleAware
  {
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly IRandomApiService _randomApiService;
    public ICommand ApiCommand { get; set; }
    public DelegateCommand<object> ItemTappedCommand { get; set; }

    private ObservableCollection<Result> _apiDatas;
    public Result getSpecificData { get; set; }

    public ObservableCollection<Result> ApiData
    {
      get
      {
        return _apiDatas;
      }
      set
      {
        SetProperty(ref _apiDatas, value);
      }
    }
    public ApiDataPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IRandomApiService randomApiService)
    {
      _navigation = navigationService;
      _pageDialogService = pageDialogService;
      _randomApiService = randomApiService;
      ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
    }

    async void ItemTapped(object specificData)
    {
      var result = specificData as Result;
      var data = new NavigationParameters();
      var list = ApiData.Where(x => x == result).ToList();
      data.Add("selectedData", list);
      await _navigation.NavigateAsync("SelectedItemDetailPage", data);
    }

    public async void OnNavigatedTo(INavigationParameters parameters)
    {
      var result = await _randomApiService.getRandomApiData();

      ObservableCollection<Result> data = new ObservableCollection<Result>(result as List<Result>);
      ApiData = data;
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public void OnAppearing()
    {
      //Console.WriteLine("We are appearing");
      _pageDialogService.DisplayAlertAsync("Alert", "Displaying Api Data", "OK");
    }

    public void OnDisappearing()
    {
      Console.WriteLine("We are disappearing");
    }
  }
}
