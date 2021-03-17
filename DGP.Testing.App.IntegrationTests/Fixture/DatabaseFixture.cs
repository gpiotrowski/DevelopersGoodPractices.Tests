using System;
using System.IO;
using DGP.Testing.App.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace DGP.Testing.App.IntegrationTests.Fixture
{
    public class DatabaseFixture : IDisposable
    {
        private TasksDbContext _dbContext;
        private string _databaseFile;

        public DatabaseFixture()
        {
        }

        public void SetupDatabase(IServiceCollection serviceCollection)
        {
            _databaseFile = $"IntegrationTestDb-{Guid.NewGuid()}.db";

            serviceCollection.AddDbContext<TasksDbContext>(db => db.UseSqlite($"Filename={_databaseFile}"));

            var serviceProvider = serviceCollection.BuildServiceProvider();
            _dbContext = serviceProvider.GetService<TasksDbContext>();
            _dbContext.Database.Migrate();
        }

        public void Dispose()
        {
            _dbContext.Dispose();

            var projectDir = Directory.GetCurrentDirectory();
            File.Delete($"{projectDir}/{_databaseFile}");
        }
    }
}
