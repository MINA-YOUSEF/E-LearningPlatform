namespace E_learnPlatform.API.Controllers.V1
{
    public class UpdateLessonProgressRequestDto
    {
        public int WatchedSeconds { get; set; }

        public bool Completed { get; set; }
    }
}
