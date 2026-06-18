using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Certificates
{
    public class CertificateResponseDto
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = string.Empty;

        public int MediaId { get; set; }

        public string FileUrl { get; set; } = string.Empty;

        public DateTime IssuedAtUtc { get; set; }

        public string CertificateNumber { get; set; } = string.Empty;

        public string VerificationCode { get; set; } = string.Empty;

        public string RecipientName { get; set; } = string.Empty;

        public string? Grade { get; set; }

        public DateTime? ExpiryDate { get; set; }
    }
}
