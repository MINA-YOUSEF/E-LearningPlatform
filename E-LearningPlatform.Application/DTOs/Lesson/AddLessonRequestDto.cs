using E_LearningPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;

namespace E_LearningPlatform.Application.DTOs.Lesson
{
    public class AddLessonRequestDto
    {
        public string Title { get; set; }

        public int SectionId { get; set; }

        public LessonContentType ContentType { get; set; }

        public int DurationInMinutes { get; set; }

        public IFormFile? VideoFile { get; set; }

        public IFormFile? File { get; set; }

        public string? Transcript { get; set; }

        public bool IsPreview { get; set; }

        public DateTime? ReleaseDate { get; set; }
    }
}
