using AutoMapper;
using E_LearningPlatform.Application.DTOs.Course;
using E_LearningPlatform.Application.DTOs.Lesson;
using E_LearningPlatform.Application.DTOs.Section;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<AddCourseRequestDto, Course>()
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src =>
                        new Money(src.PriceAmount, src.Currency)))

                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => new
                        Slug(src.Slug)))

                .ForMember(dest => dest.TotalDuration,
                    opt => opt.MapFrom(src =>
                        src.TotalDurationMinutes.HasValue ? new Duration(src.TotalDurationMinutes.Value)
            : null));

            CreateMap<Course, CourseCreatedResponseDto>()
                .ForMember(dest => dest.Price,
                    opt => opt.MapFrom(src => src.Price.Amount))
                .ForMember(dest => dest.Currency,
                    opt => opt.MapFrom(src => src.Price.Currency))
                .ForMember(dest => dest.Slug,
                    opt => opt.MapFrom(src => src.Slug.Value));

            CreateMap<Section, SectionCreatedResponseDto>();
            CreateMap<Lesson, LessonCreatedResponseDto>()
             .ForMember(dest => dest.DurationInMinutes,
             opt => opt.MapFrom(src => src.Duration.Minutes));
        }
    }
}
