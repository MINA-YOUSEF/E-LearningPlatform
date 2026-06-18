namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Common;
    using E_LearningPlatform.Application.DTOs.Course;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize]
    public class CourseApprovalController : BaseV1Controller
    {
        private readonly ICourseApprovalService _courseApprovalService;

        public CourseApprovalController (
            ICourseApprovalService courseApprovalService)
        {
            _courseApprovalService = courseApprovalService;
        }


        [Authorize(Policy = Policies.InstructorFullAccess)]
        [HttpPost("{courseId:int}/submit")]
        public async Task<IActionResult> Submit (
            int courseId,
            CancellationToken cancellationToken)
        {
            await _courseApprovalService.SubmitAsync(
                courseId,
                cancellationToken);

            return Ok(new
            {
                message = "Course submitted successfully."
            });
        }


        [Authorize(Policy = Policies.AdminFullAccess)]
        [HttpGet("pending")]
        public async Task<ActionResult<PagedResult<CourseApprovalDto>>>
            GetPending (
            [FromQuery] PagedRequest request,
            CancellationToken cancellationToken)
        {
            var result =
                await _courseApprovalService
                .GetPendingCoursesAsync(
                    request,
                    cancellationToken);

            return Ok(result);
        }


        [Authorize(Policy = Policies.AdminFullAccess)]
        [HttpPatch("{courseId:int}/approve")]
        public async Task<IActionResult>
            Approve (
            int courseId,
            CancellationToken cancellationToken)
        {
            await _courseApprovalService
                .ApproveAsync(
                    courseId,
                    cancellationToken);

            return NoContent();
        }


        [Authorize(Policy = Policies.AdminFullAccess)]
        [HttpPatch("{courseId:int}/reject")]
        public async Task<IActionResult>
            Reject (
            int courseId,
            [FromBody] RejectCourseRequestDto request,
            CancellationToken cancellationToken)
        {
            await _courseApprovalService
                .RejectAsync(
                    courseId,
                    request.Reason,
                    cancellationToken);

            return NoContent();
        }
    }
}
