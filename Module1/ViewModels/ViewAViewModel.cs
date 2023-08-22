using Android;
using Android.Widget;
//using Com.Xamarin.Textcounter;
using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Text;

namespace Module1.ViewModels
{
  public class ViewAViewModel : BindableBase
  {
    private string _title;
    public string Title
    {
      get { return _title; }
      set { SetProperty(ref _title, value); }
    }

    private bool _canUpdate = true;
    public bool CanUpdate
    {
      get { return _canUpdate; }
      set { SetProperty(ref _canUpdate, value); }
    }

    private string _updatedText;
    public string UpdatedText
    {
      get { return _updatedText; }
      set { SetProperty(ref _updatedText, value); }
    }
    private string _bindingText;
    public string BindingText
    {
      get { return _bindingText; }
      set { SetProperty(ref _bindingText, value); }
    }

    private string _vowels;
    public string Vowels
    {
      get { return _vowels; }
      set { SetProperty(ref _vowels, value); }
    }

    private string _consonents;
    public string Consonents
    {
      get { return _consonents; }
      set { SetProperty(ref _consonents, value); }
    }
    public DelegateCommand UpdateCommand { get; private set; }
    public DelegateCommand BindingCommand { get; private set; }
    public ViewAViewModel()
    {
      Title = "View A";
      UpdateCommand = new DelegateCommand(Update);
      BindingCommand = new DelegateCommand(JavaBinding);
    }

    private void JavaBinding()
    {
      //Vowels = TextCounter.NumVowels(BindingText);
      //Consonents = TextCounter.NumConsonants(BindingText);
    }

    private void Update()
    {
      UpdatedText = $"Updated: {DateTime.Now}";
    }
  }
}
