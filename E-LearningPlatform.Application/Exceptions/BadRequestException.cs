namespace E_LearningPlatform.Application.Exceptions;

public class BadRequestException : AppException
{
    public BadRequestException(string message) : base(message)
    {
    }
}
