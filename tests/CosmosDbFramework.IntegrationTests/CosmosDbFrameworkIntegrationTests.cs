using CosmosDbFramework.IntegrationTests.Contexts;
using CosmosDbFramework.IntegrationTests.Fixtures;
using System;
using System.Threading.Tasks;
using Xunit;

namespace CosmosDbFramework.IntegrationTests
{
    public class CosmosDbFrameworkIntegrationTests : IClassFixture<CosmosFixture>
    {
        private readonly VideoStoreContext _videoStoreContext;

        public CosmosDbFrameworkIntegrationTests(CosmosFixture cosmosFixture)
        {
            _videoStoreContext = cosmosFixture.VideoStoreContext;
        }

        [Fact]
        public async Task Test1()
        {
            var actor = new Documents.Actor
            {
                Id = Guid.NewGuid().ToString().GetHashCode().ToString("x"),
                Country = "Spain",
                FirstName = "Antonio",
                LastName = "Banderas",
                Locality = "Andalucia"
            };
            await _videoStoreContext.Actors.UpsertItemAsync(actor);

            var actorEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Equal("Andalucia", actorEntity.Locality);

            actor.Locality = "Marbella";
            await _videoStoreContext.Actors.UpsertItemAsync(actor);

            actorEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);
            
            Assert.Equal("Marbella", actorEntity.Locality);

            await _videoStoreContext.Actors.DeleteAsync(actor);

            actorEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Null(actorEntity);
        }
    }
}
