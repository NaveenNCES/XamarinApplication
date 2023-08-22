using GraphQLDemo.Api.Schema;
using GraphQLDemo.Api.Schema.Services;
using GreenDonut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.DataLoaders
{
  public class CourseDataLoaders : BatchDataLoader<Guid, CourseNames>
  {
    private readonly CourseRepository _courseRepository;
    public CourseDataLoaders(CourseRepository courseRepository,IBatchScheduler batchScheduler,
      DataLoaderOptions options = null) : base(batchScheduler, options)
    {
      _courseRepository = courseRepository;
    }

    protected override async Task<IReadOnlyDictionary<Guid, CourseNames>> LoadBatchAsync(IReadOnlyList<Guid> keys, CancellationToken cancellationToken)
    {
      IEnumerable<CourseNames> courseNames =await _courseRepository.GetManyByIds(keys);

      return courseNames.ToDictionary(x => x.Id);
    }
  }
}
