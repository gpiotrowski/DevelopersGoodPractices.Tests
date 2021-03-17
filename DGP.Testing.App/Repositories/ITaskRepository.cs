using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DGP.Testing.App.Models;

namespace DGP.Testing.App.Repositories
{
    public interface ITaskRepository
    {
        Task<List<UserTask>> GetAll();
        Task Add(UserTask newTask);
        Task Remove(Guid taskId);
    }
}