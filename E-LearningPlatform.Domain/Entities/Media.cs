using E_LearningPlatform.Domain.Enums;

namespace E_LearningPlatform.Domain.Entities
{
    public class Media : BaseEntity
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
        public Lesson? Lesson { get; private set; }

        private Media() { }

        public Media(string publicId, string url, MediaCategory category, MediaType type, long fileSize, string mimeType, int uploadedByUserId)
        {
            PublicId = publicId;
            Url = url;
            Category = category;
            Type = type;
            FileSize = fileSize;
            MimeType = mimeType;
            StorageProvider = StorageProvider.Local;
            ProcessingStatus = MediaProcessingStatus.Pending;
            UploadedByUserId = uploadedByUserId;
        }

        public void MarkReady() => ProcessingStatus = MediaProcessingStatus.Ready;
        public void MarkFailed() => ProcessingStatus = MediaProcessingStatus.Failed;
        public void SetLessonId(int lessonId) => LessonId = lessonId; 
    }
}
