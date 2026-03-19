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
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("Users");
                entity.HasKey(user => user.Id);

                entity.Property(user => user.Nombre)
                    .IsRequired()
                    .HasMaxLength(25);

                entity.Property(user => user.Apellidos)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(user => user.Email)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(user => user.AvatarId)
                    .HasDefaultValue(1);
            });

            modelBuilder.Entity<UserStory>(entity =>
            {
                entity.ToTable("UserStories");
                entity.HasKey(story => story.Id);

                entity.Property(story => story.Titulo)
                    .IsRequired()
                    .HasMaxLength(120);

                entity.Property(story => story.Descripcion)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(story => story.Estado)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(story => story.User)
                    .WithMany(user => user.UserStories)
                    .HasForeignKey(story => story.UserId)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}