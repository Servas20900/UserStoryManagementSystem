using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

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
        public int UserId { get; set; }

        [StringLength(30)]
        public string Estado { get; set; } = "Backlog";

        public List<SelectListItem> Users { get; set; } = new();
    }
}