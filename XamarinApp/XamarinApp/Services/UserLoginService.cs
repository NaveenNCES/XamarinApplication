using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
    public class UserLoginService : IUserLoginService
    {       
        public async Task<bool> LoginUser(UserModel user)
        {
            var userData =await App.Database.FindUser(user);

            if(userData != null)
            {
                return true;
            }

            return false;
        }
    }
}
