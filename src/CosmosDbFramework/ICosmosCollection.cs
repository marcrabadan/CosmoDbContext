using Azure;
using Azure.Cosmos;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading;
using System.Threading.Tasks;

namespace CosmosDbFramework
{
    public interface ICosmosCollection<TDocument> where TDocument : class
    {
        Task<TDocument> CreateItemAsync(TDocument entity, CancellationToken cancellationToken = default);
        Task CreateItemsAsync(IEnumerable<TDocument> entities, CancellationToken cancellationToken = default);
        Task<TDocument> ReadItemAsync(string id, string partitionKey, CancellationToken cancellationToken = default);
        AsyncPageable<TDocument> GetItemQueryIterator(string queryText = null, string continuationToken = null, QueryRequestOptions requestOptions = null, CancellationToken cancellationToken = default);
        AsyncPageable<TDocument> GetItemQueryIterator(QueryDefinition queryDefinition, string continuationToken = null, QueryRequestOptions requestOptions = null, CancellationToken cancellationToken = default);
        AsyncPageable<TDocument> WhereQueryIterator(Expression<Func<TDocument, bool>> predicate, string continuationToken = null, QueryRequestOptions requestOptions = null, CancellationToken cancellationToken = default);
        Task UpsertItemAsync(TDocument entity, CancellationToken cancellationToken = default);
        Task ReplaceItemAsync(TDocument entity, string id, CancellationToken cancellationToken = default);
        Task DeleteAsync(TDocument entity, CancellationToken cancellationToken = default);        
    }
}
