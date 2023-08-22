using CoreGraphics;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinApp.Effects;
using XamarinApp.iOS;

[assembly: ResolutionGroupName("XamarinApp")]
[assembly: ExportEffect(typeof(ButtonShadowEffect), "ShadowEffect")]
namespace XamarinApp.iOS
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
        var effect = (CustomEffects)Element.Effects.FirstOrDefault(e => e is CustomEffects);
        if(effect != null)
        {
          Control.Layer.CornerRadius = effect.Radius;
          Control.Layer.ShadowColor = effect.Color.ToCGColor();
          Control.Layer.ShadowOffset = new CGSize(effect.DistanceX, effect.DistanceY);
          Control.Layer.ShadowOpacity = 1.0f;
        }
      }
      catch(Exception ex)
      {
        Console.WriteLine("Cannot set property OnAttached", ex.Message);
      }
    }

    protected override void OnDetached()
    {
      throw new NotImplementedException();
    }
  }
}
