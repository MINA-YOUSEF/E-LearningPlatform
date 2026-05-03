using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using E_LearningPlatform.Application.DTOs.Media;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Infrastructure.Options;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class CloudinaryService : ICloudinaryService
    {
        private readonly CloudinaryOptions _options;
        private readonly Cloudinary _cloudinary;
        public CloudinaryService(IOptions<CloudinaryOptions> options)
        {
            _options = options.Value;
            var account = new Account(_options.CloudName, _options.ApiKey, _options.ApiSecret);
            _cloudinary = new Cloudinary(account) { Api = { Secure = true } };

        }
        public async Task DeleteAsync(string publicId, CancellationToken cancellationToken = default)
        {

            var imageDelete = await _cloudinary.DestroyAsync(new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Image
            });

            if (imageDelete.Error is null && IsDeleted(imageDelete.Result))
                return;

            var videoDelete = await _cloudinary.DestroyAsync(new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Video
            });

            if (videoDelete.Error is null && IsDeleted(videoDelete.Result))
                return;

            var rawDelete = await _cloudinary.DestroyAsync(new DeletionParams(publicId)
            {
                ResourceType = ResourceType.Raw
            });

            if (rawDelete.Error is not null)
                throw new BadRequestException(rawDelete.Error.Message);
        }

        public async Task<MediaUploadResultDto> UploadFileAsync(Stream fileBytes, string fileName, CancellationToken cancellationToken = default)
        {
            var uploadParams = new RawUploadParams
            {
                File = new FileDescription(fileName, fileBytes),
                Folder = "E-LearningPlatform/files"
            };

            var result = await _cloudinary.UploadAsync(uploadParams, "raw", cancellationToken);
            if (result.Error is not null || string.IsNullOrWhiteSpace(result.SecureUrl?.AbsoluteUri))
            {
                throw new BadRequestException(result.Error?.Message ?? "Cloudinary file upload failed.");
            }

            return new MediaUploadResultDto
            {
                PublicId = result.PublicId,
                Url = result.SecureUrl.AbsoluteUri,
                ThumbnailUrl = null

            };
        }

        public async Task<MediaUploadResultDto> UploadImageAsync(Stream fileBytes, string fileName, CancellationToken cancellationToken = default)
        {
            var uploadPrams = new ImageUploadParams

            {
                File = new FileDescription(fileName, fileBytes),
                Folder = "E-LearningPlatform/Images"
            };
            var uploadResult = await _cloudinary.UploadAsync(uploadPrams, cancellationToken);
            if (uploadResult.Error != null || string.IsNullOrWhiteSpace(uploadResult.SecureUrl?.AbsoluteUri))
            {
                throw new Exception(uploadResult.Error?.Message ?? "Cloudinary  upload failed.");
            }
            return new MediaUploadResultDto
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri,
                ThumbnailUrl = uploadResult.SecureUrl.AbsoluteUri
            };
        }

        public async Task<MediaUploadResultDto> UploadVideoAsync(Stream fileBytes, string fileName, CancellationToken cancellationToken = default)
        {
            var uploadPrams = new VideoUploadParams
            {
                File = new FileDescription(fileName, fileBytes),
                Folder = "E-LearningPlatform/Videos"
            };
            var uploadResult = await _cloudinary.UploadLargeAsync(uploadPrams, 6_000_000, cancellationToken);
            if (uploadResult.Error != null || string.IsNullOrWhiteSpace(uploadResult.SecureUrl?.AbsoluteUri))
            {
                throw new Exception(uploadResult.Error?.Message ?? "Cloudinary  upload failed.");
            }
            var thumbnailImageUrl =
                _cloudinary.Api.UrlVideoUp.Transform(
                    new Transformation()
                     .Width(640)
                .Height(360)
                .Gravity("auto")
                .FetchFormat("jpg")
                .Quality("auto")).BuildUrl(uploadResult.PublicId);
            
            return new MediaUploadResultDto
            {
                PublicId = uploadResult.PublicId,
                Url = uploadResult.SecureUrl.AbsoluteUri,
                ThumbnailUrl = thumbnailImageUrl
            };
        }

        private static bool IsDeleted(string? result)
        {
            return string.Equals(result, "ok", StringComparison.OrdinalIgnoreCase)
                || string.Equals(result, "not found", StringComparison.OrdinalIgnoreCase);
        }

    }
}
