
namespace E_LearningPlatform.Application.Exceptions;

public class UnauthorizedException : AppException
{
    public UnauthorizedException(string message) : base(message)
    {
    }
}
