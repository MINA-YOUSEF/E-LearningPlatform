using AutoMapper;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using Microsoft.EntityFrameworkCore;
namespace E_LearningPlatform.Infrastructure.Services
{
    public class CourseAuthorizationService : ICourseAuthorizationService
    {
        private readonly IUnitOfWork _unitOfWork;


        private readonly ICurrentUserService _currentUserService;

        public CourseAuthorizationService (
            IUnitOfWork unitOfWork,
            ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task EnsureInstructorOwnsCourseAsync (
     int courseId,
     CancellationToken cancellationToken = default)
        {
            if (!_currentUserService.UserId.HasValue)
            {
                throw new UnauthorizedAccessException(
                    "User is not authenticated.");
            }

            var course = await _unitOfWork.Courses
                .Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(
                    x => x.Id == courseId,
                    cancellationToken);

            if (course == null)
            {
                throw new InvalidOperationException(
                    "Course not found.");
            }

            if (!course.IsOwnedBy(
                _currentUserService.UserId.Value))
            {
                throw new UnauthorizedAccessException(
                    "You do not own this course.");
            }
        }
    }
}
