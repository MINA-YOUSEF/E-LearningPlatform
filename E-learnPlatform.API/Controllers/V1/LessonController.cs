using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Lesson;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_learnPlatform.API.Controllers.V1
{
    [Authorize]
    public class LessonController : BaseV1Controller
    {
        private readonly ILessonService _lessonService;

        public LessonController (
            ILessonService lessonService)
        {
            _lessonService = lessonService;
        }

        [Authorize(Policy = Policies.InstructorFullAccess)]
        [HttpPost]
        public async Task<ActionResult<LessonCreatedResponseDto>>
            Create (
            [FromForm] AddLessonRequestDto request)
        {
            var result =
                await _lessonService.AddLessonAsync(request);

            return Ok(result);
        }

        [Authorize(Policy = Policies.InstructorFullAccess)]
        [HttpPut("{lessonId:int}")]
        public async Task<ActionResult<LessonCreatedResponseDto>>
            Update (
            int lessonId,
            [FromForm] AddLessonRequestDto request)
        {
            var result =
                await _lessonService.UpdateLessonAsync(
                    lessonId,
                    request);

            return Ok(result);
        }

        [Authorize(Policy = Policies.InstructorFullAccess)]
        [HttpDelete("{lessonId:int}")]
        public async Task<IActionResult>
            Delete (
            int lessonId,
            [FromQuery] int sectionId)
        {
            await _lessonService.DeleteLessonAsync(
                lessonId,
                sectionId);

            return NoContent();
        }

        [HttpGet("{lessonId:int}")]
        public async Task<ActionResult<LessonDetailsResponseDto>>
            GetById (int lessonId)
        {
            var result =
                await _lessonService
                .GetLessonByIdAsync(lessonId);

            return Ok(result);
        }

        [HttpGet("section/{sectionId:int}")]
        public async Task<ActionResult<
            PagedResult<LessonDetailsResponseDto>>>
            GetBySection (
            int sectionId,
            [FromQuery] PagedRequest request)
        {
            var result =
                await _lessonService
                .GetLessonsBySectionIdAsync(
                    sectionId,
                    request);

            return Ok(result);
        }

        [Authorize(Policy = Policies.InstructorFullAccess)]
        [HttpPatch("section/{sectionId:int}/reorder")]
        public async Task<IActionResult>
            Reorder (
            int sectionId,
            [FromBody] List<int> orderedLessonIds)
        {
            await _lessonService
                .ReorderLessonsAsync(
                    sectionId,
                    orderedLessonIds);

            return NoContent();
        }
    }
}