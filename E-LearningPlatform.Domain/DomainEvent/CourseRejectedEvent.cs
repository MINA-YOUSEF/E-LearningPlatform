namespace E_LearningPlatform.Domain.DomainEvent
{
    public class CourseRejectedEvent : BaseDomainEvent
    {
        public int CourseId { get; private set; }
        public int InstructorId { get; private set; }
        public string CourseTitle { get; private set; } = string.Empty;
        public string RejectionReason { get; private set; } = string.Empty;
        public CourseRejectedEvent (int CourseId, int InstructorId, string CourseTitle, string RejectionReason)
        {
            this.CourseId = CourseId;
            this.InstructorId = InstructorId;
            this.CourseTitle = CourseTitle;
            this.RejectionReason = RejectionReason;
        }
    }
}
