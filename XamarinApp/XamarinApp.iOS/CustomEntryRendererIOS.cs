using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using XamarinApp.CustomRenderer;
using XamarinApp.iOS;

[assembly: ExportRenderer(typeof(CustomRender), typeof(CustomEntryRendererIOS))]
namespace XamarinApp.iOS
{
  public class CustomEntryRendererIOS : EntryRenderer
  {
    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      base.OnElementChanged(e);

      if(e.OldElement == null)
      {
        var customEntry = (CustomRender)e.NewElement;

        Control.Layer.CornerRadius = customEntry.EntryCornerRadius;
        Control.Layer.BorderColor = customEntry.EntryBorderColor.ToCGColor();

        Control.Layer.BorderWidth = 2;
      }
    }
  }
}
