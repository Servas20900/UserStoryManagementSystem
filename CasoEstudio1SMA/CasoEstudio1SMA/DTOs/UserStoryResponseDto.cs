namespace CasoEstudio1SMA.DTOs
{
    public class UserStoryResponseDto
    {
        public int Id { get; set; }
        public string Titulo { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public int UserId { get; set; }
        public string NombreUsuario { get; set; } = string.Empty;
        public int AvatarId { get; set; }
        public string Estado { get; set; } = string.Empty;
        public int Estimacion { get; set; }
    }
}