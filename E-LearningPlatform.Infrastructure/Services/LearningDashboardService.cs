using AutoMapper;
using E_LearningPlatform.Application.DTOs.Dashboards.Student;
using E_LearningPlatform.Application.DTOs.Enrollment;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Enums;
using Microsoft.EntityFrameworkCore;


namespace E_LearningPlatform.Infrastructure.Services
{
    public class LearningDashboardService : ILearningDashboardService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly IEnrollmentService _enrollmentService;

        public LearningDashboardService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            IEnrollmentService enrollmentService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _enrollmentService = enrollmentService;
        }
        public async Task<LearningDashboardResponseDto> GetDashboardAsync(CancellationToken cancellationToken = default)
        {
            if (_currentUserService.UserId == null)
            {
                throw new UnauthorizedAccessException("User must be authenticated to access the learning dashboard.");
            }
            if (!_currentUserService.IsInRole(UserRoles.Student.ToString()))
            {
                throw new UnauthorizedAccessException("Only students can access the learning dashboard.");
            }
            var userId = _currentUserService.UserId.Value;
            var learningDashboardResponse = new LearningDashboardResponseDto();
            var enrollments = await _unitOfWork.Enrollments.Query().Where(
                e => e.UserId == userId).Include(e => e.Course).ToListAsync(cancellationToken);
            learningDashboardResponse.TotalEnrollments = enrollments.Count;
            learningDashboardResponse.CompletedCourses = enrollments.Count(e => e.IsCompleted);
            var inProgressEnrollments = enrollments.Where(e => e.ProgressPercent < 100 && !e.IsCompleted).Take(5).ToList();
            learningDashboardResponse.InProgressCourses = inProgressEnrollments.Count;
            var continueLearning = await _enrollmentService.GetContinueLearningAsync(cancellationToken);
            learningDashboardResponse.ContinueLearning = continueLearning.Items.ToList();
            learningDashboardResponse.RecentEnrollments = _mapper.Map<List<RecentEnrollmentDto>>(enrollments
                .OrderByDescending(e => e.EnrolledAt).Take(5).ToList());
            return learningDashboardResponse;
        }
    }
}
