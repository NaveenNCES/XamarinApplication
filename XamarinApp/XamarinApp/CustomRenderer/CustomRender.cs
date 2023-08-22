using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace XamarinApp.CustomRenderer
{
  public class CustomRender : Entry
  {
    public static readonly string Description = "Random Api Data"; 
    public static readonly BindableProperty CornerRadiusProperty =
      BindableProperty.Create("CornerRadius", typeof(int), typeof(CustomRender), 0);

    public int EntryCornerRadius
    {
      get { return (int)GetValue(CornerRadiusProperty); }
      set { SetValue(CornerRadiusProperty, value); }
    }

    public static readonly BindableProperty BorderColorProperty =
      BindableProperty.Create("BorderThickness", typeof(Color), typeof(CustomRender), Color.AliceBlue);

    public Color EntryBorderColor
    {
      get { return (Color)GetValue(BorderColorProperty); }
      set { SetValue(BorderColorProperty, value); }
    }

  }
}
