using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Dashboards.Admin
{
    public class AdminDashboardDto
    {
        public int TotalUsers { get; set; }

        public int TotalStudents { get; set; }

        public int TotalInstructors { get; set; }

        public int TotalCourses { get; set; }

        public int PublishedCourses { get; set; }

        public int PendingCourses { get; set; }

        public decimal TotalRevenue { get; set; }

        public int TotalEnrollments { get; set; }

        public List<RecentUserDto> RecentUsers { get; set; }
            = new();

        public List<RecentPaymentDto> RecentPayments { get; set; }
            = new();
    }
}
