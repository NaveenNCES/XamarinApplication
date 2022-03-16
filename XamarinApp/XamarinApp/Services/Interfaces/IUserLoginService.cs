using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;

namespace XamarinApp.Services.Interfaces
{
    public interface IUserLoginService
    {
        Task<bool> LoginUser(UserModel user);
    }
}
