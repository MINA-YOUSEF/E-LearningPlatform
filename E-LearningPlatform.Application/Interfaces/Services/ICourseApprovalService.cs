using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Course;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ICourseApprovalService
    {

        Task SubmitAsync (
                int courseId,
                CancellationToken cancellationToken = default);

        Task ApproveAsync (
            int courseId,
            CancellationToken cancellationToken = default);

        Task RejectAsync (
            int courseId,
            string reason,
            CancellationToken cancellationToken = default);

        Task<PagedResult<CourseApprovalDto>>
                GetPendingCoursesAsync (
                    PagedRequest request,
                    CancellationToken cancellationToken = default);

    }
}