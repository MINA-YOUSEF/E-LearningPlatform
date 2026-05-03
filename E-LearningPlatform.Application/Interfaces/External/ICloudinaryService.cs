using E_LearningPlatform.Application.DTOs.Media;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.External
{
    public interface ICloudinaryService
    {
        public Task<MediaUploadResultDto> UploadImageAsync(Stream fileBytes, string fileName, CancellationToken cancellationToken = default);

        public Task<MediaUploadResultDto> UploadVideoAsync(Stream fileBytes, string fileName, CancellationToken cancellationToken = default);
        public Task<MediaUploadResultDto> UploadFileAsync(Stream fileBytes, string fileName, CancellationToken cancellationToken = default);
        public Task DeleteAsync(string publicId, CancellationToken cancellationToken = default);

    }
}
