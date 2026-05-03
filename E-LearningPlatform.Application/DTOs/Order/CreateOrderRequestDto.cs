using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Order
{
    public class CreateOrderRequestDto
    {
        public List<int> CourseIds { get; set; } = new();

    }
}
