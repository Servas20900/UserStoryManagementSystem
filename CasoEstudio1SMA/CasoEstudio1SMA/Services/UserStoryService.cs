using CasoEstudio1SMA.DTOs;
using CasoEstudio1SMA.Models;
using CasoEstudio1SMA.Repositories;

namespace CasoEstudio1SMA.Services
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUserStoryRepository _repository;
        private readonly IUserRepository _userRepository;

        public UserStoryService(IUserStoryRepository repository, IUserRepository userRepository)
        {
            _repository = repository;
            _userRepository = userRepository;
        }

        public async Task<List<UserStoryResponseDto>> GetAllAsync()
        {
            var stories = await _repository.GetAllAsync();
            return stories.Select(MapToResponse).ToList();
        }

        public async Task<UserStoryResponseDto?> GetByIdAsync(int id)
        {
            var story = await _repository.GetByIdAsync(id);
            return story is null ? null : MapToResponse(story);
        }

        public async Task<UserStoryResponseDto?> CreateAsync(UserStoryCreateDto dto)
        {
            var userExists = await _userRepository.ExistsAsync(dto.UserId);
            if (!userExists)
            {
                return null;
            }

            var entity = new UserStory
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                UserId = dto.UserId,
                Estado = dto.Estado,
                Estimacion = dto.Estimacion
            };

            var created = await _repository.CreateAsync(entity);
            var hydrated = await _repository.GetByIdAsync(created.Id);
            return hydrated is null ? null : MapToResponse(hydrated);
        }

        public async Task<bool> UpdateAsync(int id, UserStoryUpdateDto dto)
        {
            var userExists = await _userRepository.ExistsAsync(dto.UserId);
            if (!userExists)
            {
                return false;
            }

            var entity = new UserStory
            {
                Id = id,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                UserId = dto.UserId,
                Estado = dto.Estado,
                Estimacion = dto.Estimacion
            };

            return await _repository.UpdateAsync(entity);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }

        private static UserStoryResponseDto MapToResponse(UserStory entity)
        {
            return new UserStoryResponseDto
            {
                Id = entity.Id,
                Titulo = entity.Titulo,
                Descripcion = entity.Descripcion,
                UserId = entity.UserId,
                NombreUsuario = entity.User is null ? string.Empty : $"{entity.User.Nombre} {entity.User.Apellidos}".Trim(),
                AvatarId = entity.User?.AvatarId ?? 1,
                Estado = entity.Estado,
                Estimacion = entity.Estimacion
            };
        }
    }
}