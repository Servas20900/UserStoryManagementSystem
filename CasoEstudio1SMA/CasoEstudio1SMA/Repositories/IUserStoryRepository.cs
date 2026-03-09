using CasoEstudio1SMA.Models;

namespace CasoEstudio1SMA.Repositories
{
    public interface IUserStoryRepository
    {
        Task<List<UserStory>> GetAllAsync();
        Task<UserStory?> GetByIdAsync(int id);
        Task<UserStory> CreateAsync(UserStory userStory);
        Task<bool> UpdateAsync(UserStory userStory);
        Task<bool> DeleteAsync(int id);
    }
}