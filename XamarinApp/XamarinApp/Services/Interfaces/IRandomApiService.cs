using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using static XamarinApp.Models.ApiModel;

namespace XamarinApp.Services.Interfaces
{
  public interface IRandomApiService
  {
    Task<List<Result>> GetRandomApiDataAsync();
  }
}
