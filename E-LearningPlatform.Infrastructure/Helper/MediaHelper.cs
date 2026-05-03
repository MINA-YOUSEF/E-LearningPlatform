using E_LearningPlatform.Domain.Enums;

public static class MediaHelper
{
    public static MediaCategory MapToMediaCategory(LessonContentType contentType)
    {
        return contentType switch
        {
            LessonContentType.Video => MediaCategory.LessonVideo,

            LessonContentType.Downloadable => MediaCategory.LessonMaterial,

            LessonContentType.Article => MediaCategory.LessonMaterial,

            LessonContentType.Quiz => throw new InvalidOperationException(
                "Quiz lessons do not support media files."),

            _ => throw new ArgumentOutOfRangeException(nameof(contentType))
        };

    }
}