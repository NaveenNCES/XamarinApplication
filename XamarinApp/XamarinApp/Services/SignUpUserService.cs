using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
  public class SignUpUserService : ISignUpUserService
  {
    public async Task<bool> SaveUser(UserModel user)
    {
       await App.Database.SaveUser(new UserModel
      {
        UserName = user.UserName,
        Password = user.Password,
      });

      return true;

    }
  }
}
