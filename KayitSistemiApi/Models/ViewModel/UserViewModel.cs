using System.ComponentModel.DataAnnotations;

namespace KayitSistemiApi.Models.ViewModel
{
    public class UserViewModel
    {
     
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

        [Required]
        public IFormFile ProfilePicture { get; set; }
    }
}
