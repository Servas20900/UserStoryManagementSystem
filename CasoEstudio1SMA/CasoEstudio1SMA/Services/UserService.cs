using CasoEstudio1SMA.DTOs;
using CasoEstudio1SMA.Models;
using CasoEstudio1SMA.Repositories;

namespace CasoEstudio1SMA.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;
        private readonly IAvatarService _avatarService;

        public UserService(IUserRepository repository, IAvatarService avatarService)
        {
            _repository = repository;
            _avatarService = avatarService;
        }

        public async Task<List<UserResponseDto>> GetAllAsync()
        {
            var users = await _repository.GetAllAsync();
            return users.Select(MapToResponse).ToList();
        }

        public async Task<UserResponseDto?> GetByIdAsync(int id)
        {
            var user = await _repository.GetByIdAsync(id);
            return user is null ? null : MapToResponse(user);
        }

        public async Task<UserResponseDto?> CreateAsync(UserCreateDto dto)
        {
            var existing = await _repository.GetByEmailAsync(dto.Email);
            if (existing is not null)
            {
                return null;
            }

            var avatarId = await _avatarService.GetRandomAvatarIdAsync();
            var entity = new User
            {
                Nombre = dto.Nombre,
                Apellidos = dto.Apellidos,
                Email = dto.Email,
                AvatarId = avatarId
            };

            var created = await _repository.CreateAsync(entity);
            return MapToResponse(created);
        }

        private static UserResponseDto MapToResponse(User user)
        {
            return new UserResponseDto
            {
                Id = user.Id,
                Nombre = user.Nombre,
                Apellidos = user.Apellidos,
                Email = user.Email,
                AvatarId = user.AvatarId
            };
        }
    }
}