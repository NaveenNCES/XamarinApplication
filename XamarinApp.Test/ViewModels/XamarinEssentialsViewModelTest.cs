using AutoFixture;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Controls;
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
    private readonly MockRepository _mockRepository;
    private readonly Mock<IAppInfo> _appInfo;
    private readonly Mock<IScreenshot> _screenShot;
    private readonly Mock<IGeolocation> _geoLocation;
    private readonly Mock<IDeviceInfo> _ideviceInfo;
    private readonly Mock<IConnectivity> _iconnectivity;
    private readonly Mock<IEmail> _iemail;
    private readonly Mock<IPermissions> _permissions;

    public XamarinEssentialsViewModelTest()
    {
      _mockRepository = new MockRepository(MockBehavior.Strict);
      _appInfo = _mockRepository.Create<IAppInfo>();
      _screenShot = _mockRepository.Create<IScreenshot>();
      _geoLocation = _mockRepository.Create<IGeolocation>();
      _ideviceInfo = _mockRepository.Create<IDeviceInfo>();
      _iconnectivity = _mockRepository.Create<IConnectivity>();
      _iemail = _mockRepository.Create<IEmail>();
      _permissions = _mockRepository.Create<IPermissions>();
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
      _permissions.Setup(x => x.RequestAsync<Permissions.LocationWhenInUse>()).ReturnsAsync(PermissionStatus.Granted);

      //Act
      viewModel.LocationCommand.Execute();

      //Assert
      viewModel.Location.Should().BeEquivalentTo(result);
      _appInfo.Verify(x => x.RequestedTheme);
      _appInfo.Verify(x => x.Name);
      _appInfo.Verify(x => x.Version);
      _iconnectivity.Verify(x => x.NetworkAccess);
      _geoLocation.Verify(x => x.GetLocationAsync());
      _ideviceInfo.Verify(x => x.Manufacturer);
      _permissions.Verify(x => x.RequestAsync<Permissions.LocationWhenInUse>());
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void OnEmail_Clicked_Should_Compose_Email()
    {
      //Arrange
      //var fixture = _fixture.Create<string>();
      //var EmailAddress = new List<string>();
      //EmailAddress.Add(fixture);
      //var message = new EmailMessage
      //{
      //  To = EmailAddress,
      //  Body = fixture,
      //  Subject = fixture
      //};
      var email = _fixture.Create<EmailMessage>();
      _iemail.Setup(x => x.ComposeAsync(It.IsAny<EmailMessage>())).Returns(Task.FromResult(false));

      //Act
      viewModel.EmailBody = email.Body;
      viewModel.EmailId = email.To.FirstOrDefault();
      viewModel.EmailSubject = email.Subject;
      viewModel.EmailCommand.Execute();

      //Assert
      _iemail.Verify(x => x.ComposeAsync(It.IsAny<EmailMessage>()), Times.Once);
      _appInfo.Verify(x => x.RequestedTheme);
      _appInfo.Verify(x => x.Name);
      _appInfo.Verify(x => x.Version);
      _iconnectivity.Verify(x => x.NetworkAccess);
      _ideviceInfo.Verify(x => x.Manufacturer);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Chec_Net_Connectivity()
    {
      //Arrange
      var actual = $"The Mobile has {_iconnectivity.Object.NetworkAccess}";

      //Act
      var result = viewModel.NetConnectivity;

      //Assert
      result.Should().BeEquivalentTo(actual);
      _appInfo.Verify(x => x.RequestedTheme);
      _appInfo.Verify(x => x.Name);
      _appInfo.Verify(x => x.Version);
      _iconnectivity.Verify(x => x.NetworkAccess);
      _ideviceInfo.Verify(x => x.Manufacturer);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void AppInfo_Details()
    {
      //Arrange
      var appName = _appInfo.Object.Name;
      var appVersion = _appInfo.Object.Version.ToString();

      //Act
      var actualAppName = viewModel.AppInfor;
      var actualAppVersion = viewModel.AppVersion;

      //Assert
      actualAppName.Should().BeEquivalentTo(appName);
      actualAppVersion.Should().BeEquivalentTo(appVersion);
      _appInfo.Verify(x => x.RequestedTheme);
      _appInfo.Verify(x => x.Name);
      _appInfo.Verify(x => x.Version);
      _iconnectivity.Verify(x => x.NetworkAccess);
      _ideviceInfo.Verify(x => x.Manufacturer);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Check_DeviceInfo_Details()
    {
      //Arrange
      var manufacturer = _ideviceInfo.Object.Manufacturer;

      //Act
      var result = viewModel.DeviceDetail;

      //Assert
      result.Should().BeEquivalentTo(manufacturer);
      _appInfo.Verify(x => x.RequestedTheme);
      _appInfo.Verify(x => x.Name);
      _appInfo.Verify(x => x.Version);
      _iconnectivity.Verify(x => x.NetworkAccess);
      _ideviceInfo.Verify(x => x.Manufacturer);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }

    [Fact]
    public void Check_App_Requested_Theam()
    {
      //Arrange
      var actualTheam = _appInfo.Object.RequestedTheme;

      //Act
      var result = viewModel.AppTheamDetail;

      //Assert
      result.Should().Be(actualTheam);
      _appInfo.Verify(x => x.RequestedTheme);
      _appInfo.Verify(x => x.Name);
      _appInfo.Verify(x => x.Version);
      _iconnectivity.Verify(x => x.NetworkAccess);
      _ideviceInfo.Verify(x => x.Manufacturer);
      _mockRepository.Verify();
      _mockRepository.VerifyNoOtherCalls();
    }
  }
}
