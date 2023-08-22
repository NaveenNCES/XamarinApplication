using GraphQLDemo.Api.Schema.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api
{
  public class Program
  {
    public static void Main(string[] args)
    {
      IHost host = CreateHostBuilder(args).Build();

      using (IServiceScope scope = host.Services.CreateScope())
      {
        IDbContextFactory<DBContext> contextFactory =
          scope.ServiceProvider.GetRequiredService<IDbContextFactory<DBContext>>();

        using(DBContext context = contextFactory.CreateDbContext())
        {
          context.Database.Migrate(); 
        }
      };

      host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              webBuilder.UseStartup<Startup>();
            });
  }
}
