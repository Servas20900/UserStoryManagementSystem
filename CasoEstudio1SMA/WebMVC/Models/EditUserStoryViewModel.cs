using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebMVC.Models
{
    public class EditUserStoryViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(120)]
        public string Titulo { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        public string Descripcion { get; set; } = string.Empty;

        [Required]
        public int UserId { get; set; }

        [Required]
        [StringLength(30)]
        public string Estado { get; set; } = "Backlog";

        [Range(2, 13)]
        public int Estimacion { get; set; }

        public List<SelectListItem> Users { get; set; } = new();
    }
}