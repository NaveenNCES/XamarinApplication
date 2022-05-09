using GraphQLDemo.Client.Script;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StrawberryShake;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace GraphQLDemo.Client
{
  class Program
  {
    static void Main(string[] args)
    {
      Host.CreateDefaultBuilder(args)
        .ConfigureServices((context, service) =>
        {
          string graphqlApiUrl = context.Configuration.GetValue<string>("GRAPHQL_API_URL");

          string httpGraphQLApiUrl = $"https://{graphqlApiUrl}";
          string webSocketsGraphQLApiUrl = $"ws://{graphqlApiUrl}";

          service.AddGraphQLDemoClient()
            .ConfigureHttpClient(x => x.BaseAddress = new Uri(httpGraphQLApiUrl))
            .ConfigureWebSocketClient(c => c.Uri = new Uri(webSocketsGraphQLApiUrl)); 


          service.AddHostedService<StartUp>();

          service.AddTransient<GetPaginatedCourseScript>();
        })
        .Build()
        .Run();
            
    }
  }

  public class StartUp : IHostedService
  {
    private readonly GetPaginatedCourseScript _getPaginatedCourseScript;

    public StartUp(GetPaginatedCourseScript getPaginatedCourseScript)
    {
      _getPaginatedCourseScript = getPaginatedCourseScript;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
      await _getPaginatedCourseScript.Run();
      //IOperationResult<IGetCoursesResult> result = await _client.GetCourses.ExecuteAsync();

      //if (result.IsErrorResult())
      //{
      //  Console.WriteLine("Failed to get instructio ns");
      //}
      //else
      //{
      //  foreach (IGetCourses_Courses course in result.Data.Courses)
      //  {
      //    Console.WriteLine($"{course.CourseName} is priced by {course.CourseFee}");
      //  }
      //}
      //Console.WriteLine();

      //var courseByIdRsult = await _client.GetCourseNameById.ExecuteAsync(Guid.Parse("23dcf65c-796b-4322-9a2c-0dd6d4b59251"));

      //if (result.IsErrorResult())
      //{
      //  Console.WriteLine("Failed to get Courses");
      //}
      //else
      //{
      //  var course = courseByIdRsult.Data.CourseNameById;
      //  Console.WriteLine($"{course.CourseName} is priced by {course.CourseFee}");

      //}

      //IOperationResult<ICreateCourseResult> createCourseResult = await _client.CreateCourse.ExecuteAsync(new CourseNamesInput()
      //{
      //  Id = Guid.NewGuid(),
      //  CourseName = "GraphQL",
      //  CourseFee = 799
      //});

      //Guid courseId = createCourseResult.Data.CreateCourse.Id;
      //var createdCourseName = createCourseResult.Data.CreateCourse.CourseName;

      //Console.WriteLine($"Successfully create course {createdCourseName}");

      //IOperationResult<IUpdateCourseResult> updateCourseResult = await _client.UpdateCourse.ExecuteAsync(new CourseNamesInput()
      //{
      //  Id = courseId,
      //  CourseName = "Tamil",
      //  CourseFee = 999
      //});

      //var updatedCourseName = updateCourseResult.Data.UpdateCourse.CourseName;

      //Console.WriteLine($"Successfully update course name to ${updatedCourseName}");

      //IOperationResult<IDeleteCourseResult> deleteCourse = await _client.DeleteCourse.ExecuteAsync(courseId);
      //bool deleteCourseSuccessfull = deleteCourse.Data.DeleteCourse;

      //if (deleteCourseSuccessfull)
      //{
      //  Console.WriteLine("Course deleted Successfully");
      //}

      //_client.CourseCreated.Watch().Subscribe(result =>
      //{
      //  string name = result.Data.CourseCreated.CourseName;
      //  Console.WriteLine($"Course {name} was created");
      //});

      //_client.CourseUpdated.Watch(courseId).Subscribe(result =>
      //{
      //  string name = result.Data.CourseUpdated.CourseName;
      //  Console.WriteLine($"Course {courseId} was renamed to {name}");
      //});

      Console.ReadKey();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
      return Task.CompletedTask;
    }
  }
}
