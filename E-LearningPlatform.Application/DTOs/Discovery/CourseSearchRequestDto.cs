namespace E_LearningPlatform.Application.DTOs.Discovery;
public class CourseSearchRequestDto
{
    public string? Search { get; set; }

    public int? CategoryId { get; set; }

    public decimal? MinPrice { get; set; }

    public decimal? MaxPrice { get; set; }

    public decimal? MinRating { get; set; }

    public string? SortBy { get; set; }

    public bool Descending { get; set; } = true;

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}