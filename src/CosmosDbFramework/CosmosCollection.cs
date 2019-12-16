using Azure;
using Azure.Cosmos;
using CosmosDbFramework.Internal.Configurations;
using CosmosDbFramework.Internal.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosDbFramework
{
    public sealed class CosmosCollection<TDocument> : ICosmosCollection<TDocument> where TDocument : class
    {
        private readonly ConfigurationSource<TDocument> _configurationSource;
        private readonly Database _database;
        private readonly Container _container;

        public CosmosCollection(ConfigurationSource<TDocument> configurationSource)
        {
            _configurationSource = configurationSource;

            _database = GetDatabaseAsync().GetAwaiter().GetResult();
            _container = GetContainerAsync(_database).GetAwaiter().GetResult();
        }

        public async Task<TDocument> CreateItemAsync(TDocument entity, CancellationToken cancellationToken = default)
        {
            var response = await _container.CreateItemAsync(entity, cancellationToken: cancellationToken);
            return response.Value;
        }

        public async Task CreateItemsAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken = default)
        {
            foreach (var entity in entities)
            {
                await _container.CreateItemAsync(entity, cancellationToken: cancellationToken);
            }
        }

        public async Task DeleteAsync(TDocument entity, CancellationToken cancellationToken = default)
        {
            var id = (entity as dynamic).Id;
            await _container.DeleteItemAsync<TDocument>(id, GetPartitionKeyValue(entity));
        }

        public async Task<TDocument> ReadItemAsync(string id, string partitionKey, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _container.ReadItemAsync<TDocument>(id, new PartitionKey(partitionKey), cancellationToken: cancellationToken);
            }
            catch (Exception)
            {
                return default;
            }
        }

        public AsyncPageable<TDocument> GetItemQueryIteratorGetItemQueryIterator<T>(string queryText = null, string continuationToken = null, QueryRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return _container.GetItemQueryIterator<TDocument>(queryText, continuationToken, requestOptions, cancellationToken);
        }

        public AsyncPageable<TDocument> GetItemQueryIterator(QueryDefinition queryDefinition, string continuationToken = null, QueryRequestOptions requestOptions = null, CancellationToken cancellationToken = default)
        {
            return _container.GetItemQueryIterator<TDocument>(queryDefinition, continuationToken, requestOptions, cancellationToken);
        }

        public Task UpsertItemAsync(TDocument entity, CancellationToken cancellationToken = default)
        {
            return _container.UpsertItemAsync(entity, GetPartitionKeyValue(entity), cancellationToken: cancellationToken);
        }

        public Task ReplaceItemAsync(TDocument entity, string id, CancellationToken cancellationToken = default)
        {
            return _container.ReplaceItemAsync(entity, id, GetPartitionKeyValue(entity), cancellationToken: cancellationToken);
        }

        private async Task<Database> GetDatabaseAsync()
        {
            var cosmosClient = _configurationSource.Source;
            var options = _configurationSource.Model;
            var response = await cosmosClient.CreateDatabaseIfNotExistsAsync(options.DatabaseName, options.Throughput);
            return response.Database;
        }

        private async Task<Container> GetContainerAsync(Database database)
        {
            var options = _configurationSource.Model;
            var containerProperties = new ContainerProperties(options.ContainerName, options.PartitionKey.ToPartitionKeyPath())
            {
                PartitionKeyDefinitionVersion = PartitionKeyDefinitionVersion.V2,
            };
            var response = await database.CreateContainerIfNotExistsAsync(containerProperties, options.Throughput);
            return response.Container;
        }

        private PartitionKey GetPartitionKeyValue(TDocument entity)
        {
            var partitionKey = _configurationSource.Model.PartitionKey;
            var partitionKeyValue = entity.GetPartitionKeyValue(partitionKey);
            return new PartitionKey(partitionKeyValue.ToString());
        }

    }
}
