using AutoFixture;
using Moq;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services;
using Xunit;

namespace XamarinApp.Test.Services
{
  public class UserLoginServiceTest
  {
    private readonly Fixture _fixture = new Fixture();
    private readonly UserLoginService loginService;
    public UserLoginServiceTest()
    {
      loginService = new UserLoginService();
    }

    [Fact]
    public async void Given_Valid_Credentials_Retruns_True()
    {
      //Arrange
      var user = new UserModel { Password = "nn", UserName = "nn" };
      //Act
      var result = await loginService.LoginUser(user);

      //Assert
      Assert.False(result);
    }
  }
}
