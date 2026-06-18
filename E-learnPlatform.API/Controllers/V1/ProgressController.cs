namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.DTOs.Progress;
    using E_LearningPlatform.Application.Interfaces.Services;
    using E_LearningPlatform.Infrastructure.Security;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Policy = Policies.StudentFullAccess)]
    public class ProgressController : BaseV1Controller
    {
        private readonly IProgressService _progressService;

        public ProgressController (
            IProgressService progressService)
        {
            _progressService = progressService;
        }

        
        [HttpGet("course/{courseId:int}")]
        public async Task<ActionResult<CourseProgressResponseDto>>
            GetCourseProgress (
            int courseId,
            CancellationToken cancellationToken)
        {
            var result =
                await _progressService
                .GetCourseProgressAsync(
                    courseId,
                    cancellationToken);

            return Ok(result);
        }

        
        [HttpGet("lesson/{lessonId:int}")]
        public async Task<ActionResult<LessonProgressResponseDto>>
            GetLessonProgress (
            int lessonId,
            CancellationToken cancellationToken)
        {
            var result =
                await _progressService
                .GetLessonProgressAsync(
                    lessonId,
                    cancellationToken);

            return Ok(result);
        }


        [HttpPatch("lesson/{lessonId:int}")]
        public async Task<IActionResult>
            MarkLessonProgress (
            int lessonId,
            [FromBody] UpdateLessonProgressRequestDto request,
            CancellationToken cancellationToken)
        {
            await _progressService
                .MarkLessonProgressAsync(
                    lessonId,
                    request.WatchedSeconds,
                    request.Completed,
                    cancellationToken);

            return NoContent();
        }
    }
}
