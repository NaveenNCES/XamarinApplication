using HotChocolate;
using HotChocolate.Execution;
using HotChocolate.Subscriptions;
using HotChocolate.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GraphQLDemo.Api.Schema.Subscriptions
{
  public class Subscription
  {
    [Subscribe]
    public CourseNames CourseCreated([EventMessage] CourseNames course) => course;

    [SubscribeAndResolve]
    public ValueTask<ISourceStream<CourseNames>> CourseUpdated(Guid courseId,[Service]ITopicEventReceiver topicEventReceiver)
    {
      var topicName = $"{courseId}_{nameof(Subscription.CourseUpdated)}";
      return topicEventReceiver.SubscribeAsync<string,CourseNames>(topicName);
    }
  }
}
