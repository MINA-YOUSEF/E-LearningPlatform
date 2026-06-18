using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Course
{
    public class CourseApprovalDto
    {
        public int CourseId { get; set; }
        public string Title { get; set; }
        public int InstructorId { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.Now;
    }
}