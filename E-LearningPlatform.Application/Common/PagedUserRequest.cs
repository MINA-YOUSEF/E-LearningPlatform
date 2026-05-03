namespace E_LearningPlatform.Application.Common
{
    public class PagedUserRequest : PagedRequest
    {
        public string? Role { get; set; }

        public string? Search { get; set; }

        public bool? IsActive { get; set; }

        public bool? EmailConfirmed { get; set; }

        public DateTime? CreatedFrom { get; set; }

    }
}
