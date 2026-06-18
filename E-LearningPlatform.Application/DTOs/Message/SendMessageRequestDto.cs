using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Message
{
    public class SendMessageRequestDto
    {
        public int ConversationId { get; set; }

        public string Content { get; set; } = null!;
    }
}
