using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Jobs
{
    public interface ICertificateGeneratedJob
    {
        Task CertificateGeneratedAsync (int courseId ,CancellationToken cancellationToken);
    }
}
