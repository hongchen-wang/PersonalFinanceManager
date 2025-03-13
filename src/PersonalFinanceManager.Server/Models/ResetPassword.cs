namespace PersonalFinanceManager.Server.Models
{
    public class ResetPassword
    {
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
