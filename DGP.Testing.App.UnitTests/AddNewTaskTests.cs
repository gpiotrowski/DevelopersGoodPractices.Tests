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
        private Mock<ITaskRepository> _tasksRepositoryMock;
        private TasksService _service;

        [SetUp]
        public void SetUp()
        {
            _tasksRepositoryMock = new Mock<ITaskRepository>();
            _service = new TasksService(_tasksRepositoryMock.Object);
        }

        [Test]
        public async Task AddingNewTaskShouldSaveItInRepository()
        {
            // Arrange
            var existingTasks = new List<UserTask>()
            {
                new UserTask()
                {
                    Id = Guid.NewGuid(),
                    Text = "First task",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _tasksRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(existingTasks);

            var newTaskDto = new UserTaskDto()
            {
                Text = "Test task"
            };

            // Act
            await _service.Add(newTaskDto);

            // Assert
            _tasksRepositoryMock.Verify(x => x.Add(It.IsAny<UserTask>()), Times.Once);
        }

        [Test]
        public void AddingTaskWithExistingTextShouldThrowException()
        {
            // Arrange
            var existingTasks = new List<UserTask>()
            {
                new UserTask()
                {
                    Id = Guid.NewGuid(),
                    Text = "Task",
                    CreatedAt = DateTime.UtcNow
                }
            };

            _tasksRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(existingTasks);

            var newTaskDto = new UserTaskDto()
            {
                Text = "Task"
            };

            // Act & Assert
            Assert.ThrowsAsync<DuplicatedTaskTextException>(() => _service.Add(newTaskDto));
        }
    }
}
