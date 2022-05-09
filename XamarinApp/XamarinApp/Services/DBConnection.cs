using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using XamarinApp.Models;
using XamarinApp.Services.Interfaces;

namespace XamarinApp.Services
{
  public class DBConnection
  {
    private readonly SQLiteAsyncConnection _db;

    public DBConnection(string dbPath)
    {
      _db = new SQLiteAsyncConnection(dbPath);
      _db.CreateTableAsync<UserModel>().Wait();
    }

    public Task<List<UserModel>> GetUsers()
    {
      return _db.Table<UserModel>().ToListAsync();
    }

    public Task<UserModel> FindUser(UserModel user)
    {
      return _db.Table<UserModel>().Where(x => x.UserName == user.UserName && x.Password == user.Password).FirstOrDefaultAsync();
    }

    public Task<int> SaveUser(UserModel user)
    {
      if (user.ID != 0)
      {
        return _db.UpdateAsync(user);
      }
      else
      {
        return _db.InsertAsync(user);
      }
    }
  }
}
