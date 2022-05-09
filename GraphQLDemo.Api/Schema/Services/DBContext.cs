using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema.Services
{
  public class DBContext : DbContext
  {
    public DBContext(DbContextOptions<DBContext> options):base(options)
    {        
    }
    public DbSet<CourseNames> Courses { get; set; }
  }
}
