namespace E_LearningPlatform.Domain.Entities
{
    public class StudentQuizAttempt : BaseEntity
    {
        private StudentQuizAttempt() { }

        public StudentQuizAttempt(int studentId, int quizId, int score)
        {
            StudentId = studentId;
            QuizId = quizId;
            Score = score;
        }

        public int StudentId { get; private set; }
        public int QuizId { get; private set; }
        public Quiz Quiz { get; private set; } = null!;
        public int Score { get; private set; }
    }
}
