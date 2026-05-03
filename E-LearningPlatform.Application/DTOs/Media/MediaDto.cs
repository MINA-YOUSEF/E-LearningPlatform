using E_LearningPlatform.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Domain.Entities
{
    public class MediaDto
    {
        public string PublicId { get; private set; } = null!;
        public string Url { get; private set; } = null!;
        public string? ThumbnailUrl { get; private set; }
        public string? Title { get; private set; }
        public MediaCategory Category { get; private set; }
        public MediaType Type { get; private set; }
        public long FileSize { get; private set; }
        public string MimeType { get; private set; } = string.Empty;
        public int? DurationSeconds { get; private set; }
        public string? Checksum { get; private set; }
        public StorageProvider StorageProvider { get; private set; }
        public MediaProcessingStatus ProcessingStatus { get; private set; }
        public int UploadedByUserId { get; private set; }
        public int? LessonId { get; private set; }
    }
}
