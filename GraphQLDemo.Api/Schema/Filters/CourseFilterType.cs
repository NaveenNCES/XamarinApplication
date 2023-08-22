using HotChocolate.Data.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema.Filters
{
  public class CourseFilterType : FilterInputType<CourseNames>
  {
    protected override void Configure(IFilterInputTypeDescriptor<CourseNames> descriptor)
    {
      descriptor.Ignore(x => x.CourseFee);
      base.Configure(descriptor);
    }
  }
}
