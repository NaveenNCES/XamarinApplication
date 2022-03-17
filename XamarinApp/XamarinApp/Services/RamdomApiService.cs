using Flurl.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.Services
{
  public class RamdomApiService : IRandomApiService
  {
    public async Task<List<Result>> getRandomApiData()
    {
      string url = "https://randomuser.me/api/?results=50";

      var apidata = await url.GetJsonAsync<Root>();
      var dataList = new List<Result>(apidata.Results);

      return dataList;
    }

  }
}
