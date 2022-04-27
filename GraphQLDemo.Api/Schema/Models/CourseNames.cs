using GraphQLDemo.Api.DataLoaders;
using HotChocolate;
using HotChocolate.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema
{
  public class CourseNames
  {
    [GraphQLName("id")]
    public Guid Id { get; set; }
    public string CourseName { get; set; }
    [IsProjected(false)]
    public int CourseFee { get; set; }
  }
}
