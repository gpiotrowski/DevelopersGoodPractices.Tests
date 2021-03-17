using DGP.Testing.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DGP.Testing.App.Repositories
{
    public class TasksDbContext : DbContext
    {
        public DbSet<UserTask> Tasks { get; set; }

        public TasksDbContext(DbContextOptions options) : base(options)
        {
        }
    }
}
