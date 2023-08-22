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
  public class LoginService : ILoginService
  {
    private readonly IRepository<UserModel> _userRepository;

    public LoginService(IRepository<UserModel> userRepository)
    {
      _userRepository = userRepository;
    }

    public async Task<bool> LoginUserAsync(UserModel user)
    {
      var result = await _userRepository.GetAllDetailsAsync();

      var userData = result.FirstOrDefault(x => x.Password == user.Password && x.UserName == user.UserName);

      if (userData != null)
      {
        return true;
      }

      return false;
    }
  }
}
