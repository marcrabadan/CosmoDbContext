using Azure.Cosmos;
using Microsoft.Azure.Cosmos;
using System;

namespace CosmosDbFramework.Options.Builders
{
    public class CosmosDbOptionBuilder
    {
        private readonly CosmosDbOptions _cosmosDbOptions;
        private readonly CosmosDbOption _cosmosDbOption;
        private CosmosClientOptions _cosmosClientSettings;

        public CosmosDbOptionBuilder(CosmosDbOptions options)
        {
            _cosmosDbOptions = options;
            _cosmosDbOption = new CosmosDbOption();
            _cosmosClientSettings = new CosmosClientOptions();
        }

        public CosmosDbOptionBuilder Endpoint(string endpoint)
        {
            _cosmosDbOption.Endpoint = endpoint;
            return this;
        }

        public CosmosDbOptionBuilder AuthKeyOrResourceToken(string authKeyOrResourceToken)
        {
            _cosmosDbOption.AuthKeyOrResourceToken = authKeyOrResourceToken;
            return this;
        }

        public CosmosDbOptionBuilder Configure(Action<CosmosClientOptions> builder)
        {
            builder.Invoke(_cosmosClientSettings);
            _cosmosDbOption.Settings = _cosmosClientSettings;
            return this;
        }

        public virtual CosmosDbOptions Build()
        {
            _cosmosDbOptions.Options = _cosmosDbOption;
            return _cosmosDbOptions;
        }
    }
    public class CosmosDbOptionBuilder<TContext> : CosmosDbOptionBuilder where TContext : CosmosDbContext
    {
        public CosmosDbOptionBuilder() : base(new CosmosDbOptions<TContext>())
        {
        }
    }
}
