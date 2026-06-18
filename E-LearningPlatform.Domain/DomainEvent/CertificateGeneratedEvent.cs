using E_LearningPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.DomainEvent
{
    public class CertificateGeneratedEvent : BaseDomainEvent
    {
        public int UserId { get; }
        public string CourseTitle { get; } = null!;
        public CertificateGeneratedEvent (
        int userId,
        string courseTitle)
        {
            UserId = userId;
            CourseTitle = courseTitle;
        }
    }
}
