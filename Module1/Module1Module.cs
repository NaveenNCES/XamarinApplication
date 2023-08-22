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
      var regionManger = containerProvider.Resolve<IRegionManager>();

      IRegion region = regionManger.Regions["ContentRegion"];
      
      var tabA = containerProvider.Resolve<ViewA>();

      region.Add(tabA);
    }

    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.Register<IApplicationCommand, ApplicationCommands>();

      containerRegistry.RegisterForNavigation<ViewA>();
    }
  }
}
