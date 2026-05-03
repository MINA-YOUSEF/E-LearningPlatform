using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.DTOs.Media
{
    public class MediaUploadResultDto
    {

        public string PublicId { get; set; } = null!;
        public string Url { get; set; } = null!;
        public string? ThumbnailUrl { get; set; }

    }
}
