using CasoEstudio1SMA.DTOs;

namespace CasoEstudio1SMA.Services
{
    public interface IUserStoryService
    {
        Task<List<UserStoryResponseDto>> GetAllAsync();
        Task<UserStoryResponseDto?> GetByIdAsync(int id);
        Task<UserStoryResponseDto?> CreateAsync(UserStoryCreateDto dto);
        Task<bool> UpdateAsync(int id, UserStoryUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}