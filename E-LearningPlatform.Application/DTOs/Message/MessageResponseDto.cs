using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Message
{
    public class MessageResponseDto
    {
        public int Id { get; set; }

        public int SenderId { get; set; }

        public int ReceiverId { get; set; }

        public string Content { get; set; } = null!;

        public bool IsRead { get; set; }

        public bool IsEdited { get; set; }

        public DateTime CreatedAtUtc { get; set; }
    }
}
