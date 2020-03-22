using Azure.Cosmos;
using CosmosDbFramework.Extensions.DependencyInjection;
using CosmosDbFramework.IntegrationTests.Contexts;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace CosmosDbFramework.IntegrationTests.Fixtures
{
    public class CosmosFixture : IDisposable
    {
        public CosmosFixture()
        {
            var services = new ServiceCollection()
                .AddCosmosDbContext<VideoStoreContext>(config =>
                {
                    config.Endpoint("https://localhost:8081");
                    config.AuthKeyOrResourceToken("C2y6yDjf5/R+ob0N8A7Cgv30VRDJIWEHLM+4QDU5DE2nQ9nDuVTqobD4b8mGGyPMbIZnqyMsEcaGQy67XIw/Jw==");
                    config.AddDefaultSettings();
                })
                .BuildServiceProvider();

            VideoStoreContext = services.GetRequiredService<VideoStoreContext>();
        }
        public VideoStoreContext VideoStoreContext { get; set; }
        public void Dispose()
        {
            VideoStoreContext = null;
        }
    }
}
