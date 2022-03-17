using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Services.Interfaces;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.ViewModels
{
  public class ApiDataPageViewModel : ViewModelBase
  {
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly IRandomApiService _randomApiService;
    public ICommand ApiCommand { get; set; }
    public ICommand ItemTappedCommand { get; set; }

    private List<Result> _apiDatas;
    public Result getSpecificData { get; set; }
    

    public List<Result> ApiData
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
      ApiCommand = new Command(OnApiClicked);
      ItemTappedCommand = new Command(ItemTapped);
    }

    public void ItemTapped()
    {
      var data = new NavigationParameters();
      var list = ApiData.Where(x => x == getSpecificData).ToList();
      data.Add("selectedData",list);
      //_pageDialogService.DisplayAlertAsync("User Data", "Name: " + selectedItem.Name.First +" "+ selectedItem.Name.Last, "Ok");
      _navigation.NavigateAsync("SelectedItemDetailPage",data);
    }

    private async void OnApiClicked()
    {
      var result = await _randomApiService.getRandomApiData();

      ApiData = result;
    }
  }
}
