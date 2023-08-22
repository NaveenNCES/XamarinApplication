using HotChocolate.Data.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema.NewFolder
{
  public class CourseSortType : SortInputType<CourseNames>
  {
    protected override void Configure(ISortInputTypeDescriptor<CourseNames> descriptor)
    {
      //descriptor.Field(x => x.Id).Name("CourseID");
      base.Configure(descriptor);
    }
  }
}
