namespace WebMVC.Models
{
    public class UserStoryViewModel
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int AvatarId { get; set; } = 1;
        public string AvatarImageUrl { get; set; } = string.Empty;
        public string Estado { get; set; } = string.Empty;
        public int Estimacion { get; set; }
    }
}