using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.OrderItem
{
    public class OrderItemDto
    {
        public   int CourseId {  get; set; }

        public string CourseTilte { get; set; } = null!;
        public decimal Amount { get; set; } 
        public decimal Currency { get; set; }   




    }
}
