using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
    public class UserLoginService : IUserLoginService
    {
        public bool LoginUser(UserModel user)
        {

            if (user.UserName == "naveenchpt@gmail.com" && user.Password == "naveen")
            {
                return true;
            }

            return false;
        }
    }
}
