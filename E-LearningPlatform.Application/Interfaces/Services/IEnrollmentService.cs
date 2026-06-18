using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.ContinueLearning;
using E_LearningPlatform.Application.DTOs.Enrollment;
using E_LearningPlatform.Domain.ValueObjects;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IEnrollmentService
    {
        Task EnrollAsync(
            int userId,
            int courseId,
            Money price,
            CancellationToken cancellationToken = default);

        Task<bool>IsUserEnrolledAsync(
            int userId,
            int courseId,
            CancellationToken cancellationToken = default);

        Task<PagedResult<EnrollmentCourseResponseDto>> GetUserEnrollmentsAsync(
            int userId,
            int pageNumber,
            int pageSize,
            CancellationToken cancellationToken = default);
        Task<EnrollmentCourseResponseDto> GetEnrollmentAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default);
        Task CompleteEnrollmentAsync(
            int enrollmentId,
            CancellationToken cancellationToken = default);
        Task DeactivateEnrollmentAsync(
        int enrollmentId,
        CancellationToken cancellationToken = default);

        Task<PagedResult<ContinueLearningResponseDto>> GetContinueLearningAsync(
            CancellationToken cancellationToken = default);
    }

}
