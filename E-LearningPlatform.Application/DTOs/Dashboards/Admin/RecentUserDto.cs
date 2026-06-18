namespace E_LearningPlatform.Application.DTOs.Dashboards.Admin
{
    public class RecentUserDto
    {
        public int UserId { get; set; }

        public string FullName { get; set; } = null!;

        public string Email { get; set; } = null!;

        public DateTime CreatedAtUtc { get; set; }
    }
}
