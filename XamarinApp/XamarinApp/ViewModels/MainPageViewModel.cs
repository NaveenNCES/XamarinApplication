using Prism.Commands;
using Prism.Mvvm;
using Prism.Navigation;
using Prism.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using XamarinApp.Services.Interfaces;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.ViewModels
{
  public class MainPageViewModel : ViewModelBase, INavigatedAware
  {
    private readonly INavigationService Navigation;
    private readonly IPageDialogService _pageDialogService;
    public ICommand LoginCommand { get; set; }
    public ICommand ApiCommand { get; set; }
    public DelegateCommand EventAggregatorCommand { get; set; }

    private List<string> _name;
    public List<string> Name
    {
      get { return _name; }
      set { SetProperty(ref _name, value); }
    }

    private string _message;
    public string Message
    {
      get { return _message; }
      set { SetProperty(ref _message, value); }
    }



    public MainPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService)
    {
      Navigation = navigationService;
      _pageDialogService = pageDialogService;
      LoginCommand = new Command(OnLoginClicked);
      ApiCommand = new Command(OnApiClicked);
      EventAggregatorCommand = new DelegateCommand(OnEventClicked);
      MessagingCenter.Subscribe<LoginPageViewModel,string>(this,"Hi", (sender,args) =>
      {
        //await _pageDialogService.DisplayAlertAsync("Test", "Hi " + args, "Ok");
        message(args);
      });
    }

    private void message(string data)
    {
      //Name.Add(data);
    }
    private async void OnEventClicked()
    {
      await Navigation.NavigateAsync("AddNotesPage");
    }

    private async void OnApiClicked()
    {
      await Navigation.NavigateAsync("ApiDataPage");
    }

    private async void OnLoginClicked()
    {
      await Navigation.NavigateAsync("LoginPage");
    }

    public void OnNavigatedFrom(INavigationParameters parameters)
    {     
    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
      Message= parameters.GetValue<string>("Name");
      //var a = parameters.GetValue<string>("Name");
      //Message.Add(a);
      //Name = parameters.GetValue<string>("Name");

      //MessagingCenter.Subscribe<LoginPageViewModel,DateTime>(this, "tick", (p,datetime) =>
      //{
      //  Message.Add(datetime.ToString());
      //});
    }
  }
}
