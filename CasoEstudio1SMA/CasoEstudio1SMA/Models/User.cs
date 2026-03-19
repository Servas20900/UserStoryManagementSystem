namespace CasoEstudio1SMA.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public int AvatarId { get; set; } = 1;

        public ICollection<UserStory> UserStories { get; set; } = new List<UserStory>();
    }
}