using Module1;
using Module1.ViewModels;
using Module1.Views;
using NLog;
using Plugin.FirebasePushNotification;
using Prism;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
using Xamarin.CommunityToolkit.Helpers;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using XamarinApp.Composite_Command;
using XamarinApp.Models;
using XamarinApp.Resx;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using XamarinApp.Views;

namespace XamarinApp
{
  public partial class App 
  {
    static DBConnection database;
    public string DatabaseLocation;
    public static DBConnection Database
    {
      get
      {
        if (database == null)
        {
          database = new DBConnection(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.db3"));
        }

        return database;
      }
    }


    public App(IPlatformInitializer initializer)
        : base(initializer)
    {
    }

    protected override async void OnInitialized()
    {
      LocalizationResourceManager.Current.Init(AppResource.ResourceManager);
      InitializeComponent();
      DependencyService.Register<DBConnection>();
      DatabaseLocation = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "user.db5");
      await NavigationService.NavigateAsync("NavigationPage/LoginPage");

      CrossFirebasePushNotification.Current.OnTokenRefresh += firebasePushNotificationTokenEventHandler;
    }

    private void firebasePushNotificationTokenEventHandler(object source, FirebasePushNotificationTokenEventArgs e)
    {
      System.Diagnostics.Debug.WriteLine($"Token: { e.Token}");
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
      containerRegistry.Register<IUserLoginService, UserLoginService>();
      containerRegistry.Register<IRandomApiService, RamdomApiService>();
      containerRegistry.Register<ISignUpUserService, SignUpUserService>();
      containerRegistry.Register<IApplicationCommand, ApplicationCommands>();
      containerRegistry.Register<ILogManager,NLogManagerService>();
      containerRegistry.Register<Services.Interfaces.ILogger, NLogLogger>();
      containerRegistry.RegisterSingleton<Logger>();
      containerRegistry.Register(typeof(IRepository<>),typeof(Repository<>));
      //containerRegistry.RegisterInstance<IRepository<UserModel>>(new GenericRepository<UserModel>(DatabaseLocation));

      containerRegistry.RegisterForNavigation<NavigationPage>();
      containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
      containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
      containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
      containerRegistry.RegisterForNavigation<ApiDataPage, ApiDataPageViewModel>();
      containerRegistry.RegisterForNavigation<SelectedItemDetailPage, SelectedItemDetailPageViewModel>();
      containerRegistry.RegisterForNavigation<AddNotesPage, AddNotesPageViewModel>();
      containerRegistry.RegisterForNavigation<GesturePage, GesturePageViewModel>();
      containerRegistry.RegisterForNavigation<XamarinEssentials, XamarinEssentialsViewModel>();
    }

    protected override void ConfigureModuleCatalog(IModuleCatalog moduleCatalog)
    {
      moduleCatalog.AddModule<Module1Module>();
      //moduleCatalog.AddModule<ViewA, ViewAViewModel>();
    }
    protected override void OnStart()
    {
      base.OnStart();
    }
    protected override void OnSleep()
    {
      base.OnSleep();
    }
    protected override async void OnResume()
    {
      await NavigationService.NavigateAsync("NavigationPage/LoginPage");
    }
  }
}
