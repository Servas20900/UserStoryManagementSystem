using CasoEstudio1SMA.DTOs;

namespace CasoEstudio1SMA.Services
{
    public interface IUserService
    {
        Task<List<UserResponseDto>> GetAllAsync();
        Task<UserResponseDto?> GetByIdAsync(int id);
        Task<UserResponseDto?> CreateAsync(UserCreateDto dto);
    }
}