using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class Conversation : BaseEntity
    {
        private readonly List<Message> _messages = new();
        private Conversation() { }

        public Conversation(int studentId, int instructorId, int courseId)
        {
            StudentId = studentId;
            InstructorId = instructorId;
            CourseId = courseId;
        }

        public int StudentId { get; private set; }
        public int InstructorId { get; private set; }
        public int CourseId { get; private set; }
        public Course Course { get; private set; }
        public IReadOnlyCollection<Message> Messages => _messages.AsReadOnly();
    }
}
