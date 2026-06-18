using System;

namespace E_LearningPlatform.Domain.Entities
{
    public class Certificate : BaseEntity
    {
        private Certificate () { }

        public Certificate (
            int userId,
            int courseId,
            string certificateNumber,
            string verificationCode,
            string recipientName)
        {


            UserId = userId;
            CourseId = courseId;
            CertificateNumber = certificateNumber;
            VerificationCode = verificationCode;
            RecipientName = recipientName;
            IssuedAtUtc = DateTime.UtcNow;
        }

        public int UserId { get; private set; }

        public int CourseId { get; private set; }

        public int? MediaId { get; private set; }

        public Course Course { get; private set; } = null!;

        public Media? CertificateFile { get; private set; }

        public DateTime IssuedAtUtc { get; private set; }

        public string CertificateNumber { get; private set; } =
            string.Empty;

        public string VerificationCode { get; private set; } =
            string.Empty;

        public string RecipientName { get; private set; } =
            string.Empty;

        public string? Grade { get; private set; }

        public DateTime? ExpiryDate { get; private set; }

        public void AddMedia (Media media)
        {
            if (media == null)
            {
                throw new ArgumentNullException(
                    nameof(media));
            }

            MediaId = media.Id;
            CertificateFile = media;
        }
    }
}