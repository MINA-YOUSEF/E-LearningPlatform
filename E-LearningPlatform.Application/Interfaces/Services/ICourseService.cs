using E_LearningPlatform.Application.Common;
using E_LearningPlatform.Application.DTOs.Course;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ICourseService
    {

        Task<CourseCreatedResponseDto> AddCourseAsync(AddCourseRequestDto request);
        Task<PagedResult<CourseCreatedResponseDto>> GetCoursesAsync(int pageNumber, int pageSize, CancellationToken cancellationToken);
        Task<CourseCreatedResponseDto> GetCourseByIdAsync(int courseId);
        Task<CourseCreatedResponseDto> UpdateCourseAsync(int courseId, AddCourseRequestDto request);


    }



}

