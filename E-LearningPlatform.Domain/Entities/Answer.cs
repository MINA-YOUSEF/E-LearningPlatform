namespace E_LearningPlatform.Domain.Entities
{
    public class Answer : BaseEntity
    {
        public string Text { get; private set; } = null!;
        public bool IsCorrect { get; private set; }
        public int QuestionId { get; private set; }
        public Question Question { get; private set; } = null!;
    }
}
