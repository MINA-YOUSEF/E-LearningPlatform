using System;
using System.Collections.Generic;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Domain.Entities
{
    public class Section : BaseEntity
    {
        private readonly List<Lesson> _lessons = new();

        private Section() { }

        public Section(string title, int order, int courseId)
        {
            SetTitle(title);
            SetOrder(order);
            CourseId = courseId;
            IsLocked = false;
        }

        public string Title { get; private set; } = string.Empty;
        public string? Description { get; private set; }
        public int Order { get; private set; }
        public bool IsLocked { get; private set; }
        public Duration? TotalDuration { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public IReadOnlyCollection<Lesson> Lessons => _lessons.AsReadOnly();
        public int? QuizId { get; private set; }
        public Quiz? Quiz { get; private set; }

        public void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Section title is required.");
            Title = title.Trim();
        }

        public void SetOrder(int order)
        {
            if (order <= 0) throw new ArgumentOutOfRangeException(nameof(order), "Order must be positive.");
            Order = order;
        }
        public void SetDescription(string? description)
        {
            Description = description?.Trim();
        }
        public void Lock() => IsLocked = true;
        public void Unlock() => IsLocked = false;
    }
}
