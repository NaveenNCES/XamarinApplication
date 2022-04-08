using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace XamarinApp.ViewModels
{
  public class XamarinEssentialsViewModel : ViewModelBase
  {
    private ImageSource _image;
    public ImageSource Image
    {
      get { return _image; }
      set { SetProperty(ref _image, value); }
    }
    private string _appInfor;
    public string AppInfor
    {
      get { return _appInfor; }
      set { SetProperty(ref _appInfor, value); }
    }
    private string _appVersion;
    public string AppVersion
    {
      get { return _appVersion; }
      set { SetProperty(ref _appVersion, value); }
    }
    private string _connectivity;
    public string NetConnectivity
    {
      get { return _connectivity; }
      set { SetProperty(ref _connectivity, value); }
    }

    private string _deviceInfo;
    public string DeviceDetail
    {
      get { return _deviceInfo; }
      set { SetProperty(ref _deviceInfo, value); }
    }

    private bool _theam;
    public bool Theam
    {
      get { return _theam; }
      set {
        if (value)
        {
          App.Current.UserAppTheme = OSAppTheme.Dark;
        }
        else
        {
          App.Current.UserAppTheme = OSAppTheme.Light;
        }
        SetProperty(ref _theam, value); }
    }

    private string _emailId;
    public string EmailId
    {
      get { return _emailId; }
      set { SetProperty(ref _emailId, value); }
    }

    private string _subject;
    public string EmailSubject
    {
      get { return _subject; }
      set { SetProperty(ref _subject, value); }
    }

    private string _body;
    public string EmailBody
    {
      get { return _body; }
      set { SetProperty(ref _body, value); }
    }

    private string _location;
    public string Location
    {
      get { return _location; }
      set { SetProperty(ref _location, value); }
    }
    public DelegateCommand ScreenShotCommand { get; set; }
    public ICommand EmailCommand { get; set; }
    public ICommand LocationCommand { get; set; }
    public XamarinEssentialsViewModel()
    {
      ScreenShotCommand = new DelegateCommand(CaptureScreenshot);
      EmailCommand = new Command(OnEmailClicked);
      LocationCommand = new Command(OnLocationClicked);
      AppTheme appTheme = AppInfo.RequestedTheme;
      App.Current.UserAppTheme = (OSAppTheme)appTheme;
      AppInfor = AppInfo.Name;
      AppVersion = AppInfo.Version.ToString();
      NetConnectivity = $"The Mobile has {Connectivity.NetworkAccess}";
      DeviceDetail = DeviceInfo.Manufacturer;
    }

    private async void OnLocationClicked(object obj)
    {
      var status = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
      var location = await Geolocation.GetLocationAsync();
      Location = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
    }

    private async void OnEmailClicked(object obj)
    {
      var EmailAddress = new List<string>();
      EmailAddress.Add(EmailId);
      var message = new EmailMessage
      {
        To = EmailAddress,
        Body = EmailBody,
        Subject = EmailSubject
      };

      await Email.ComposeAsync(message);  
    }

    private async void CaptureScreenshot()
    {
      var screenshot = await Screenshot.CaptureAsync();
      var stream = await screenshot.OpenReadAsync();

      Image = ImageSource.FromStream(() => stream);
    }
  }
}
