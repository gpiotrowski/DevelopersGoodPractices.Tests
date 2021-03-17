using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using DGP.Testing.App.Dtos;
using DGP.Testing.App.IntegrationTests.Fixture;
using DGP.Testing.App.Models;
using NUnit.Framework;

namespace DGP.Testing.App.IntegrationTests
{
    class TasksIntegrationTests
    {
        private HttpClient _client;
        private AppFixture _appFixture;


        [SetUp]
        public void SetUp()
        {
            _appFixture = new AppFixture();
            _client = _appFixture.GetClient();
        }

        [TearDown]
        public void TearDown()
        {
            _appFixture.Dispose();
        }

        [Test]
        public async Task GetAllShouldReturnEmptyListIfThereIsNoDataInDatabase()
        {
            // Arrange

            // Act
            var response = await _client.GetAsync("/api/tasks");
            
            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<List<UserTaskDto>>(responseString);

            // Assert
            Assert.IsEmpty(responseData);
        }

        [Test]
        public async Task GetAllShouldReturnAllTaskFromDatabase()
        {
            // Arrange
            var task = new UserTask()
            {
                Id = Guid.NewGuid(),
                Text = "First task",
                CreatedAt = DateTime.UtcNow
            };
            await _appFixture.GetDatabase().AddTask(task);

            // Act
            var response = await _client.GetAsync("/api/tasks");

            response.EnsureSuccessStatusCode();
            var responseString = await response.Content.ReadAsStringAsync();
            var responseData = JsonSerializer.Deserialize<List<UserTaskDto>>(responseString);

            // Assert
            Assert.AreEqual(1, responseData.Count);
        }
    }
}
