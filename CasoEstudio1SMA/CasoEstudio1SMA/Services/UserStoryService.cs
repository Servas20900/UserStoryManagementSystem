using CasoEstudio1SMA.DTOs;
using CasoEstudio1SMA.Models;
using CasoEstudio1SMA.Repositories;

namespace CasoEstudio1SMA.Services
{
    public class UserStoryService : IUserStoryService
    {
        private readonly IUserStoryRepository _repository;

        public UserStoryService(IUserStoryRepository repository)
        {
            _repository = repository;
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

        public async Task<UserStoryResponseDto> CreateAsync(UserStoryCreateDto dto)
        {
            var entity = new UserStory
            {
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                AsignadoA = dto.AsignadoA,
                Estado = dto.Estado,
                Estimacion = dto.Estimacion
            };

            var created = await _repository.CreateAsync(entity);
            return MapToResponse(created);
        }

        public async Task<bool> UpdateAsync(int id, UserStoryUpdateDto dto)
        {
            var entity = new UserStory
            {
                Id = id,
                Titulo = dto.Titulo,
                Descripcion = dto.Descripcion,
                AsignadoA = dto.AsignadoA,
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
                AsignadoA = entity.AsignadoA,
                Estado = entity.Estado,
                Estimacion = entity.Estimacion
            };
        }
    }
}