using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;
using DGP.Testing.App.Services;

namespace DGP.Testing.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TasksController : ControllerBase
    {
        private readonly ITasksService _tasksService;

        public TasksController(ITasksService tasksService)
        {
            _tasksService = tasksService;
        }

        [HttpGet]
        public async Task<List<UserTaskDto>> GetAll()
        {
            return await _tasksService.GetAll();
        }

        [HttpPost]
        public async Task Add(UserTaskDto newUserTask)
        {
            await _tasksService.Add(newUserTask);
        }

        [HttpDelete("{taskId}")]
        public async Task Delete(Guid taskId)
        {
            await _tasksService.Remove(taskId);
        }
    }
}
