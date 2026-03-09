using System.ComponentModel.DataAnnotations;

namespace CasoEstudio1SMA.DTOs
{
    public class UserStoryUpdateDto
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

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Backlog";

        [Range(2, 13)]
        public int Estimacion { get; set; }
    }
}