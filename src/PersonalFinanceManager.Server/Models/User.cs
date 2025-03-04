using PersonalFinanceManager.Server.Assets;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PersonalFinanceManager.Server.Models
{
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        // hashed password
        public string PasswordHash { get; set; }

        [Required]
        public string Role { get; set; } = Ressources.UserRole.User;

    }
}
