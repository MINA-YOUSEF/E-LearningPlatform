
using E_LearningPlatform.Application.Common;
 using E_LearningPlatform.Application.DTOs.Section;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Infrastructure.Security;
 using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace E_learnPlatform.API.Controllers.V1
{
    public class SectionController : BaseV1Controller
    {
        private readonly ISectionService _sectionService;
        public SectionController(ISectionService sectionService)
        {
            _sectionService = sectionService;
        }
        [Authorize(policy: Policies.InstructorFullAccess)]
        [HttpPost("create")]
        public async Task<ActionResult<SectionCreatedResponseDto>> CreateSection([FromBody] AddSectionRequestDto section)
        {
            var result = await _sectionService.AddSectionAsync(section);

            return Ok(result);
        }
        [Authorize(policy: Policies.FullAccess)]
        [HttpGet("{sectionId}")]
        public async Task<ActionResult> GetSectionById(int sectionId)
        {
            var result = await _sectionService.GetSectionByIdAsync(sectionId);
            return Ok(result);
        }

        [Authorize(policy: Policies.FullAccess)]
        [HttpGet("all/{courseId}")]
        public async Task<ActionResult> GetAllSection(int courseId, [FromQuery] PagedRequest pagedRequest, CancellationToken cancellationToken)
        {
            var result = await _sectionService.GetSectionsByCourseIdAsync(courseId, pagedRequest.PageNumber, pagedRequest.PageSize, cancellationToken);
            return Ok(result);
        }

        [Authorize(policy: Policies.InstructorFullAccess)]
        [HttpPut("{sectionId}")]
        public async Task<ActionResult> UpdateSection(int sectionId, [FromBody] AddSectionRequestDto requestDto)
        {
            var result = await _sectionService.UpdateSectionAsync(sectionId, requestDto);
            return Ok(result);

        }


        [Authorize(policy: Policies.InstructorFullAccess)]
        [HttpDelete("{sectionId}")]
        public async Task<ActionResult> DeleteSection(int sectionId, [FromBody] int courseId)
        {
            await _sectionService.DeleteSectionAsync(sectionId, courseId);
            return Ok();
        }

        [Authorize(policy: Policies.InstructorFullAccess)]
        [HttpPut("reorder/{courseId}")]
        public async Task<ActionResult> ReorderSections(int courseId, [FromBody] List<int> sectionIds)
        {
            await _sectionService.ReorderSectionsAsync(courseId, sectionIds);
            return Ok();
        }
    }

}

