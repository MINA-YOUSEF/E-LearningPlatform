using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IMediaService
    {

        Task<Media> UploadLessonMediaAsync(
        int lessonId,
        IFormFile file,
        LessonContentType contentType,
        CancellationToken cancellationToken = default);

    Task<Media> ReplaceLessonMediaAsync(
        int lessonId,
        IFormFile file,
        LessonContentType contentType,
        CancellationToken cancellationToken = default);

    Task<Media> SetCourseThumbnailAsync(
        int courseId,
        IFormFile file,
        CancellationToken cancellationToken = default);

    Task DeleteMediaAsync(
        int mediaId,
        CancellationToken cancellationToken = default);

        //    Task<MediaDto> UploadVideoAsync(UploadMediaRequestDto request, CancellationToken cancellationToken = default);
        //    Task<MediaDto> UploadImageAsync(UploadMediaRequestDto request, CancellationToken cancellationToken = default);
        //    Task<MediaDto> UploadFileAsync(UploadMediaRequestDto request, CancellationToken cancellationToken = default);
        //    Task DeleteAsync(int mediaId, CancellationToken cancellationToken = default);
        //    Task<PagedResult<MediaDto>> GetPagedAsync(MediaPagedRequest request, CancellationToken cancellationToken = default);
        //
    }
}
