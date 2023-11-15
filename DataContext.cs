using Microsoft.EntityFrameworkCore;
using backend.Entities;

namespace backend
{
    public class DataContext : DbContext
    {
        public DbSet<ConfigUser> ConfigUsers { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Config> Configs { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<Answer> Answers { get; set; }

        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                entityType.SetTableName(entityType.DisplayName().ToLower());
            }

            modelBuilder.UseSerialColumns();

            Database.MigrateAsync();
            base.OnModelCreating(modelBuilder);
        }
    }
}