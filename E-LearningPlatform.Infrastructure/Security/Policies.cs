 
namespace E_LearningPlatform.Infrastructure.Security
{
   
        public static class Policies
        {
            public const string PasswordChangedRequired =
                "PasswordChangedRequired";

            public const string StudentFullAccess =
                "StudentFullAccess";

            public const string InstructorFullAccess =
                "InstructorFullAccess";

            public const string AdminFullAccess =
                "AdminFullAccess";

            public const string AdminOrInstructor =
                "AdminOrInstructor";

            public const string FullAccess =
                "FullAccess";
        }
    }
 