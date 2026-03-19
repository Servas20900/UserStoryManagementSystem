using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IUserStoryOrchestrationService
    {
        Task<List<UserStoryViewModel>> GetAllAsync();
        Task<UserStoryViewModel?> GetByIdAsync(int id);
        Task CreateAsync(CreateUserStoryViewModel model);
        Task<bool> UpdateAsync(EditUserStoryViewModel model);
        Task<bool> DeleteAsync(int id);
        Task UpdateStatusAsync(int id, string newStatus);
        Task<List<UserViewModel>> GetUsersAsync();
        Task CreateUserAsync(CreateUserViewModel model);
    }
}