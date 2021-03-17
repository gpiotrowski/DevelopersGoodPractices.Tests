using System;
using System.IO;
using System.Net.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Configuration;

namespace DGP.Testing.App.IntegrationTests.Fixture
{
    internal class AppFixture : IDisposable
    {
        private TestServer _server;
        private HttpClient _httpClient;
        private DatabaseFixture _databaseFixture;

        public AppFixture()
        {
            var projectDir = Directory.GetCurrentDirectory();

            _databaseFixture = new DatabaseFixture();

            _server = new TestServer(new WebHostBuilder()
                .UseEnvironment("Development")
                .UseContentRoot(projectDir)
                .UseConfiguration(new ConfigurationBuilder()
                    .SetBasePath(projectDir)
                    .AddJsonFile("integrationsettings.json")
                    .Build()
                )
                .ConfigureServices(x =>
                {
                    _databaseFixture.SetupDatabase(x);
                })
                .UseStartup<Startup>());


            _httpClient = _server.CreateClient();
        }

        public HttpClient GetClient()
        {
            return _httpClient;
        }

        public void Dispose()
        {
            _httpClient?.Dispose();
            _server?.Dispose();
            _databaseFixture.Dispose();
        }
    }
}
