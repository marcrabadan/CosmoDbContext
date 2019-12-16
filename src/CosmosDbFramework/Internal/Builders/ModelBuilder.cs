using Azure.Cosmos;
using CosmosDbFramework.Internal.Configurations;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("CosmosDbFramework.UnitTests")]
[assembly: InternalsVisibleTo("CosmosDbFramework.IntegrationTests")]

namespace CosmosDbFramework.Internal.Builders
{
    public sealed class ModelBuilder
    {
        private readonly Dictionary<Type, object> _modelConfig = new Dictionary<Type, object>();
        internal CosmosClient Client { get; }

        public ModelBuilder(CosmosClient client)
        {
            Client = client;
        }

        public DocumentTypeBuilder<TDocument> Document<TDocument>() where TDocument : class
        {
            return new DocumentTypeBuilder<TDocument>(c => Apply(c));
        }

        internal void Apply<TDocument>(DocumentTypeBuilder<TDocument> modelBuilder) where TDocument : class
        {
            var config = new ConfigurationSource<TDocument>(Client);
            var model = new Model<TDocument>
            {
                DatabaseName = modelBuilder.DatabaseName,
                PartitionKey = modelBuilder.PartitionKey,
                ContainerName = modelBuilder.CollectionName,
                Throughput = modelBuilder.Throughput
            };
            config.Model = model;
            if (!_modelConfig.ContainsKey(typeof(TDocument)))
                _modelConfig.Add(typeof(TDocument), config);
            else
                _modelConfig[typeof(TDocument)] = config;
        }

        internal Dictionary<Type, object> Models => _modelConfig;
    }
}
