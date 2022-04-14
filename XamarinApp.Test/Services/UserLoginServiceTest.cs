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
using XamarinApp.Services.Interfaces;
using Xunit;

namespace XamarinApp.Test.Services
{
  public class UserLoginServiceTest
  {
    private readonly Fixture _fixture = new Fixture();
    private readonly UserLoginService _loginService;
    private readonly Mock<IRepository<UserModel>> _repo;
    public UserLoginServiceTest()
    {
      _loginService = new UserLoginService(_repo.Object);
    }

    [Fact]
    public async void Given_Valid_Credentials_Retruns_True()
    {
      //Arrange
      var user = new UserModel { Password = "nn", UserName = "nn" };
      //Act
      var result = await _loginService.LoginUserAsync(user);

      //Assert
      Assert.False(result);
    }
  }
}
