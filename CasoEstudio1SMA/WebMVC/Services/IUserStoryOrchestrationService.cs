using WebMVC.Models;

namespace WebMVC.Services
{
    public interface IUserStoryOrchestrationService
    {
        Task<List<UserStoryViewModel>> GetAllAsync();
        Task CreateAsync(CreateUserStoryViewModel model);
        Task UpdateStatusAsync(int id, string newStatus);
        Task<List<UserViewModel>> GetUsersAsync();
        Task CreateUserAsync(CreateUserViewModel model);
    }
}