using Prism;
using Prism.Ioc;
using Prism.Modularity;
using System;
using System.IO;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using XamarinApp.Composite_Command;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using XamarinApp.Views;

namespace XamarinApp
{
  public partial class App
  {
    static DBConnection database;

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
      InitializeComponent();
      DependencyService.Register<UserLoginService>();
      DependencyService.Register<DBConnection>();

      await NavigationService.NavigateAsync("NavigationPage/LoginPage");
      //MainPage = new NavigationPage(new LoginPage());
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterSingleton<IAppInfo, AppInfoImplementation>();
      containerRegistry.Register<IUserLoginService, UserLoginService>();
      containerRegistry.Register<IRandomApiService, RamdomApiService>();
      containerRegistry.Register<ISignUpUserService, SignUpUserService>();
      containerRegistry.Register<IApplicationCommand, ApplicationCommands>();

      containerRegistry.RegisterForNavigation<NavigationPage>();
      containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
      containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
      containerRegistry.RegisterForNavigation<SignUpPage, SignUpPageViewModel>();
      containerRegistry.RegisterForNavigation<ApiDataPage, ApiDataPageViewModel>();
      containerRegistry.RegisterForNavigation<SelectedItemDetailPage, SelectedItemDetailPageViewModel>();
      containerRegistry.RegisterForNavigation<AddNotesPage, AddNotesPageViewModel>();
    }

  }
}
