using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Section;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ISectionService
    {
        public Task<SectionCreatedResponseDto> AddSectionAsync(AddSectionRequestDto request);
        public Task<SectionResponseDto> GetSectionByIdAsync(int sectionId);
        public Task<PagedResult<SectionResponseDto>> GetSectionsByCourseIdAsync(int courseId, int pageNumber, int pageSize, CancellationToken cancellationToken);
        public Task<SectionCreatedResponseDto> UpdateSectionAsync(int sectionId, AddSectionRequestDto request);
       public Task ReorderSectionsAsync(int courseId, List<int> request);
        public Task DeleteSectionAsync(int sectionId, int courseId);
    }
}
