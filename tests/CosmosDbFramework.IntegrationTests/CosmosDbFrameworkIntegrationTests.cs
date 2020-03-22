using Azure.Cosmos;
using CosmosDbFramework.IntegrationTests.Contexts;
using CosmosDbFramework.IntegrationTests.Documents;
using CosmosDbFramework.IntegrationTests.Fixtures;
using System;
using System.Collections.Generic;
using System.Linq;
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

        [Fact(Skip = "Configure")]
        public async Task UpsertItemTest()
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

        [Fact(Skip = "Configure")]
        public async Task AddItemTest()
        {
            var actor = new Actor
            {
                Id = Guid.NewGuid().ToString("x").GetHashCode().ToString("x"),
                Country = "Spain",
                FirstName = "Antonio",
                LastName = "Banderas",
                Locality = "Andalucia"
            };
            await _videoStoreContext.Actors.CreateItemAsync(actor);
            
            var actorEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Equal("Andalucia", actorEntity.Locality);

            await _videoStoreContext.Actors.DeleteAsync(actor);

            var checkEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Null(checkEntity);
        }

        [Fact(Skip = "Configure")]
        public async Task UpdateItemTest()
        {
            var actor = new Actor
            {
                Id = Guid.NewGuid().ToString("x").GetHashCode().ToString("x"),
                Country = "Spain",
                FirstName = "Antonio",
                LastName = "Banderas",
                Locality = "Andalucia"
            };
            await _videoStoreContext.Actors.CreateItemAsync(actor);

            var actorEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Equal("Andalucia", actorEntity.Locality);
            
            actor.Locality = "Marbella";
            await _videoStoreContext.Actors.UpsertItemAsync(actor);

            actorEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Equal("Marbella", actorEntity.Locality);

            await _videoStoreContext.Actors.DeleteAsync(actor);

            var checkEntity = await _videoStoreContext.Actors.ReadItemAsync(actor.Id, actor.Country);

            Assert.Null(checkEntity);
        }

        [Fact(Skip = "Configure")]
        public async Task WhereQueryIteratorTest()
        {
            var actorList = Enumerable.Range(0, 1000).Select(c => new Actor
            {
                Id = Guid.NewGuid().ToString("x").GetHashCode().ToString("x"),
                FirstName = $"FirstName{c}",
                LastName = $"LastName{c}",
                Locality = $"Locality{c}",
                Country = "ES",
                Created = DateTime.Now
            }).ToList();

            await _videoStoreContext.Actors.CreateItemsAsync(actorList);

            var options = new QueryRequestOptions
            {
                PartitionKey = new PartitionKey("ES"),
                MaxItemCount = 20
            };

            var actors = new List<Actor>();
            string continuationToken= null;
            var queryIterator = _videoStoreContext.Actors.WhereQueryIterator(c => c.FirstName == "FirstName500", requestOptions: options);
            await foreach(var page in queryIterator.AsPages(continuationToken))
            {
                actors.AddRange(page.Values);
                queryIterator = _videoStoreContext.Actors.WhereQueryIterator(c => c.FirstName == "FirstName500", requestOptions: options);
            }

            Assert.Collection<Actor>(actors, c => Assert.Equal("FirstName500", c.FirstName));

            foreach(var actor in actorList)
            {
                await _videoStoreContext.Actors.DeleteAsync(actor);
            }
        }
    }
}
