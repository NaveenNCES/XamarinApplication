using Prism.Commands;
using System;
using Xamarin.Forms;
using XamarinApp.Resx;

namespace XamarinApp.ViewModels
{
  public class GesturePageViewModel : ViewModelBase
  {
    private string _countLable;
    public int TapCount = 0;
    public bool MyBoolean = false;

    public EventHandler<SwipedEventArgs> Swipe;
    public DelegateCommand<object> TapCommand { get; }
    public string CountLable
    {
      get { return _countLable; }
      set { SetProperty(ref _countLable, value); }
    }

    public GesturePageViewModel()
    {
      TapCommand = new DelegateCommand<object>(OnTapGestureRecognizerTapped);
    }

    public void OnTapGestureRecognizerTapped(object e)
    {
      TapCount++;
      CountLable = AppResource.IWasTapped + TapCount + AppResource.Times;
      if(e != null)
      {
        var direction = e.ToString();
        if (direction == AppResource.Left)
          CountLable = AppResource.SwipLeft;
        else if (direction == AppResource.Right)
          CountLable = AppResource.SwipRight;
        else if (direction == AppResource.Up)
          CountLable = AppResource.SwipUp;
        else if (direction == AppResource.Down)
          CountLable = AppResource.SwipDown;
      }      
    }
  }
}
