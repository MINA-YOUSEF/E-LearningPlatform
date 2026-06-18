namespace E_LearningPlatform.Application.DTOs;

public class CourseCardDto
{
    public int CourseId { get; set; }

    public string Title { get; set; } = null!;


    public string Slug { get; set; } = null!;

    public string? ThumbnailUrl { get; set; }

    public decimal Price { get; set; }

    public string Currency { get; set; } = null!;

    public decimal AverageRating { get; set; }

    public int RatingCount { get; set; }

    public int EnrollmentCount { get; set; }
}