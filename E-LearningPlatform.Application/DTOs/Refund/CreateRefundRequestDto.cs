using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Refund
{
    public class CreateRefundRequestDto
    {
        public int PaymentId { get; set; }

        public string Reason { get; set; } = null!;
    }
}
