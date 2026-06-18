using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Review
{
    public class ReviewResponseDto
    {
        public int Id { get; set; }

        public int CourseId { get; set; }

        public int UserId { get; set; }

        public int Rating { get; set; }

        public string Comment { get; set; } = null!;

        public string? Title { get; set; }

        public DateTime CreatedAtUtc { get; set; }

        public int HelpfulCount { get; set; }
    }
}
