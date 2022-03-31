using Module1.ViewModels;
using Module1.Views;
using Prism.Ioc;
using Prism.Modularity;
using Prism.Navigation;
using Prism.Regions;
using System;
using XamarinApp.Composite_Command;

namespace Module1
{
  public class Module1Module : IModule
  {
    
    public void OnInitialized(IContainerProvider containerProvider)
    {
      //await NavigationService.NavigateAsync("NavigationPage/ViewA");
      var regionManger = containerProvider.Resolve<IRegionManager>();

      //regionManger.RegisterViewWithRegion("View1A", typeof(Views.ViewA));
      IRegion region = regionManger.Regions["ContentRegion"];
      
      var tabA = containerProvider.Resolve<ViewA>();
      //SetTitle(tabA, "Tab A");
      region.Add(tabA);

      //var tabB= containerProvider.Resolve<ViewA>();
      //SetTitle(tabB, "Tab B");
      //region.Add(tabB);

      //var tabC = containerProvider.Resolve<ViewA>();
      //SetTitle(tabC, "Tab C");
      //region.Add(tabC);
    }

    //private void SetTitle(ViewA tab, string title)
    //{
    //  (tab.BindingContext as ViewAViewModel).Title = title;
    //}

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.Register<IApplicationCommand, ApplicationCommands>();

      containerRegistry.RegisterForNavigation<ViewA>();
    }
  }
}
