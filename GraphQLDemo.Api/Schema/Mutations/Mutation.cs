using GraphQLDemo.Api.DataLoaders;
using GraphQLDemo.Api.Schema.Services;
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
    private readonly CourseRepository _courseRepository;

    public Mutation(CourseRepository courseRepository)
    {
      _courseRepository = courseRepository;
    }
       
    public async Task<CourseNames> CreateCourse(CourseNames courseInput,[Service] ITopicEventSender topicEventSender)
    {

      var courseDTO = await _courseRepository.Create(courseInput);

      CourseNames courses = new CourseNames() { Id = courseDTO.Id, CourseName = courseDTO.CourseName, CourseFee = courseDTO.CourseFee };

      await topicEventSender.SendAsync(nameof(Subscription.CourseCreated), courses);

      return courses;
    }

    public async Task<CourseNames> UpdateCourse(CourseNames courseInput, [Service] ITopicEventSender topicEventSender)
    {
      var courseDTO = await _courseRepository.Update(courseInput);

      if(courseDTO == null)
      {
        throw new GraphQLException(new Error("Id not found","Can't update the value"));
      }

      CourseNames courses = new CourseNames() { Id = courseDTO.Id, CourseName = courseDTO.CourseName, CourseFee = courseDTO.CourseFee };

      string updateCourseTopic = $"{courses.Id}_{nameof(Subscription.CourseUpdated)}";

      await topicEventSender.SendAsync(updateCourseTopic, courses);

      return courses;
    }

    public async Task<bool> DeleteCourse(Guid id)
    {
      return await _courseRepository.Delete(id);
    }
  }
}
