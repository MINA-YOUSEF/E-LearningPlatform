using System;
using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Domain.Entities
{
    public class CourseApprovalHistory : BaseEntity
    {
        private CourseApprovalHistory() { }

        public CourseApprovalHistory(int courseId, ApprovalStatus status, int reviewedBy, string? notes)
        {
            CourseId = courseId;
            Status = status;
            ReviewedByAdminId = reviewedBy;
            Notes = notes;
            ReviewedAtUtc = DateTime.UtcNow;
        }

        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public ApprovalStatus Status { get; private set; }
        public int ReviewedByAdminId { get; private set; }
        public string? Notes { get; private set; }
        public DateTime ReviewedAtUtc { get; private set; }
    }
}
