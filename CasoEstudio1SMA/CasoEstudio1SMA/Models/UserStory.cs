using System.ComponentModel.DataAnnotations;

namespace CasoEstudio1SMA.Models
{
    public class UserStory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        public User User { get; set; } = null!;

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Backlog";

        [Range(2, 13)]
        public int Estimacion { get; set; }
    }
}