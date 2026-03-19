using CasoEstudio1SMA.Data;
using CasoEstudio1SMA.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio1SMA.Repositories
{
    public class UserStoryRepository : IUserStoryRepository
    {
        private readonly StoryDbContext _context;

        public UserStoryRepository(StoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserStory>> GetAllAsync()
        {
            return await _context.UserStories
                .AsNoTracking()
                .Include(story => story.User)
                .ToListAsync();
        }

        public async Task<UserStory?> GetByIdAsync(int id)
        {
            return await _context.UserStories
                .AsNoTracking()
                .Include(story => story.User)
                .FirstOrDefaultAsync(story => story.Id == id);
        }

        public async Task<UserStory> CreateAsync(UserStory userStory)
        {
            _context.UserStories.Add(userStory);
            await _context.SaveChangesAsync();
            return userStory;
        }

        public async Task<bool> UpdateAsync(UserStory userStory)
        {
            var existing = await _context.UserStories.FirstOrDefaultAsync(story => story.Id == userStory.Id);
            if (existing is null)
            {
                return false;
            }

            existing.Titulo = userStory.Titulo;
            existing.Descripcion = userStory.Descripcion;
            existing.UserId = userStory.UserId;
            existing.Estado = userStory.Estado;
            existing.Estimacion = userStory.Estimacion;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var existing = await _context.UserStories.FirstOrDefaultAsync(story => story.Id == id);
            if (existing is null)
            {
                return false;
            }

            _context.UserStories.Remove(existing);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}