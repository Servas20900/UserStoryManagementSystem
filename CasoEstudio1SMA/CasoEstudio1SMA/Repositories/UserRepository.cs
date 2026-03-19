using CasoEstudio1SMA.Data;
using CasoEstudio1SMA.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio1SMA.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly StoryDbContext _context;

        public UserRepository(StoryDbContext context)
        {
            _context = context;
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _context.Users.AsNoTracking().OrderBy(user => user.Nombre).ThenBy(user => user.Apellidos).ToListAsync();
        }

        public async Task<User?> GetByIdAsync(int id)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Id == id);
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users.AsNoTracking().FirstOrDefaultAsync(user => user.Email == email);
        }

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Users.AnyAsync(user => user.Id == id);
        }
    }
}