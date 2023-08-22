using Android.App;
using Android.Content;
using Android.Gms.Maps;
using Android.Gms.Maps.Model;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Maps.Android;
using XamarinApp.CustomRenderer;
using XamarinApp.Droid.CustomRenderers;
using XamarinApp.Models;

[assembly: ExportRenderer(typeof(CustomMap), typeof(CustomMapRenderer))]
namespace XamarinApp.Droid.CustomRenderers
{
  public class CustomMapRenderer : MapRenderer, GoogleMap.IInfoWindowAdapter
  {
    public CustomMapRenderer(Context context) : base(context)
    {
    }

    List<CustomPin> customPins;
    CustomMap customMap = new CustomMap();

    protected override void OnElementChanged(Xamarin.Forms.Platform.Android.ElementChangedEventArgs<Map> e)
    {
      base.OnElementChanged(e);

      if (e.OldElement == null)
      {
        var pin = new CustomPin
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

        //var formsMap = (CustomMap)e.NewElement;
        customPins = customMap.CustomPins;
      }
    }
    protected override void OnMapReady(GoogleMap map)
    {
      var mMap = map;

      LatLng syndey = new LatLng(10.468870294698933, 77.64990631397154);
      mMap.AddMarker(CreateMarker(customPins.FirstOrDefault()));

      CameraPosition.Builder builder = CameraPosition.InvokeBuilder();
      builder.Target(syndey);
      builder.Zoom(18);
      builder.Bearing(155);
      builder.Tilt(65);

      CameraPosition cameraPosition = builder.Build();

      CameraUpdate cameraUpdate = CameraUpdateFactory.NewCameraPosition(cameraPosition);

      mMap.AnimateCamera(cameraUpdate);

      var createMarker = CreateMarker(customPins.FirstOrDefault());

      NativeMap.InfoWindowClick += OnInfoWindowClick;
      NativeMap.SetInfoWindowAdapter(this);
      //map.MoveToRegion()
      base.OnMapReady(mMap);     

    }

     void OnInfoWindowClick(object sender, GoogleMap.InfoWindowClickEventArgs e)
    {
      var customPin = GetCustomPin(e.Marker);
      if (customPin == null)
      {
        throw new Exception("Custom pin not found");
      }

      if (!string.IsNullOrWhiteSpace(customPin.Url))
      {
        var url = Android.Net.Uri.Parse(customPin.Url);
        var intent = new Intent(Intent.ActionView, url);
        intent.AddFlags(ActivityFlags.NewTask);
        Android.App.Application.Context.StartActivity(intent);
      }
    }

    protected override MarkerOptions CreateMarker(Pin pin)
    {
      var marker = new MarkerOptions();
      marker.SetPosition(new LatLng(10.468870294698933, 77.64990631397154));
      marker.SetTitle("This is Naveen");
      marker.SetSnippet(pin.Address);
      marker.SetIcon(BitmapDescriptorFactory.FromResource(Resource.Drawable.pin));
      return marker;
    }

    public global::Android.Views.View GetInfoContents(Marker marker)
    {
      var inflater = global::Android.App.Application.Context.GetSystemService(Context.LayoutInflaterService) as global::Android.Views.LayoutInflater;
      if (inflater != null)
      {
        global::Android.Views.View view;

        var customPin = GetCustomPin(marker);
        if (customPin == null)
        {
          throw new Exception("Custom pin not found");
        }
        view = inflater.Inflate(Resource.Layout.mtrl_layout_snackbar_include, null);
        if (customPin.Name.Equals("Xamarin"))
        {
          view = inflater.Inflate(Resource.Drawable.naveenimg, null);
        }
        else
        {
          view = inflater.Inflate(Resource.Drawable.pickawood, null);
        }

        var infoTitle = view.FindViewById<TextView>(Resource.Drawable.nessie);
        var infoSubtitle = view.FindViewById<TextView>(Resource.Drawable.pickawood);

        if (infoTitle != null)
        {
          infoTitle.Text = marker.Title;
        }
        if (infoSubtitle != null)
        {
          infoSubtitle.Text = marker.Snippet;
        }

        return view;
      }
      return null;
    }

    public global::Android.Views.View GetInfoWindow(Marker marker)
    {
      return null;
    }

    CustomPin GetCustomPin(Marker annotation)
    {
      var position = new Position(annotation.Position.Latitude, annotation.Position.Longitude);
      foreach (var pin in customPins)
      {
        if (pin.Position == position)
        {
          return pin;
        }
      }
      return null;
    }
  }
}
