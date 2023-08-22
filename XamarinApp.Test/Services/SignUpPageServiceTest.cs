using AutoFixture;
using FluentAssertions;
using Moq;
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
  public class SignUpPageServiceTest
  {
    private readonly SignUpUserService _signUpUserService;
    private readonly Fixture _fixture = new Fixture();
    private readonly Mock<IRepository<UserModel>> _iRepository;

    public SignUpPageServiceTest()
    {
      _iRepository = new Mock<IRepository<UserModel>>();
      _signUpUserService = new SignUpUserService(_iRepository.Object);
    }

    [Fact]
    public async void Given_UserDetail_Save_in_DB()
    {
      //Arrange
      var user = _fixture.Create<UserModel>();

      //Act
      var result =await _signUpUserService.SaveUserAsync(user);

      //Arrange
      result.Should().BeTrue();
    }
  }
}
