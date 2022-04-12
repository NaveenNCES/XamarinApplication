using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
  public class Repository<T> : IRepository<T> where T : class, new()
 {
    private SQLiteAsyncConnection db;
    public Repository()
    {
      var con = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "userModel.db3");
      db = new SQLiteAsyncConnection(con);

      db.CreateTableAsync<T>().Wait();
    }
    public AsyncTableQuery<T> AsQueryable() => db.Table<T>();

    public async Task<int> Delete(T entity)
    {
      return await db.DeleteAsync<T>(entity);
    }
    public async Task<bool> FindUser(T entity)
    {
      var result = await db.FindAsync<T>(entity);

      if (result != null)
      {
        return true;
      }

      return false;
    }

    public async Task<T> GetUser(T enitity)
    {
      try
      {
        return await db.GetAsync<T>(enitity);
      }
      catch(Exception ex)
      {
        var a = ex.Message;
        var list = await db.Table<T>().ToListAsync();
        return list.Where(x => x == enitity).FirstOrDefault();
      }
    }

    public async Task<int> Insert(T entity)
    {
      return await db.InsertAsync(entity);
    }

    public async Task<int> Update(T entity)
    {
      return await db.UpdateAsync(entity);
    }
    public async Task<List<T>> GetAllDetailsAsync()
    {
      return await db.Table<T>().ToListAsync();
    }
  }
}
