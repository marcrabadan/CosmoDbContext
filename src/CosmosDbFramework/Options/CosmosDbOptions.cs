using Azure.Cosmos;
using System;

namespace CosmosDbFramework.Options
{
    public class CosmosDbOptions<TContext> : CosmosDbOptions where TContext : CosmosDbContext
    {
        public override Type ContextType => typeof(TContext);
    }

    public abstract class CosmosDbOptions
    {
        internal CosmosClient _cosmosClient;

        public CosmosDbOption Options { get; set; }

        internal CosmosClient CosmosClient
        {
            get
            {
                if (_cosmosClient == null)
                {
                    if (!string.IsNullOrEmpty(Options.AuthKeyOrResourceToken) &&
                        !string.IsNullOrEmpty(Options.Endpoint))
                    {
                        _cosmosClient = new CosmosClient(Options.Endpoint, Options.AuthKeyOrResourceToken, Options.Settings);
                    }
                    else if (string.IsNullOrEmpty(Options.AuthKeyOrResourceToken) &&
                      !string.IsNullOrEmpty(Options.Endpoint))
                    {
                        _cosmosClient = new CosmosClient(Options.Endpoint, Options.Settings);
                    }
                    else
                        throw new ArgumentNullException("Configure CosmosDb options from CosmosDbContext IoC");
                }
                return _cosmosClient;
            }
            set
            {
                _cosmosClient = value;
            }
        }

        public abstract Type ContextType { get; }
    }
}
