using E_LearningPlatform.Application.DTOs.Certificates;
using E_LearningPlatform.Infrastructure.Security;

namespace E_learnPlatform.API.Controllers.V1
{
    using E_LearningPlatform.Application.Interfaces.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    public class CertificateController : BaseV1Controller
    {
        private readonly ICertificateService _certificateService;

        public CertificateController (
            ICertificateService certificateService)
        {
            _certificateService = certificateService;
        }


        [Authorize(Policy = Policies.StudentFullAccess)]
        [HttpPost("generate/{courseId:int}")]
        public async Task<ActionResult<CertificateResponseDto>>
            GenerateCertificate (
            int courseId,
            CancellationToken cancellationToken)
        {
            var result =
                await _certificateService
                .GenerateCertificateAsync(
                    courseId,
                    cancellationToken);

            return Ok(result);
        }

        [Authorize(Policy = Policies.StudentFullAccess)]
        [HttpGet("my-certificates")]
        public async Task<ActionResult<List<CertificateResponseDto>>>
            GetMyCertificates (
            CancellationToken cancellationToken)
        {
            var result =
                await _certificateService
                .GetMyCertificatesAsync(
                    cancellationToken);

            return Ok(result);
        }

        [Authorize(Policy = Policies.StudentFullAccess)]
        [HttpGet("my/{certificateNumber}")]
        public async Task<ActionResult<CertificateResponseDto>>
            GetMyCertificate (
            string certificateNumber,
            CancellationToken cancellationToken)
        {
            var result =
                await _certificateService
                .GetMyCertificateByCertificateNumberAsync(
                    certificateNumber,
                    cancellationToken);

            return Ok(result);
        }


        [AllowAnonymous]
        [HttpGet("verify/{verificationCode}")]
        public async Task<ActionResult<CertificateResponseDto>>
            VerifyCertificate (
            string verificationCode,
            CancellationToken cancellationToken)
        {
            var result =
                await _certificateService
                .GetCertificateByVerificationCodeAsync(
                    verificationCode,
                    cancellationToken);

            return Ok(result);
        }
    }
}