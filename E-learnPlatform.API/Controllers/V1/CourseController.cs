using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Course;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
 using Microsoft.AspNetCore.Mvc;
 
namespace E_learnPlatform.API.Controllers.V1
{
    public class CourseController : BaseV1Controller
    {
        private readonly ICourseService _courseService;
        public CourseController(ICourseService courseService)
        {
            _courseService = courseService;
        }
        [Authorize(policy: Policies.InstructorFullAccess)]
        [HttpPost("create")]
        public async Task<ActionResult<CourseCreatedResponseDto>> CreateCourse([FromForm] AddCourseRequestDto requestDto)
        {
            var result = await _courseService.AddCourseAsync(requestDto);
            return Ok(result);
        }
        [Authorize(policy: Policies.FullAccess)]
        [HttpGet("{courseId}")]
        public async Task<ActionResult> GetCourseById(int courseId)
        {
            var result = await _courseService.GetCourseByIdAsync(courseId);
            return Ok(result);
        }
        [Authorize(policy: Policies.FullAccess)]
        [HttpGet("all")]
        public async Task<ActionResult> GetAllCourses([FromQuery] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            var result = await _courseService.GetCoursesAsync(pagedRequest.PageNumber, pagedRequest.PageSize, cancellationToken);
            return Ok(result);
        }

        [Authorize(policy: Policies.InstructorFullAccess)]
        [HttpPut("{courseId}")]
        public async Task<ActionResult> UpdateCourse(int courseId, [FromForm] AddCourseRequestDto requestDto)
        {
            var result = await _courseService.UpdateCourseAsync(courseId, requestDto);
            return Ok(result);

        }
    }

}
