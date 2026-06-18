using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Course;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class CourseApprovalService : ICourseApprovalService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;
        public CourseApprovalService (IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task ApproveAsync (
    int courseId,
    CancellationToken cancellationToken)
        {

            var course =
                await _unitOfWork.Courses
                .GetByIdAsync(
                    courseId,
                    cancellationToken);

            if (course == null)
            {
                throw new NotFoundException(
                    "Course not found.");
            }

            course.Approve(_currentUserService.UserId!.Value);
            course.Publish();
            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        public async Task<PagedResult<CourseApprovalDto>> GetPendingCoursesAsync (PagedRequest request, CancellationToken cancellationToken = default)
        {
            var query =
                   _unitOfWork.Courses
                       .Query()
                       .AsNoTracking()
                       .Where(x =>
                           x.ApprovalStatus ==
                           ApprovalStatus.Pending);
            var totalCount =
                await query.CountAsync(
                    cancellationToken);
            var courses =
                await query
                    .OrderBy(x => x.CreatedAtUtc)
                    .Skip(
                        (request.PageNumber - 1)
                        * request.PageSize)
                    .Take(request.PageSize)
                    .Select(x =>
                        new CourseApprovalDto
                        {
                            CourseId = x.Id,

                            Title = x.Title,

                            InstructorId =
                                x.InstructorId,

                        })
                    .ToListAsync(
                        cancellationToken);
            return new PagedResult<
                CourseApprovalDto>
            {
                Items = courses,

                TotalCount = totalCount,

                PageNumber =
                    request.PageNumber,

                PageSize =
                    request.PageSize
            };
        }

        public async Task RejectAsync (
      int courseId,
      string reason,
      CancellationToken cancellationToken)
        {
            var course =
                await _unitOfWork.Courses
                .GetByIdAsync(
                    courseId,
                    cancellationToken);

            if (course == null)
            {
                throw new NotFoundException(
                    "Course not found.");
            }

            course.Reject(_currentUserService.UserId!.Value, reason);


            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }

        public async Task SubmitAsync (int courseId, CancellationToken cancellationToken = default)
        {
            var course = await _unitOfWork.Courses
                            .Query()
                            .FirstOrDefaultAsync(
                                x => x.Id == courseId,
                                cancellationToken);

            if (course == null)
            {
                throw new NotFoundException(
                    "Course not found.");
            }

            if (course.InstructorId !=
                _currentUserService.UserId!.Value)
            {
                throw new UnauthorizedAccessException();
            }

            course.SubmitForReview();

            await _unitOfWork.SaveChangesAsync(
                cancellationToken);
        }


    }
}
