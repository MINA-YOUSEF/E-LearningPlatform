using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public class CourseCompletedEvent : BaseDomainEvent
    {
        public int UserId { get; }
        public int CourseId { get; }
        public string CourseTitle { get; } = string.Empty;
        public CourseCompletedEvent (int userId, int courseId, string courseTitle)
        {
            UserId = userId;
            CourseId = courseId;
            CourseTitle = courseTitle;
        }
    }
}
