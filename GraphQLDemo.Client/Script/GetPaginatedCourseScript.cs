using StrawberryShake;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GraphQLDemo.Client.Script
{
  public class GetPaginatedCourseScript
  {
    private readonly IGraphQLDemoClient _client;

    public GetPaginatedCourseScript(IGraphQLDemoClient client)
    {
      _client = client;
    }

    public async Task Run()
    {
      ConsoleKey key;

      int? first = 5;
      string after = null;

      int? last = null;
      string before = null;

      do
      {
        IOperationResult<IPaginatedCourseResult> coursesResult = await _client.PaginatedCourse.ExecuteAsync(
            first, after, last, before,
            new List<CourseNamesSortInput>()
            {
                        new CourseNamesSortInput()
                        {
                            CourseName = SortEnumType.Asc
                        }
            });

        Console.WriteLine($"{"Name",-10} | {"Subject",-10}");
        Console.WriteLine();

        foreach (IPaginatedCourse_PaginatedCourses_Nodes course in coursesResult.Data.PaginatedCourses.Nodes)
        {
          Console.WriteLine($"{course.CourseName,-10}");
        }

        IPaginatedCourse_PaginatedCourses_PageInfo pageInfo = coursesResult.Data.PaginatedCourses.PageInfo;

        if (pageInfo.HasPreviousPage)
        {
          Console.WriteLine("Press 'A' to move to the previous page.");
        }

        if (pageInfo.HasNextPage)
        {
          Console.WriteLine("Press 'D' to move to the next page.");
        }

        Console.WriteLine("Press 'Enter' to exit.");

        key = Console.ReadKey().Key;

        if (key == ConsoleKey.A && pageInfo.HasPreviousPage)
        {
          last = 5;
          before = pageInfo.StartCursor;

          first = null;
          after = null;
        }
        else if (key == ConsoleKey.D && pageInfo.HasNextPage)
        {
          first = 5;
          after = pageInfo.EndCursor;

          last = null;
          before = null;
        }
      } while (key != ConsoleKey.Enter);
    }
  }
}
