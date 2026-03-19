namespace CasoEstudio1SMA.Services
{
    public interface IAvatarService
    {
        Task<int> GetRandomAvatarIdAsync();
    }
}