using Azure.Cosmos;
using CosmosDbFramework.Internal;
using CosmosDbFramework.Internal.Builders;
using CosmosDbFramework.Internal.Configurations;
using CosmosDbFramework.Options;
using System;
using System.Linq;

namespace CosmosDbFramework
{
    public class CosmosDbContext
    {
        public CosmosDbContext(CosmosDbOptions options)
        {
            if (options == null)
                throw new InvalidOperationException("The options argument at CosmosDbContext is mandatory.");

            CosmosClient = options.CosmosClient;
            DiscoverAndInitializeCollections();
        }

        internal CosmosClient CosmosClient { get; }

        internal void DiscoverAndInitializeCollections()
        {
            var discoveryProperties = new PropertyDiscovery<CosmosDbContext>(this);
            discoveryProperties.Initialize(typeof(CosmosCollection<>), typeof(ICosmosCollection<>), "Collection");
        }

        public virtual void OnModelCreating(ModelBuilder modelBuilder)
        {
        }

        public CosmosCollection<TDocument> Collection<TDocument>() where TDocument : class
        {
            var modelBuilder = new ModelBuilder(CosmosClient);
            OnModelCreating(modelBuilder);
            var configurationSource = modelBuilder.Models.Any()
                ? (ConfigurationSource<TDocument>)modelBuilder.Models[typeof(TDocument)]
                : new ConfigurationSource<TDocument>(CosmosClient);
            return new CosmosCollection<TDocument>(configurationSource);
        }
    }
}
