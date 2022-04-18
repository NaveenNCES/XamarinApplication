using Prism.Commands;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
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
    private AppTheme _appTheamDetail;
    public AppTheme AppTheamDetail
    {
      get { return _appTheamDetail; }
      set { SetProperty(ref _appTheamDetail, value); }
    }
    public DelegateCommand ScreenShotCommand { get; set; }
    public DelegateCommand EmailCommand { get; set; }
    public DelegateCommand LocationCommand { get; set; }

    /////Interface Declaration////
    private readonly IAppInfo _appInfo;
    private readonly IScreenshot _screenShot;
    private readonly IGeolocation _geoLocation;
    private readonly IDeviceInfo _ideviceInfo;
    private readonly IConnectivity _iconnectivity;
    private readonly IEmail _iemail;
    private readonly IPermissions _permissions;

    public XamarinEssentialsViewModel(IAppInfo appInfo,IScreenshot screenshot,IGeolocation geolocation,
      IDeviceInfo deviceInfo,IConnectivity connectivity,IEmail email,IPermissions permissions)
    {
      _appInfo = appInfo;
      _screenShot = screenshot;
      _geoLocation = geolocation;
      _ideviceInfo = deviceInfo;
      _iconnectivity = connectivity;
      _iemail = email;
      _permissions = permissions;
      ScreenShotCommand = new DelegateCommand(CaptureScreenshot);
      EmailCommand = new DelegateCommand(OnEmailClicked);
      LocationCommand = new DelegateCommand(OnLocationClicked);
      AppTheamDetail = _appInfo.RequestedTheme;
      //App.Current.UserAppTheme = (OSAppTheme)appTheme;
      AppInfor = _appInfo.Name;
      AppVersion = _appInfo.Version.ToString();
      NetConnectivity = $"The Mobile has {_iconnectivity.NetworkAccess}";
      DeviceDetail = _ideviceInfo.Manufacturer;
    }

    private async void OnLocationClicked()
    {
      var status = await _permissions.RequestAsync<Permissions.LocationWhenInUse>();
      var location = await _geoLocation.GetLocationAsync();
      Location = $"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}";
    }

    private async void OnEmailClicked()
    {
      var EmailAddress = new List<string>();
      EmailAddress.Add(EmailId);
      var message = new EmailMessage
      {
        To = EmailAddress,
        Body = EmailBody,
        Subject = EmailSubject
      };

      await _iemail.ComposeAsync(message);  
    }

    private async void CaptureScreenshot()
    {
      var screenshot = await _screenShot.CaptureAsync();
      var stream = await screenshot.OpenReadAsync();

      Image = ImageSource.FromStream(() => stream);
    }
  }
}
