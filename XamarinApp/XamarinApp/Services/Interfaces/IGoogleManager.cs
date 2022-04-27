using System;
using System.Collections.Generic;
using System.Text;
using XamarinApp.Models;

namespace XamarinApp.Services.Interfaces
{
  public interface IGoogleManager
  {
    void Login(Action<GoogleUser, string> OnLoginComplete);

    void Logout();
  }
}
