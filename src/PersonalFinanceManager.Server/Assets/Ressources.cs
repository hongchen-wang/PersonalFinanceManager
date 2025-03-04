namespace PersonalFinanceManager.Server.Assets
{
    public class Ressources
    {
        public static class UserRole
        {
            public const string Admin = "Admin";
            public const string User = "User";
        }

        public static class BusinessException
        {
            public const string UserAlreadyExists = "User already exists";
            public const string InvalidCredentials = "Invalid credentials";
            public const string JwtKeyMissing = "JWT key is missing";
        }
    }
}
