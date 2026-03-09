using System.ComponentModel.DataAnnotations;

namespace WebMVC.Models
{
    public class CreateUserStoryViewModel
    {
        [Required]
        [StringLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string AsignadoA { get; set; } = string.Empty;

        [StringLength(30)]
        public string Estado { get; set; } = "Backlog";
    }
}