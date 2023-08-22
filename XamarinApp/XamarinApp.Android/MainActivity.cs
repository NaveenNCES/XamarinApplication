using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.Gms.Auth.Api;
using Android.Gms.Auth.Api.SignIn;
using Android.OS;
using Android.Widget;
using Com.Xamarin.Textcounter;
using Firebase;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;

namespace XamarinApp.Droid
{
  [Activity(Theme = "@style/MainTheme",
            ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    internal static MainActivity Instance { get; private set; }

    protected override void OnCreate(Bundle savedInstanceState)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;
      Instance = this;

      base.OnCreate(savedInstanceState);
      Xamarin.Essentials.Platform.Init(this, savedInstanceState);
      global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
      Xamarin.FormsMaps.Init(this, savedInstanceState);
      UserDialogs.Init(this);

      int Vowels = TextCounter.NumVowels("This is a sample Binding Library");
      int Consonents = TextCounter.NumConsonants("This is a sample Binding Library");

      LoadApplication(new App(new AndroidInitializer()));

      FirebasePushNotificationManager.ProcessIntent(this, Intent);

    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
    {
      base.OnActivityResult(requestCode, resultCode, data);
      if (requestCode == 1)
      {
        GoogleSignInResult result = Auth.GoogleSignInApi.GetSignInResultFromIntent(data);
        GoogleManager.Instance.OnAuthCompleted(result);
      }
    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    {
      Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
  }

  public class AndroidInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<ILogManager, NLogManager>();
      containerRegistry.RegisterSingleton<IGoogleManager, GoogleManager>();
      // Register any platform specific implementations
    }
  }
}

