using Prism;
using Prism.Ioc;
using Xamarin.Essentials.Implementation;
using Xamarin.Essentials.Interfaces;
using Xamarin.Forms;
using XamarinApp.Services;
using XamarinApp.Services.Interfaces;
using XamarinApp.ViewModels;
using XamarinApp.Views;

namespace XamarinApp
{
    public partial class App
    {
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

            containerRegistry.RegisterForNavigation<NavigationPage>();
            containerRegistry.RegisterForNavigation<MainPage, MainPageViewModel>();
            containerRegistry.RegisterForNavigation<LoginPage, LoginPageViewModel>();
        }
    }
}
