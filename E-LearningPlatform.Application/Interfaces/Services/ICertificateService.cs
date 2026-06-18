using E_LearningPlatform.Application.DTOs.Certificates;
using System;
using System.Collections.Generic;
using System.Text;

namespace E_LearningPlatform.Application.Interfaces.Services
{
    public interface ICertificateService
    {
        Task<CertificateResponseDto>
        GenerateCertificateAsync (
        int courseId,
        CancellationToken cancellationToken = default);

        Task<List<CertificateResponseDto>>
        GetMyCertificatesAsync (
        CancellationToken cancellationToken = default);

        Task<CertificateResponseDto> GetMyCertificateByCertificateNumberAsync (
            string certificateNumber,
            CancellationToken cancellationToken = default
        );
        Task<CertificateResponseDto>
            GetCertificateByVerificationCodeAsync (
            string verificationCode,
            CancellationToken cancellationToken = default);
    }
}
