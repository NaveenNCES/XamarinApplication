using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinApp.Droid;
using XamarinApp.Effects;
using Button = Xamarin.Forms.Button;

[assembly: ResolutionGroupName("XamarinApp")]
[assembly:ExportEffect(typeof(ButtonShadowEffect),"ShadowEffect")]
namespace XamarinApp.Droid
{
  public class ButtonShadowEffect : PlatformEffect
  {
    public ButtonShadowEffect() { }

    protected override void OnAttached()
    {
      Button button = Element as Button;
      if (button == null)
        return;

      try
      {
        var control = Control as Android.Widget.Button;
        var effect = (CustomEffects)Element.Effects.FirstOrDefault(e => e is CustomEffects);
        if(effect != null)
        {
          float radius = effect.Radius;
          float distanceX = effect.DistanceX;
          float distanceY = effect.DistanceY;
          Android.Graphics.Color color = effect.Color.ToAndroid();
          control.SetShadowLayer(radius, distanceX, distanceY, color);
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine("Cannot set property on attached, Error", ex.Message);
      }
    }

    protected override void OnDetached()
    {
      //throw new NotImplementedException();
    }
  }
}
