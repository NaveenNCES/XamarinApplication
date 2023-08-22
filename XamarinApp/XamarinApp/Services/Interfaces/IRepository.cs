using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace XamarinApp.Services.Interfaces
{
  public interface IRepository<T> where T : class, new()
  {
    Task<T> GetUser(T id);
    Task<int> Insert(T entity);
    Task<int> Update(T entity);
    Task<int> Delete(T entity);
    Task<bool> FindUser(T entity);
    Task<List<T>> GetAllDetailsAsync();
  }
}
