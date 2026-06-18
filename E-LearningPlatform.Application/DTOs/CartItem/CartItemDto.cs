using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.CartItem
{
    public class CartItemDto
    {
        public int CourseId { get; set; }

        public string Title { get; set; } = null!;

        public decimal Price { get; set; }

        public string? ThumbnailUrl { get; set; }
    }
}
