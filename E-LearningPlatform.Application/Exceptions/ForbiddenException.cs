namespace E_LearningPlatform.Application.Exceptions;

public class ForbiddenException : AppException
{
    public ForbiddenException(string message) : base(message)
    {
    }
}
