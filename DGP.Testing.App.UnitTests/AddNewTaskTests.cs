using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;
using DGP.Testing.App.Models;
using DGP.Testing.App.Repositories;
using DGP.Testing.App.Services;
using Moq;
using NUnit.Framework;

namespace DGP.Testing.App.UnitTests
{
    public class AddNewTaskTests
    {
        [Test]
        public async Task AddingNewTaskShouldSaveItInRepository()
        {
            // Arrange
            var tasksRepositoryMock = new Mock<ITaskRepository>();

            var service = new TasksService(tasksRepositoryMock.Object);

            var existingTasks = new List<UserTask>()
            {
                new UserTask()
                {
                    Id = Guid.NewGuid(),
                    Text = "First task",
                    CreatedAt = DateTime.UtcNow
                }
            };

            tasksRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(existingTasks);

            var newTaskDto = new UserTaskDto()
            {
                Text = "Test task"
            };

            // Act
            await service.Add(newTaskDto);

            // Assert
            tasksRepositoryMock.Verify(x => x.Add(It.IsAny<UserTask>()), Times.Once);
        }
    }
}
