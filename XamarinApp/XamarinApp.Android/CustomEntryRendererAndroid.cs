using Android.App;
using Android.Content;
using Android.Graphics.Drawables;
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
using XamarinApp.CustomRenderer;
using XamarinApp.Droid;

[assembly: ExportRenderer(typeof(CustomRender), typeof(CustomEntryRendererAndroid))]
namespace XamarinApp.Droid
{
  class CustomEntryRendererAndroid : EntryRenderer
  {
    public CustomEntryRendererAndroid(Context context): base(context)
    {
    }

    CustomRender customRender;

    protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
    {
      base.OnElementChanged(e);

      if(e.OldElement == null)
      {
        customRender = (CustomRender)e.NewElement;
        var gradientDrawable = new GradientDrawable();
        gradientDrawable.SetCornerRadius(customRender.EntryCornerRadius);
        gradientDrawable.SetStroke(10, customRender.EntryBorderColor.ToAndroid());

        Control.SetBackground(gradientDrawable);
      }
    }
  }
}
