using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.ContinueLearning
{
    public class ContinueLearningResponseDto
    {
        public int CourseId { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseSlug { get; set; } = null!;
        public string CourseThumbnailUrl { get; set; } = null!; 
        public decimal ProgressPercent { get; set; }
        public int LastLessonId { get; set; }
        public string LastLessonTitle { get; set;} = null!;
        public DateTime LastAccessedAt { get; set; } 
    }
}
