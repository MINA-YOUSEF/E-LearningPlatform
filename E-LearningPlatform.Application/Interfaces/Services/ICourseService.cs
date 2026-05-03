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
        Task<CourseCreatedResponseDto> GetCourseByIdAsync(int courseId);
        Task<CourseCreatedResponseDto> UpdateCourseAsync(int courseId, AddCourseRequestDto request);


    }



}

