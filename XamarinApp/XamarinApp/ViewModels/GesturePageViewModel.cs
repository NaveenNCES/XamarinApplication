using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using Xamarin.Forms;
using Module1;

namespace XamarinApp.ViewModels
{
  public class GesturePageViewModel : ViewModelBase
  {
    private string _countLable;
    public int tapCount = 0;
    public Xamarin.Forms.SwipeDirection Direction { get; set; }

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

      //var swip = new SwipeGestureRecognizer { Direction = (SwipeDirection)e };
      //var a = new SwipedEventArgs(e, (SwipeDirection)e);
      //Direction = (SwipeDirection)e;
      tapCount++;
      CountLable = "I was tapped " + tapCount + " times";
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
