using GraphQLDemo.Api.Schema.Subscriptions;
using HotChocolate;
using HotChocolate.Subscriptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema
{
  public class Mutation
  {
    private readonly List<CourseNames> _courseNames;

    public Mutation()
    {
      _courseNames = new List<CourseNames>();
    }

    public async Task<List<CourseNames>> CreateCourse(CourseNames courseInput,[Service] ITopicEventSender topicEventSender)
    {
      CourseNames courses = new CourseNames() { Id = courseInput.Id, CourseName = courseInput.CourseName, CourseFee = courseInput.CourseFee };

      _courseNames.Add(courses);
      await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courses);

      return _courseNames;
    }
    public async Task<List<CourseNames>> UpdateCourse(Guid id,string courseName,int courseFee,[Service] ITopicEventSender topicEventSender)
    {
      CourseNames courses = _courseNames.FirstOrDefault(x => x.Id == id);

      if(courses == null)
      {
        throw new GraphQLException(new Error("Id not found","Can't update the value"));
      }
      courses.CourseName = courseName;
      courses.CourseFee = courseFee;
      string updateCourseTopic = $"{courses.Id}_{nameof(Subscription.CourseUpdated)}";

      await topicEventSender.SendAsync(updateCourseTopic, courses);

      return _courseNames;
    }

    public bool DeleteCourse(Guid id)
    {
      _courseNames.RemoveAll(x => x.Id == id);

      return true;
    }
  }
}
