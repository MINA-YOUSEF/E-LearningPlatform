using AutoMapper;
using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Section;
using E_LearningPlatform.Application.Exceptions;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class SectionService : ISectionService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICourseAuthorizationService _courseAuthorizationService;
        public SectionService (IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ICourseAuthorizationService courseAuthorizationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _courseAuthorizationService = courseAuthorizationService;
        }
        public async Task<SectionCreatedResponseDto> AddSectionAsync (AddSectionRequestDto request)
        {
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);

            if (course == null)
                throw new NotFoundException("Course not found.");

            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);

            var sectionCount = await _unitOfWork.Sections
                .Query()
                .Where(s => s.CourseId == request.CourseId)
                .CountAsync();

            var section = new Section(
                request.Title,
                sectionCount + 1,
                request.CourseId
            );

            section.SetDescription(request.Description);

            await _unitOfWork.Sections.AddAsync(section);

            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<SectionCreatedResponseDto>(section);
        }

        public async Task DeleteSectionAsync (int sectionId, int courseId)
        {
            if (sectionId <= 0) throw new BadRequestException("Invalid section ID.");
            if (courseId <= 0) throw new BadRequestException("Invalid course ID.");
            var section = await _unitOfWork.Sections.GetByIdAsync(sectionId);
            if (section == null)
                throw new NotFoundException("Section not found.");
            if (section.CourseId != courseId)
                throw new BadRequestException("Section does not belong to the specified course.");
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException("Course not found.");
            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);
            _unitOfWork.Sections.Remove(section);
            var sectionsToUpdate = await _unitOfWork.Sections
                .Query()
                .Where(s => s.CourseId == courseId
                && s.Order > section.Order)
                .ToListAsync();
            foreach (var s in sectionsToUpdate)
            {
                s.SetOrder(s.Order - 1);
            }
            await _unitOfWork.SaveChangesAsync();
            return;
        }

        public async Task<SectionCreatedResponseDto> UpdateSectionAsync (int sectionId, AddSectionRequestDto request)
        {
            if (sectionId <= 0) throw new BadRequestException("Invalid section ID.");
            var section = await _unitOfWork.Sections.GetByIdAsync(sectionId);
            if (section == null)
                throw new NotFoundException("Section not found.");
            if (section.CourseId != request.CourseId)
                throw new BadRequestException("Section does not belong to the specified course.");
            var course = await _unitOfWork.Courses.GetByIdAsync(request.CourseId);
            if (course == null)
                throw new NotFoundException("Course not found.");

            if (course.InstructorId != _currentUserService.UserId)
                throw new ForbiddenException("You are not allowed to modify this course.");


            section.SetTitle(request.Title);
            section.SetDescription(request.Description);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<SectionCreatedResponseDto>(section);
        }

        public async Task ReorderSectionsAsync (int courseId, List<int> sections)
        {
            if (courseId <= 0) throw new BadRequestException("Invalid course ID.");
            var course = await _unitOfWork.Courses.GetByIdAsync(courseId);
            if (course == null)
                throw new NotFoundException("Course not found.");
            await _courseAuthorizationService.EnsureInstructorOwnsCourseAsync(course.Id);

            //var sectionIds = sections.(s => s.SectionId).ToHashSet();
            var sectionIds = await _unitOfWork.Sections
                .Query()
                .Where(s => s.CourseId == courseId)
                .Select(s => s.Id)
                .ToListAsync();
            if (sectionIds.Count != sections.Count)
                throw new BadRequestException("The number of sections provided does not match the number of sections in the course.");
            var existingSections = await _unitOfWork.Sections
                .Query()
                .Where(s => s.CourseId == courseId && sectionIds.Contains(s.Id))
                .ToListAsync();
            if (existingSections.Count != sections.Count ||
            existingSections.Any(s => !sections.Contains(s.Id)))
                throw new BadRequestException("One or more sections do not belong to the specified course.");

            for (int i = 0; i < sections.Count; i++)
            {
                var sectionId = sections[i];
                var section = existingSections.First(s => s.Id == sectionId);
                section.SetOrder(i + 1);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<SectionResponseDto> GetSectionByIdAsync (int sectionId)
        {
            var section = await _unitOfWork.Sections.GetByIdAsync(sectionId);
            if (section == null)
                throw new NotFoundException("Section not found.");
            return _mapper.Map<SectionResponseDto>(section);
        }

        public async Task<PagedResult<SectionResponseDto>> GetSectionsByCourseIdAsync (int courseId, int pageNumber, int pageSize, CancellationToken cancellationToken)
        {
            var skip = (pageNumber - 1) * pageSize;
            var totalCount = await _unitOfWork.Sections.Query().Where(s => s.CourseId == courseId).CountAsync(cancellationToken);
            if (totalCount == 0)
                throw new NotFoundException("No sections found for the specified course.");
            if (skip >= totalCount)
                skip = 0; // Reset skip to 0 if it exceeds total count to return the first page
            var sections = await _unitOfWork.Sections.GetAllAsync(s => s.CourseId == courseId, skip, pageSize, cancellationToken);
            if (sections == null || sections.Count == 0)
                throw new NotFoundException("No sections found for the specified course.");

            return new PagedResult<SectionResponseDto>
            {
                Items = _mapper.Map<List<SectionResponseDto>>(sections),
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }
    }
}
