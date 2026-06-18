using AutoMapper;
using E_LearningPlatform.Application.DTOs.Certificates;
using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Application.Interfaces.Repositories;
using E_LearningPlatform.Application.Interfaces.Services;
using E_LearningPlatform.Domain.DomainEvent;
using E_LearningPlatform.Domain.Entities;
using E_LearningPlatform.Domain.Enums;
using E_LearningPlatform.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace E_LearningPlatform.Infrastructure.Services
{
    public class CertificateService : ICertificateService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ICurrentUserService _currentUserService;
        private readonly ICertificatePdfGenerator _certificatePdfGenerator;
        private readonly UserManager<AppUser> _userManager;
        private readonly ICloudinaryService _cloudinaryService;

        public CertificateService (
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICurrentUserService currentUserService,
            ICertificatePdfGenerator certificatePdfGenerator,
            UserManager<AppUser> userManager,
            ICloudinaryService cloudinaryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _currentUserService = currentUserService;
            _certificatePdfGenerator = certificatePdfGenerator;
            _userManager = userManager;
            _cloudinaryService = cloudinaryService;
        }

        public async Task<CertificateResponseDto> GenerateCertificateAsync (
            int courseId,
            CancellationToken cancellationToken = default)
        {
            var enrollment = await _unitOfWork
                .Enrollments
                .Query()
                .Include(x => x.Course)
                .FirstOrDefaultAsync(
                    x =>
                        x.CourseId == courseId &&
                        x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (enrollment == null)
            {
                throw new InvalidOperationException(
                    "User is not enrolled in this course.");
            }

            if (!enrollment.IsCompleted)
            {
                throw new InvalidOperationException(
                    "Course is not completed yet.");
            }

            var existingCertificate = await _unitOfWork
                .Certificates
                .Query()
                .Include(x => x.Course)
                .Include(x => x.CertificateFile)
                .FirstOrDefaultAsync(
                    x =>
                        x.CourseId == courseId &&
                        x.UserId == _currentUserService.UserId,
                    cancellationToken);

            if (existingCertificate != null)
            {
                return _mapper.Map<CertificateResponseDto>(
                    existingCertificate);
            }

            var student = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.Id == _currentUserService.UserId!.Value,
                    cancellationToken);

            if (student == null)
            {
                throw new InvalidOperationException(
                    "Student not found.");
            }

            var instructor = await _userManager.Users
                .FirstOrDefaultAsync(
                    x => x.Id == enrollment.Course.InstructorId,
                    cancellationToken);

            if (instructor == null)
            {
                throw new InvalidOperationException(
                    "Instructor not found.");
            }

            var certificateNumber =
                $"CERT-{Guid.NewGuid():N}"
                .Substring(0, 16)
                .ToUpper();

            var verificationCode =
                Guid.NewGuid()
                .ToString("N")
                .Substring(0, 8)
                .ToUpper();

            var certificate = new Certificate(
                student.Id,
                enrollment.CourseId,
                certificateNumber,
                verificationCode,
                student.FullName);

            var certificatePdf =
                _certificatePdfGenerator
                    .GenerateCertificatePdf(
                        certificate,
                        enrollment.Course.Title,
                        instructor.FullName);

            var fileSize = certificatePdf.Length;

            using var stream =
                new MemoryStream(certificatePdf);

            var uploadResult =
                await _cloudinaryService.UploadFileAsync(
                    stream,
                    $"certificates/{certificateNumber}.pdf",
                    cancellationToken);

            if (uploadResult == null)
            {
                throw new InvalidOperationException(
                    "Failed to upload certificate PDF.");
            }

            var media = new Media(
                uploadResult.PublicId,
                uploadResult.Url,
                MediaCategory.Certificate,
                MediaType.Pdf,
                fileSize,
                "application/pdf",
                student.Id);

            await _unitOfWork
                .Media
                .AddAsync(media, cancellationToken);

            certificate.AddMedia(media);

            await _unitOfWork
                .Certificates
                .AddAsync(certificate, cancellationToken);


            certificate.AddDomainEvent(
               new CertificateGeneratedEvent(
                   certificate.UserId,
                   enrollment.Course.Title));
            await _unitOfWork
               .SaveChangesAsync(cancellationToken);
            return _mapper.Map<CertificateResponseDto>(
                certificate);
        }

        public async Task<CertificateResponseDto>
            GetCertificateByVerificationCodeAsync (
            string verificationCode,
            CancellationToken cancellationToken = default)
        {
            var certificate = await _unitOfWork
                .Certificates
                .Query()
                .AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.CertificateFile)
                .FirstOrDefaultAsync(
                    x =>
                        x.VerificationCode ==
                        verificationCode,
                    cancellationToken);

            if (certificate == null)
            {
                throw new FileNotFoundException(
                    "Certificate not found.");
            }

            return _mapper.Map<CertificateResponseDto>(
                certificate);
        }

        public async Task<CertificateResponseDto>
            GetMyCertificateByCertificateNumberAsync (
            string certificateNumber,
            CancellationToken cancellationToken = default)
        {
            var certificate = await _unitOfWork
                .Certificates
                .Query()
                .AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.CertificateFile)
                .FirstOrDefaultAsync(
                    x =>
                        x.CertificateNumber ==
                            certificateNumber &&
                        x.UserId ==
                            _currentUserService.UserId,
                    cancellationToken);

            if (certificate == null)
            {
                throw new FileNotFoundException(
                    "Certificate not found.");
            }

            return _mapper.Map<CertificateResponseDto>(
                certificate);
        }

        public async Task<List<CertificateResponseDto>>
            GetMyCertificatesAsync (
            CancellationToken cancellationToken = default)
        {
            var certificates = await _unitOfWork
                .Certificates
                .Query()
                .AsNoTracking()
                .Include(x => x.Course)
                .Include(x => x.CertificateFile)
                .Where(x =>
                    x.UserId ==
                    _currentUserService.UserId)
                .OrderByDescending(
                    x => x.IssuedAtUtc)
                .ToListAsync(cancellationToken);

            if (!certificates.Any())
            {
                return new List<CertificateResponseDto>();
            }

            return _mapper.Map<
                List<CertificateResponseDto>>(
                    certificates);
        }
    }
}