using CasoEstudio1SMA.Models;
using Microsoft.EntityFrameworkCore;

namespace CasoEstudio1SMA.Data
{
    public class StoryDbContext : DbContext
    {
        public StoryDbContext(DbContextOptions<StoryDbContext> options) : base(options)
        {
        }

        public DbSet<UserStory> UserStories => Set<UserStory>();
    }
}