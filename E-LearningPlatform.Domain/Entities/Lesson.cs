using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Lesson : BaseEntity
    {
        private readonly List<LessonProgress> _progresses = new();
        private readonly List<Media> _mediaFiles = new();
        private readonly List<Comment> _comments = new();

        private Lesson() { }

        public Lesson(string title, int order, int sectionId, LessonContentType contentType, Duration duration)
        {
            SetTitle(title);
            SetOrder(order);
            SectionId = sectionId;
            ContentType = contentType;
            Duration = duration;
            IsActive = true;
        }

        public string Title { get; private set; } = string.Empty;
        public LessonContentType ContentType { get; private set; }
        public string? VideoUrl { get; private set; }
        public string? Transcript { get; private set; }
        public DateTime? ReleaseDate { get; private set; }
        public int Order { get; private set; }
        public Duration Duration { get; private set; }
        public bool IsActive { get; private set; }
        public bool IsPreview { get; private set; }
        public int SectionId { get; private set; }
        public Section Section { get; private set; }
        public IReadOnlyCollection<LessonProgress> Progresses => _progresses.AsReadOnly();
        public IReadOnlyCollection<Media> MediaFiles => _mediaFiles.AsReadOnly();
        public IReadOnlyCollection<Comment> Comments => _comments.AsReadOnly();

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Lesson title is required.");
            Title = title.Trim();
        }

        public void SetOrder(int order)
        {
            if (order <= 0) throw new ArgumentOutOfRangeException(nameof(order), "Order must be positive.");
            Order = order;
        }

        public void SetVideoUrl(string? url) => VideoUrl = url;
        public void SetTranscript(string? transcript) => Transcript = transcript;
        public void ScheduleRelease(DateTime releaseDate) => ReleaseDate = releaseDate;
        public void MarkPreview(bool isPreview) => IsPreview = isPreview;
    }
}
