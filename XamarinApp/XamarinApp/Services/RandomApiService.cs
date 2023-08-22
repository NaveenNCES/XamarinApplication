using AutoMapper;
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
  public class RandomApiService : IRandomApiService
  {
    private readonly IMapper _mapper;

    public RandomApiService(IMapper mapper)
    {
      _mapper = mapper;
    }
    public async Task<List<Result>> GetRandomApiDataAsync()
    {
      string url = "https://randomuser.me/api/?results=50";
      var apidata = await url.GetJsonAsync<Root>();
      //var dataList = new List<Result>(apidata.Results);
      var dataList = _mapper.Map<List<Result>>(apidata.Results);

      return dataList;
    }
  }
}
