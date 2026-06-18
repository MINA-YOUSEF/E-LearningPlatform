using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public class ReviewAddedEvent :BaseDomainEvent
    {
        public int ReviewId { get; }

        public int CourseId { get; }

        public int InstructorId { get; }

        public int StudentId { get; }

        public string CourseTitle { get; }

        public int Rating { get; }

        public ReviewAddedEvent (
            int reviewId,
            int courseId,
            int instructorId,
            int studentId,
            string courseTitle,
            int rating)
        {
            ReviewId = reviewId;
            CourseId = courseId;
            InstructorId = instructorId;
            StudentId = studentId;
            CourseTitle = courseTitle;
            Rating = rating;
        }
    }
}
