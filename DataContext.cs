using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace backend
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            Database.MigrateAsync();
            base.OnModelCreating(modelBuilder);
        }

    }
}