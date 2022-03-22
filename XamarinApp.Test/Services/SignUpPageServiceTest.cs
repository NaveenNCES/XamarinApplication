using AutoFixture;
using Moq;
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
  public class SignUpPageServiceTest
  {
    private readonly SignUpUserService _signUpUserService;
    private readonly Fixture _fixture = new Fixture();

    public SignUpPageServiceTest()
    {
      _signUpUserService = new SignUpUserService();
    }

    [Fact]
    public async void Given_UserDetail_Save_in_DB()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      var result =await _signUpUserService.SaveUser(user);

      //Arrange
      Assert.True(result);
    }
  }
}
