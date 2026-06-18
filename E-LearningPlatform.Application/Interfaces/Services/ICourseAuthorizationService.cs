using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ICourseAuthorizationService
    {
        Task EnsureInstructorOwnsCourseAsync (
            int courseId,
            CancellationToken cancellationToken = default);
    }
}
