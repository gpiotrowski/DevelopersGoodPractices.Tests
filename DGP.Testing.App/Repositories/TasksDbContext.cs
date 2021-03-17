using DGP.Testing.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DGP.Testing.App.Repositories
{
    public class TasksDbContext : DbContext
    {
        private readonly DatabaseConfiguration _databaseConfiguration;
        public DbSet<UserTask> Tasks { get; set; }

        public TasksDbContext(DatabaseConfiguration databaseConfiguration)
        {
            _databaseConfiguration = databaseConfiguration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(_databaseConfiguration.ConnectionString);
        }
    }
}
