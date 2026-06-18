using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Conversation
{
    public class ConversationDto
    {
        public int Id { get; set; }

        public int StudentId { get; set; }

        public int InstructorId { get; set; }

        public int CourseId { get; set; }

        public string CourseTitle { get; set; } = null!;

        public DateTime? LastMessageAt { get; set; }

        public int UnreadCount { get; set; }
    }
}
