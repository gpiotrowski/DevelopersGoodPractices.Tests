using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;
using DGP.Testing.App.Models;
using DGP.Testing.App.Repositories;

namespace DGP.Testing.App.Services
{

    public delegate DateTime GetCurrentDateTime();

    public class TasksService : ITasksService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly GetCurrentDateTime _getCurrentDateTime;

        public TasksService(ITaskRepository taskRepository, GetCurrentDateTime getCurrentDateTime)
        {
            _taskRepository = taskRepository;
            _getCurrentDateTime = getCurrentDateTime;
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
            var allTasks = await _taskRepository.GetAll();
            if (allTasks.Any(x => x.Text.Equals(newTask.Text)))
            {
                throw new DuplicatedTaskTextException();
            }

            var task = new UserTask()
            {
                Id = Guid.NewGuid(),
                Text = newTask.Text,
                CreatedAt = _getCurrentDateTime()
            };

            await _taskRepository.Add(task);
        }

        public async Task Remove(Guid taskId)
        {
            await _taskRepository.Remove(taskId);
        }
    }
}
