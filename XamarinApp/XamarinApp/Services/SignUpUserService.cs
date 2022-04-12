using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
  public class SignUpUserService : ISignUpUserService
  {
    //private readonly IRepository<UserModel> _userRepo;
    private readonly IRepository<UserModel> _userRepo;
    public SignUpUserService(IRepository<UserModel> repository)
    {
      _userRepo = repository;
    }
    public async Task<bool> SaveUserAsync(UserModel user)
    {
      await _userRepo.Insert(new UserModel
      {
        UserName = user.UserName,
        Password = user.Password
      });

      return true;

    }
  }
}
