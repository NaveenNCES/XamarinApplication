using GraphQLDemo.Api.DataLoaders;
using GraphQLDemo.Api.Schema;
using GraphQLDemo.Api.Schema.Services;
using GraphQLDemo.Api.Schema.Subscriptions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api
{
  public class Startup
  {
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
      _configuration = configuration;
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    public void ConfigureServices(IServiceCollection services)
    {
      services.AddGraphQLServer()
        .AddQueryType<Query>()
        .AddMutationType<Mutation>()
        .AddSubscriptionType<Subscription>()
        .AddFiltering()
        .AddSorting()
        .AddProjections();

      services.AddInMemorySubscriptions();

      string connectionString = _configuration.GetConnectionString("default");
      services.AddPooledDbContextFactory<DBContext>(options => options.UseSqlite(connectionString));

      services.AddScoped<CourseRepository>();
      services.AddScoped<CourseDataLoaders>();
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseWebSockets();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapGraphQL();
      });
    }
  }
}
