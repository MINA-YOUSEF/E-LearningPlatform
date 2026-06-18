using E_LearningPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.External
{
    public interface ICertificatePdfGenerator
    {
        byte[] GenerateCertificatePdf (
        Certificate certificate,
        string courseTitle,
        string instructorName);

    }
}
