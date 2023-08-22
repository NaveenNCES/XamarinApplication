using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Hardware;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using XamarinApp.CustomRenderer;
using XamarinApp.Droid;
using XamarinApp.ViewModels;

[assembly: ExportRenderer(typeof(CustomViewRenderer),typeof(ViewRendererPageRendererAndroid))]
namespace XamarinApp.Droid
{
  public class ViewRendererPageRendererAndroid : ViewRenderer<CustomViewRenderer,TextView>
  {
    public ViewRendererPageRendererAndroid(Context context): base(context)
    {
    }

    protected override void OnElementChanged(ElementChangedEventArgs<CustomViewRenderer> e)
    {
      base.OnElementChanged(e);

      if(Control == null)
      {
        SetNativeControl(new TextView(Context)
        {
          Text = "Hello this is CustomRenderer for text"
        });

        Control.SetTextSize(Android.Util.ComplexUnitType.Sp, 24);
      }
    }

    protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      base.OnElementPropertyChanged(sender, e);
    }
  }
}
