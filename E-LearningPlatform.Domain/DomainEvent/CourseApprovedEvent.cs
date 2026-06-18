using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public class CourseApprovedEvent : BaseDomainEvent
    {
        public int CourseId { get; private set; }
        public int InstructorId { get; private set; }
        public string CourseTitle { get; private set; } = string.Empty;
        public CourseApprovedEvent (int CourseId,
    int InstructorId,
    string CourseTitle)
        {
            this.CourseId = CourseId;
            this.InstructorId = InstructorId;
            this.CourseTitle = CourseTitle;
        }
    }

}

