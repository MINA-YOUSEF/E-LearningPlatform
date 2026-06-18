using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Dashboards.Instructor
{
    public class InstructorDashboardDto
    {
        public decimal TotalRevenue { get; set; }
        public int TotalCourses { get; set; } = 0;
        public int TotalStudents { get; set; } = 0;
        public int PublishedCourses { get; set; } = 0;
        public decimal AverageRating { get; set; }

        public List<TopCourseDto> TopCourses { get; set; }
            = new();
        public List<RecentReviewDto> RecentReviews { get; set; }
        = new();

    }
}
