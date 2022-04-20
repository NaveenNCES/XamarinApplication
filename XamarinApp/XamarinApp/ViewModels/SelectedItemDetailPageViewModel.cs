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
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using XamarinApp.PageName;
using XamarinApp.Resx;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.ViewModels
{
  public class SelectedItemDetailPageViewModel : ViewModelBase
  {
    private readonly INavigationService _navigation;
    private readonly IPageDialogService _pageDialogService;
    private readonly IPhoneDialer _dialor;
    private readonly IEmail _email;
    public DelegateCommand PhoneDialerCommand { get; }
    public DelegateCommand EmailCommand { get; }
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
    public SelectedItemDetailPageViewModel(INavigationService navigationService, IPageDialogService pageDialogService,IPhoneDialer phoneDialer,IEmail email)
    {
      _navigation = navigationService;
      _pageDialogService = pageDialogService;
      _dialor = phoneDialer;
      _email = email;
      PhoneDialerCommand = new DelegateCommand(OnCallClicked);
      EmailCommand = new DelegateCommand(async () => await OnEmailClicked());
    }

    private async Task OnEmailClicked()
    {
      var EmailAddress = new List<string>();
      var UserDetail = getSelectedData.FirstOrDefault();
      EmailAddress.Add(UserDetail.Email);

      var message = new EmailMessage
      {
        To = EmailAddress
      };

      await _email.ComposeAsync(message);
    }

    private void OnCallClicked()
    {
      _dialor.Open(getSelectedData.FirstOrDefault().Phone);
    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {
      var result = parameters.GetValue<List<Result>>(NavigationKeys.selectedData);
      ObservableCollection<Result> data = new ObservableCollection<Result>(result);

      getSelectedData = data;
    }
  }
}
