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
            public const string InvalidRefreshToken = "Invalid refresh token";
            public const string NoRefreshTokenFound = "No refresh token found";
            public const string JwtKeyMissing = "JWT key is missing";
            public const string RefreshTokenExpired = "Invalid or expired refresh token.";
        }
    }
}
