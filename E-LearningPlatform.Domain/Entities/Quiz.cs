using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class Quiz : BaseEntity
    {
        private readonly List<Question> _questions = new();
        private readonly List<StudentQuizAttempt> _attempts = new();

        public int SectionId { get; private set; }
        public Section Section { get; private set; } = null!;
        public string Title { get; private set; } = null!;
        public int? TimeLimit { get; private set; }
        public IReadOnlyCollection<Question> Questions => _questions.AsReadOnly();
        public IReadOnlyCollection<StudentQuizAttempt> StudentQuizAttempts => _attempts.AsReadOnly();
    }
}
