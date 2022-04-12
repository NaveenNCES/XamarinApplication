using Flurl.Http.Testing;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Services;
using Xunit;

namespace XamarinApp.Test.Services
{
  public class RandomApiServiceTest
  {
    private readonly RamdomApiService apiService;


    public RandomApiServiceTest()
    {
      apiService = new RamdomApiService();
    }

    [Fact]
    public async void RadamApi_Returns_JsonData()
    {
      using(var httpTest = new HttpTest())
      {
        //Arrange
        httpTest.RespondWith("OK", 200);
        httpTest.ForCallsTo("https://randomuser.me/api/?results=50").AllowRealHttp();

        //Act
        var result = await apiService.GetRandomApiDataAsync();

        //Assert
        httpTest.RespondWithJson("https://randomuser.me/api/?results=50");
        Assert.Equal(50, result.Count);
        Assert.Equal("System.Collections.Generic.List`1[XamarinApp.Models.ApiModel+Result]", result.GetType().ToString());
      }
    }
  }
}
