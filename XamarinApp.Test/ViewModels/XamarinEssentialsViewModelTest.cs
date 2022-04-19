using AutoFixture;
using Moq;
using System;
using System.Collections.Generic;
using Xamarin.Essentials;
using Xamarin.Essentials.Interfaces;
using XamarinApp.ViewModels;
using Xunit;

namespace XamarinApp.Test.ViewModels
{
  public class XamarinEssentialsViewModelTest
  {
    private readonly XamarinEssentialsViewModel viewModel;
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<IAppInfo> _appInfo;
    private readonly Mock<IScreenshot> _screenShot;
    private readonly Mock<IGeolocation> _geoLocation;
    private readonly Mock<IDeviceInfo> _ideviceInfo;
    private readonly Mock<IConnectivity> _iconnectivity;
    private readonly Mock<IEmail> _iemail;
    private readonly Mock<IPermissions> _permissions;

    public XamarinEssentialsViewModelTest()
    {
      _appInfo = new Mock<IAppInfo>();
      _screenShot = new Mock<IScreenshot>();
      _geoLocation = new Mock<IGeolocation>();
      _ideviceInfo = new Mock<IDeviceInfo>();
      _iconnectivity = new Mock<IConnectivity>();
      _iemail = new Mock<IEmail>();
      _permissions = new Mock<IPermissions>();
      _appInfo.Setup(x => x.RequestedTheme).Returns(AppTheme.Light);
      _appInfo.Setup(x => x.Name).Returns("Xamarin");
      _appInfo.Setup(x => x.Version).Returns(new Version("1.1.1"));
      _iconnectivity.Setup(x => x.NetworkAccess).Returns(NetworkAccess.Internet);
      _geoLocation.Setup(x => x.GetLocationAsync()).ReturnsAsync(() => new Location());
      _ideviceInfo.Setup(x => x.Manufacturer).Returns("Samsung");
      viewModel = new XamarinEssentialsViewModel(_appInfo.Object,_screenShot.Object,_geoLocation.Object,_ideviceInfo.Object
        ,_iconnectivity.Object,_iemail.Object,_permissions.Object);
    }

    [Fact]
    public async void OnLocation_Should_Return_User_CurrentLocation()
    {
      //Arrange
      var a = await _geoLocation.Object.GetLocationAsync();
      var result = $"Latitude: {a.Latitude}, Longitude: {a.Longitude}, Altitude: {a.Altitude}";

      //Act
      viewModel.LocationCommand.Execute();

      //Assert
      Assert.Equal(result, viewModel.Location);
    }

    [Fact]
    public void OnEmail_Clicked_Should_Compose_Email()
    {
      //Arrange
      var fixture = _fixture.Create<string>();
      var EmailAddress = new List<string>();
      EmailAddress.Add(fixture);
      var message = new EmailMessage
      {
        To = EmailAddress,
        Body = fixture,
        Subject = fixture
      };


      //Act
      viewModel.EmailBody = fixture;
      viewModel.EmailId = fixture;
      viewModel.EmailSubject = fixture;
      viewModel.EmailCommand.Execute();

      //Assert
      _iemail.Verify(x => x.ComposeAsync(It.IsAny<EmailMessage>()), Times.Once);
    }

    [Fact]
    public void Chec_Net_Connectivity()
    {
      //Arrange
      var net = NetworkAccess.Internet;
      var actual = $"The Mobile has {net}";

      //Act
      var result = viewModel.NetConnectivity;

      //Assert
      Assert.Equal(actual, result);
    }

    [Fact]
    public void AppInfo_Details()
    {
      //Arrange
      var appName = "Xamarin";
      var appVersion = "1.1.1";

      //Act
      var actualAppName = viewModel.AppInfor;
      var actualAppVersion = viewModel.AppVersion;

      //Assert
      Assert.Equal(appName, actualAppName);
      Assert.Equal(appVersion, actualAppVersion);
    }

    [Fact]
    public void Check_DeviceInfo_Details()
    {
      //Arrange
      var manufacturer = "Samsung";

      //Act
      var result = viewModel.DeviceDetail;

      //Assert
      Assert.Equal(result, manufacturer);
    }

    [Fact]
    public void Check_App_Requested_Theam()
    {
      //Arrange
      var actualTheam = AppTheme.Light;

      //Act
      var result = viewModel.AppTheamDetail;

      //Assert
      Assert.Equal(actualTheam, result);
    }
  }
}
