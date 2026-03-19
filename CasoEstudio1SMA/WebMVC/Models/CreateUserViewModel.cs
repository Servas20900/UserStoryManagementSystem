using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class CreateUserViewModel
    {
        [Required]
        [StringLength(25)]
        public string Nombre { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Apellidos { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;
    }
}