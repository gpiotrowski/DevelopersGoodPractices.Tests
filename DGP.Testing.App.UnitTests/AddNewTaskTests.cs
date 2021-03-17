using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Bogus;
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
        private Mock<GetCurrentDateTime> _getCurrentDateTime;

        [SetUp]
        public void SetUp()
        {
            _tasksRepositoryMock = new Mock<ITaskRepository>();
            _getCurrentDateTime = new Mock<GetCurrentDateTime>();
            _getCurrentDateTime.Setup(x => x()).Returns(() => DateTime.UtcNow);;

            _service = new TasksService(_tasksRepositoryMock.Object, _getCurrentDateTime.Object);
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

        [Test]
        [TestCase("Task with longer text than usual")]
        [TestCase("@H4CK3R!")]
        [TestCase("🦃")]
        public async Task ComplicatedTaskTextsShouldBeSavedSuccessfully(string taskText)
        {
            // Arrange
            var existingTasks = new List<UserTask>();

            _tasksRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(existingTasks);

            var newTaskDto = new UserTaskDto()
            {
                Text = taskText
            };

            // Act
            await _service.Add(newTaskDto);

            // Assert
            _tasksRepositoryMock.Verify(x => x.Add(It.IsAny<UserTask>()), Times.Once);
        }

        [Test]
        public async Task AddingTaskWithRandomNameShouldSaveItSuccessfully()
        {
            // Arrange
            var existingTasks = new List<UserTask>();

            _tasksRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(existingTasks);

            var newTaskDto = new Faker<UserTaskDto>()
                .RuleFor(x => x.Text, x => x.Lorem.Sentence())
                .Generate();

            // Act
            await _service.Add(newTaskDto);

            // Assert
            _tasksRepositoryMock.Verify(x => x.Add(It.IsAny<UserTask>()), Times.Once);
        }

        [Test]
        public async Task WhenAddingNewTaskDateTimeShouldBeSettedAsUtcNow()
        {
            // Arrange
            var existingTasks = new List<UserTask>();

            _tasksRepositoryMock.Setup(x => x.GetAll()).ReturnsAsync(existingTasks);

            var newTaskDto = new Faker<UserTaskDto>()
                .RuleFor(x => x.Text, x => x.Lorem.Sentence())
                .Generate();

            var createdAt = new DateTime(2021, 3, 1, 11, 23, 44);
            _getCurrentDateTime.Setup(x => x()).Returns(createdAt);

            // Act
            await _service.Add(newTaskDto);

            // Assert
            _tasksRepositoryMock.Verify(x => x.Add(It.Is<UserTask>(t => t.CreatedAt == createdAt)), Times.Once);
        }

    }
}
