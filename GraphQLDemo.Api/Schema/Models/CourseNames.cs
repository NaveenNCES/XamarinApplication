using HotChocolate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema
{
  public class CourseNames
  {
    [GraphQLName("id")]
    public Guid Id { get; set; }
    public string CourseName { get; set; }
    public int CourseFee { get; set; }
  }
}
