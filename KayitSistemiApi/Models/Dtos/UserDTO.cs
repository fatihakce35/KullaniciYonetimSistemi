using System.ComponentModel.DataAnnotations;

namespace KayitSistemiApi.Models.Dtos
{
    public class UserDTO
    {
        [Required]
        public int Id { get; set; }
        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; }

        public string ProfilePicture { get; set; }
    }
}
