using AutoFixture;
using AutoMapper;
using Flurl.Http.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services;
using Xunit;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.Test.Services
{
  public class RandomApiServiceTest
  {
    private readonly RandomApiService apiService;
    private readonly IMapper _mapper;
    private readonly Fixture _fixture = new Fixture();

    public RandomApiServiceTest()
    {
      var config = new MapperConfiguration(cfg =>
      {
        cfg.CreateMap<Result, ApiModel>();
      });
      var mapper = config.CreateMapper();
      _mapper = mapper;

      apiService = new RandomApiService(_mapper);
    }

    [Fact]
    public async void RadamApi_Returns_JsonData()
    {
      using (var httpTest = new HttpTest())
      {
        //Arrange
        httpTest.RespondWith("OK", 200);
        httpTest.ForCallsTo("https://randomuser.me/api/?results=50").AllowRealHttp();
        var root = _fixture.Create<Root>();

        //Act
        //_mapper.Setup(x => x.Map<List<Result>>(root)).Returns(root.Results);
        var result = await apiService.GetRandomApiDataAsync();

        //Assert
        httpTest.RespondWithJson("https://randomuser.me/api/?results=50");
        Assert.Equal(50, result.Count);
        Assert.Equal("System.Collections.Generic.List`1[XamarinApp.Models.ApiModel+Result]", result.GetType().ToString());
      }
    }
  }
}
