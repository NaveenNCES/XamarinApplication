using GraphQLDemo.Api.DataLoaders;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema.Models
{
  public class CourseType
  {
    public Guid Id { get; set; }
    public string CourseName { get; set; }
    public int CourseFee { get; set; }
    public Guid InstructorID { get; set; }

    [GraphQLNonNullType]
    public async Task<CourseNames> DataLoaderCourse([Service] CourseDataLoaders courseDataLoader)
    {
      var courseDTO = await courseDataLoader.LoadAsync(InstructorID, CancellationToken.None);

      return courseDTO;
    }
  }
}
