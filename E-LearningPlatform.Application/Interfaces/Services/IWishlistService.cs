using E_LearningPlatform.Application.DTOs.Wishlist;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface IWishlistService
    {
        Task AddCourseAsync (int courseId, CancellationToken cancellationToken = default);

        Task RemoveCourseAsync (int courseId, CancellationToken cancellationToken = default);

        Task<List<WishlistCourseDto>>
            GetMyWishlistAsync (CancellationToken cancellationToken = default);
    }
}
