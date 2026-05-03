
using Microsoft.AspNetCore.Http;

namespace E_LearningPlatform.Application.DTOs.Course
{
    public class AddCourseRequestDto
    {


        public string Title { get; set; }

        public string? Subtitle { get; set; }

        public string Description { get; set; }

        public string Language { get; set; }

        public string Slug { get; set; }

        public string? LearningObjectives { get; set; }

        public string? TargetAudience { get; set; }

        public string? Prerequisites { get; set; }

        public decimal PriceAmount { get; set; }

        public string Currency { get; set; } = "USD";

        public decimal? DiscountPercentage { get; set; }

        public DateTime? DiscountEndDateUtc { get; set; }

        public int? TotalDurationMinutes { get; set; }

        public IFormFile? ThumbnailFile { get; set; }
    }
}


