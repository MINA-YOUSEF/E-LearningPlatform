using System.Collections.Generic;

namespace E_LearningPlatform.Domain.Entities
{
    public class Question : BaseEntity
    {
        private readonly List<Answer> _answers = new();

        public string Text { get; private set; } = null!;
        public string Type { get; private set; } = null!;
        public int? Degree { get; private set; }
        public int QuizId { get; private set; }
        public Quiz Quiz { get; private set; }
        public IReadOnlyCollection<Answer> Answers => _answers.AsReadOnly();
    }
}
