using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApp.Effects
{
  public class CustomEffects : RoutingEffect
  {
    public CustomEffects() : base("XamarinApp.ShadowEffect") { }

    public float Radius { get; set; }
    public Color Color { get; set; }
    public float DistanceX { get; set; }
    public float DistanceY { get; set; }
  }
}
