using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema.Services
{
  public class CourseRepository
  {
    private readonly IDbContextFactory<DBContext> _contextFactory;

    public CourseRepository(IDbContextFactory<DBContext> contextFactory)
    {
      _contextFactory = contextFactory;
    }

    public async Task<IEnumerable<CourseNames>> GetAll()
    {
      using (DBContext context = _contextFactory.CreateDbContext())
      {
        return await context.Courses.ToListAsync();
      }
    }

    public async Task<CourseNames> GetById(Guid courseID)
    {
      using (DBContext context = _contextFactory.CreateDbContext())
      {
        return await context.Courses.FindAsync(courseID);
      }
    }

    public async Task<IEnumerable<CourseNames>> GetManyByIds(IReadOnlyList<Guid> courseIds)
    {
      using (DBContext context = _contextFactory.CreateDbContext())
      {
        return await context.Courses.Where(x => courseIds.Contains(x.Id)).ToListAsync();
      }
    }

    public async Task<CourseNames> Create(CourseNames course)
    {
      using(DBContext context = _contextFactory.CreateDbContext())
      {
        context.Courses.Add(course);
        await context.SaveChangesAsync();

        return course;
      }
    }

    public async Task<CourseNames> Update(CourseNames course)
    {
      using(DBContext context = _contextFactory.CreateDbContext())
      {
        context.Courses.Update(course);
        await context.SaveChangesAsync();

        return course;
      }
    }
    public async Task<bool> Delete(Guid id)
    {
      using(DBContext context = _contextFactory.CreateDbContext())
      {
        CourseNames course = new CourseNames()
        {
          Id = id
        };
        context.Courses.Remove(course);
        await context.SaveChangesAsync();

        return true;
      }
    }
  }
}
