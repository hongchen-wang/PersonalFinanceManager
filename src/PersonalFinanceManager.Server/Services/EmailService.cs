using SendGrid;
using SendGrid.Helpers.Mail;

namespace PersonalFinanceManager.Server.Services
{
    public class EmailService
    {
        // TO DO: Example of how to send a password reset email by MS SendGrid
        public async Task SendPasswordResetEmail(string email, string resetToken)
        {
            var client = new SendGridClient("YOUR_SENDGRID_API_KEY");
            var from = new EmailAddress("noreply@yourdomain.com", "Your App");
            var subject = "Password Reset Request";
            var to = new EmailAddress(email);
            var htmlContent = $"<p>Click <a href='https://yourfrontend.com/reset?token={resetToken}'>here</a> to reset your password.</p>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlContent);
            await client.SendEmailAsync(msg);
        }
    }
}
