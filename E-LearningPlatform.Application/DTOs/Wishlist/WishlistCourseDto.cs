using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Wishlist
{
    public class WishlistCourseDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string? ThumbnailUrl { get; set; }

        public double Rating { get; set; }
    }
}
