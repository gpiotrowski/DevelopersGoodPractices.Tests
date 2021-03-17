using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;
using DGP.Testing.App.Models;
using DGP.Testing.App.Repositories;

namespace DGP.Testing.App.Services
{
    public class TasksService : ITasksService
    {
        private readonly ITaskRepository _taskRepository;

        public TasksService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public async Task<List<UserTaskDto>> GetAll()
        {
            var tasks = await _taskRepository.GetAll();

            return tasks.Select(x => new UserTaskDto()
            {
                Id = x.Id,
                Text = x.Text,
                CreatedAt = x.CreatedAt
            }).ToList();
        }

        public async Task Add(UserTaskDto newTask)
        {
            var task = new UserTask()
            {
                Id = Guid.NewGuid(),
                Text = newTask.Text,
                CreatedAt = DateTime.Now
            };

            await _taskRepository.Add(task);
        }

        public async Task Remove(Guid taskId)
        {
            await _taskRepository.Remove(taskId);
        }
    }
}
