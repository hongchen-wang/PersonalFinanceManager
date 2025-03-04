using System.ComponentModel.DataAnnotations;

namespace PersonalFinanceManager.Server.Models
{
    public class RefreshTokenRequest
    {
        [Required]
        public string RefreshToken { get; set; }
    }
}
