using E_LearningPlatform.Application.Interfaces.External;
using E_LearningPlatform.Domain.Entities;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;


namespace E_LearningPlatform.Infrastructure.Services
{

    public class CertificatePdfGenerator
        : ICertificatePdfGenerator
    {
        public byte[] GenerateCertificatePdf (
            Certificate certificate,
            string courseTitle,
            string instructorName)
        {
            return Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);

                    page.Margin(50);

                    page.DefaultTextStyle(
                        x => x.FontSize(20));

                    page.Content().Column(col =>
                    {
                        col.Spacing(20);

                        col.Item().AlignCenter().Text(
                            "Certificate of Completion")
                            .Bold()
                            .FontSize(32);

                        col.Item().AlignCenter().Text(
                            "This certificate is proudly presented to");

                        col.Item().AlignCenter().Text(
                            certificate.RecipientName)
                            .Bold()
                            .FontSize(28);

                        col.Item().AlignCenter().Text(
                            $"For successfully completing the course:");

                        col.Item().AlignCenter().Text(
                            courseTitle)
                            .Bold()
                            .FontSize(24);

                        col.Item().AlignCenter().Text(
                            $"Instructor: {instructorName}");

                        col.Item().AlignCenter().Text(
                            $"Issued at: {certificate.IssuedAtUtc:yyyy-MM-dd}");

                        col.Item().AlignCenter().Text(
                            $"Certificate #: {certificate.CertificateNumber}");

                        col.Item().AlignCenter().Text(
                            $"Verification Code: {certificate.VerificationCode}");
                    });
                });
            }).GeneratePdf();
        }
    }
}

