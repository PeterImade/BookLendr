using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UserService.Domain.Entities
{
    [Table("Users")]
    public class User
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required(ErrorMessage = "First name is a required field")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is a required field")]
        public string LastName { get; set; } = string.Empty;

        [EmailAddress]
        [Required(ErrorMessage = "Email is a required field")]
        public string Email { get; set; } = string.Empty;
        [StringLength(16)]
        [MinLength(8, ErrorMessage = "Password should be at least 8 characters")]
        public string PasswordHash { get; set; } = string.Empty;
    }
}
