using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
  public class UserLoginService : IUserLoginService
  {
    private readonly IRepository<UserModel> _userRepo;

    public UserLoginService(IRepository<UserModel> repository)
    {
      _userRepo = repository;
    }

    public async Task<bool> LoginUserAsync(UserModel user)
    {
      var result = await _userRepo.GetAllDetailsAsync();

      var userData = result.FirstOrDefault(x => x.Password == user.Password && x.UserName == user.UserName);

      if (userData != null)
      {
        return true;
      }

      return false;
    }
  }
}
