using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;
using DGP.Testing.App.Models;
using DGP.Testing.App.Repositories;

namespace DGP.Testing.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITaskRepository _taskRepository;

        public TasksController(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        [HttpGet]
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

        [HttpPost]
        public async Task Add(UserTaskDto newUserTask)
        {
            var task = new UserTask()
            {
                Id = Guid.NewGuid(),
                Text = newUserTask.Text,
                CreatedAt = DateTime.Now
            };

            await _taskRepository.Add(task);
        }

        [HttpDelete("{taskId}")]
        public async Task Delete(Guid taskId)
        {
            await _taskRepository.Remove(taskId);
        }
    }
}
