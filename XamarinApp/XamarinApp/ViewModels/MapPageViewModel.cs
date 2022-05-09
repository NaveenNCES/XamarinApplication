using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms.Maps;
using XamarinApp.CustomRenderer;
using XamarinApp.Models;

namespace XamarinApp.ViewModels
{
  public class MapPageViewModel : ViewModelBase
  {
    private CustomMap _customMap;
    public CustomMap customMap
    {
      get { return _customMap; }
      set { SetProperty(ref _customMap, value); }
    }
    public MapPageViewModel()
    {
      customMap = new CustomMap();
      CustomPin pin = new CustomPin
      {
        Type = PinType.Place,
        Position = new Position(10.468870294698933, 77.64990631397154),
        Label = "You are at Naveen's location",
        Address = "Mullai Nagar,Chatrapatti,Oddanchatram",
        Name = "Xamarin",
        Url = "http://xamarin.com/about/"
      };

      customMap.CustomPins = new List<CustomPin> { pin };
      customMap.Pins.Add(pin);
      customMap.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(10.468870294698933, 77.64990631397154), Distance.FromMiles(1.0)));
    }
  }
}
