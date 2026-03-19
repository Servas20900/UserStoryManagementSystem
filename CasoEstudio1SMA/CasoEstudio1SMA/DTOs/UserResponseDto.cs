namespace CasoEstudio1SMA.DTOs
{
    public class UserResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int AvatarId { get; set; }
    }
}