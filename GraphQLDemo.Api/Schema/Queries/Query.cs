using AutoFixture;
using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema
{
  public class Query
  {
    private List<CourseNames> _courseNames;
    private readonly Fixture _fixture = new Fixture();

    public Query()
    {
      _courseNames = _fixture.Build<CourseNames>().CreateMany(5).ToList();
    }
    public IEnumerable<CourseNames> GetCourses()
    {
      return _courseNames;
    }

    public async Task<CourseNames> GetCourseNameByIdAsync(Guid id)
    {
      await Task.Delay(1000);

      var course = _courseNames.Where(x => x.Id == id).FirstOrDefault();

      return course;
    }

    [GraphQLDeprecated("This query is deprecated")]
    public string Instruction => "This is a sample GraphQL Application";
  }
}
