using AutoFixture;
using GraphQLDemo.Api.Schema.Filters;
using GraphQLDemo.Api.Schema.Models;
using GraphQLDemo.Api.Schema.NewFolder;
using GraphQLDemo.Api.Schema.Services;
using HotChocolate;
using HotChocolate.Data;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema
{
  public class Query
  {
    private readonly CourseRepository _courseRepository;

    public Query(CourseRepository courseRepository)
    {
      _courseRepository = courseRepository;
    }

    [UsePaging(IncludeTotalCount = true,DefaultPageSize =10)]
    [UseSorting]
    public async Task<IEnumerable<CourseType>> GetCourses()
    {
      var result = await _courseRepository.GetAll();

      return result.Select(c => new CourseType() { Id = c.Id, CourseFee = c.CourseFee, CourseName = c.CourseName,InstructorID=c.Id });
    }

    [UseDbContext(typeof(DBContext))]
    [UsePaging(IncludeTotalCount = true,DefaultPageSize =10)]
    [UseProjection]
    [UseFiltering(typeof(CourseFilterType))]
    [UseSorting(typeof(CourseSortType))]
    public IQueryable<CourseNames> GetPaginatedCourses([ScopedService] DBContext context)
    {
      return context.Courses.Select(c => new CourseNames() { Id = c.Id, CourseFee = c.CourseFee, CourseName = c.CourseName });
    } 

    [UseOffsetPaging(IncludeTotalCount = true,DefaultPageSize =10)]
    public async Task<IEnumerable<CourseType>> GetOffSetCourses()
    {
      var result = await _courseRepository.GetAll();

      return result.Select(c => new CourseType() { Id = c.Id, CourseFee = c.CourseFee, CourseName = c.CourseName,InstructorID=c.Id });
    }

    public async Task<CourseNames> GetCourseNameByIdAsync(Guid id)
    {
      return await _courseRepository.GetById(id);
    }

    [GraphQLDeprecated("This query is deprecated")]
    public string Instruction => "This is a sample GraphQL Application";
  }
}
