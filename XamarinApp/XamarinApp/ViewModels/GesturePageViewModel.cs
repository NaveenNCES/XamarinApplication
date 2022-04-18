using Prism.Commands;
using System;
using Xamarin.Forms;
using XamarinApp.Resx;

namespace XamarinApp.ViewModels
{
  public class GesturePageViewModel : ViewModelBase
  {
    private string _countLable;
    public int tapCount = 0;
    public bool MyBoolean = false;
    public SwipeDirection Direction { get; set; }

    public EventHandler<SwipedEventArgs> Swipe;
    public DelegateCommand<object> TapCommand { get; set; }
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
      tapCount++;
      CountLable = AppResource.IWasTapped + tapCount + AppResource.Times;
      if(e != null)
      {
        var direction = e.ToString();
        if (direction == "Left")
          CountLable = "Swip left";
        else if (direction == "Right")
          CountLable = "Swip Right";
        else if (direction == "Up")
          CountLable = "Swip Up";
        else if (direction == "Down")
          CountLable = "Swip Down";
      }      
    }
  }
}
