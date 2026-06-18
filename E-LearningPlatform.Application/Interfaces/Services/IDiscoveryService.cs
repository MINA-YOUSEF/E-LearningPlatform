using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs;
using E_LearningPlatform.Application.DTOs.Discovery;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IDiscoveryService
    {
        Task<PagedResult<TopRatedCourseDto>>
            GetTopRatedCourseAsync (PagedRequest request, CancellationToken cancellationToken = default);
        Task<PagedResult<PopularCourseDto>> GetPopularCoursesAsync (PagedRequest request, CancellationToken cancellationToken = default);
        Task<PagedResult<CourseCardDto>>
        SearchCoursesAsync (
        CourseSearchRequestDto request,
        CancellationToken cancellationToken = default);
    }
}
