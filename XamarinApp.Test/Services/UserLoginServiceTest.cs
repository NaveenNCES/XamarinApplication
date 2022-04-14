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
      _repo = new Mock<IRepository<UserModel>>();
      _loginService = new UserLoginService(_repo.Object);
    }

    [Fact]
    public async void Given_Valid_Credentials_Retruns_True()
    {
      //Arrange
      var fixture = _fixture.Build<UserModel>().CreateMany(5).ToList();
      var specifiedUser = fixture.FirstOrDefault();

      //Act
      _repo.Setup(x => x.GetAllDetailsAsync()).ReturnsAsync(fixture);
      var result = await _loginService.LoginUserAsync(specifiedUser);

      //Assert
      Assert.True(result);
    }

    [Fact]
    public async void Given_InValid_Credentials_Retruns_False()
    {
      //Arrange
      var fixture = _fixture.Build<UserModel>().CreateMany(5).ToList();      
      var specifiedUser = new UserModel { Password = "admin", UserName = "admin" };

      //Act
      _repo.Setup(x => x.GetAllDetailsAsync()).ReturnsAsync(fixture);
      var result = await _loginService.LoginUserAsync(specifiedUser);

      //Assert
      Assert.False(result);
    }
  }
}
