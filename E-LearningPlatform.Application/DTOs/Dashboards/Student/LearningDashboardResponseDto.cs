using E_LearningPlatform.Application.DTOs.ContinueLearning;
using E_LearningPlatform.Application.DTOs.Enrollment;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Dashboards.Student
{
    public class LearningDashboardResponseDto
    {
        public int TotalEnrollments { get; set; }

        public int CompletedCourses { get; set; }

        public int InProgressCourses { get; set; }

        public List<ContinueLearningResponseDto>
            ContinueLearning
        { get; set; } = new();

        public List<RecentEnrollmentDto>
            RecentEnrollments
        { get; set; } = new();
    }
}
