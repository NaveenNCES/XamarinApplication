using Prism.Commands;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using XamarinApp.PageName;
using XamarinApp.Resx;
using XamarinApp.Services.Interfaces;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.ViewModels
{
  public class ApiDataPageViewModel : ViewModelBase
  {
    private readonly INavigationService _navigationService;
    private readonly IPageDialogService _pageDialogService;
    private readonly IRandomApiService _randomApiService;

    public DelegateCommand<object> ItemTappedCommand { get; }

    private ObservableCollection<Result> _apiData;

    public ObservableCollection<Result> ApiData
    {
      get
      {
        return _apiData;
      }
      set
      {
        SetProperty(ref _apiData, value);
      }
    }
    public ApiDataPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService, IRandomApiService randomApiService)
    {
      _navigationService = navigationService;
      _pageDialogService = pageDialogService;
      _randomApiService = randomApiService;
      ItemTappedCommand = new DelegateCommand<object>(ItemTapped);
    }

    async void ItemTapped(object specificData)
    {
      try
      {
        IsLoading = true;
        IndicatorVisible = true;

        var result = specificData as Result;
        var data = new NavigationParameters();
        var list = ApiData.Where(x => x == result).ToList();
        data.Add(NavigationKeys.selectedData, list);        
        await _navigationService.NavigateAsync(PageNames.SelectedItemDetailPage, data);
      }

      catch(Exception ex)
      {
        await _pageDialogService.DisplayAlertAsync(AppResource.Alert, ex.Message, AppResource.Ok);
      }

      finally
      {
        IsLoading = false;
        IndicatorVisible = false;
      }    
      
    }

    public async override void OnNavigatedTo(INavigationParameters parameters)
    {
      if (parameters.GetNavigationMode() == NavigationMode.New)
      {
        var result = await _randomApiService.GetRandomApiDataAsync();

        ObservableCollection<Result> data = new ObservableCollection<Result>(result);
        ApiData = data;
      }      
    }

    public override void OnAppearing()
    {
      _pageDialogService.DisplayAlertAsync(AppResource.Alert, AppResource.ApiDataDisplaying , AppResource.Ok);
    }

    public override void OnDisappearing()
    {
      Console.WriteLine("We are disappearing");
    }
  }
}
