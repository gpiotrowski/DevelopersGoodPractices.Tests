using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;

namespace DGP.Testing.App.Services
{
    public interface ITasksService
    {
        Task<List<UserTaskDto>> GetAll();
        Task Add(UserTaskDto newTask);
        Task Remove(Guid taskId);
    }
}