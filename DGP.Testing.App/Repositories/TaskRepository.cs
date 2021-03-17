using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DGP.Testing.App.Models;
using Microsoft.EntityFrameworkCore;

namespace DGP.Testing.App.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TasksDbContext _dbContext;

        public TaskRepository(TasksDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Task<List<UserTask>> GetAll()
        {
            return _dbContext.Tasks.ToListAsync();
        }

        public Task Add(UserTask newTask)
        {
            _dbContext.Tasks.Add(newTask);
            
            return _dbContext.SaveChangesAsync();
        }

        public async Task Remove(Guid taskId)
        {
            var taskToRemove = await _dbContext.Tasks.SingleOrDefaultAsync(x => x.Id == taskId);

            if (taskToRemove != null)
            {
                _dbContext.Tasks.Remove(taskToRemove);
                await _dbContext.SaveChangesAsync();
            }
        }
    }
}