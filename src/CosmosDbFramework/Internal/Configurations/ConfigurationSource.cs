using Azure.Cosmos;
using CosmosDbFramework.Internal.Builders;
using System;

namespace CosmosDbFramework.Internal.Configurations
{
    public sealed class ConfigurationSource<TDocument> : ConfigurationSource where TDocument : class
    {
        public ConfigurationSource(CosmosClient client) : base(client)
        {
        }

        internal Model<TDocument> Model { get; set; }

        public override void DisposeResources()
        {
            this.Model = null;
        }
    }

    public abstract class ConfigurationSource : IDisposable
    {
        public ConfigurationSource(CosmosClient client)
        {
            Source = client;
        }

        internal CosmosClient Source { get; set; }

        public abstract void DisposeResources();

        public void Dispose()
        {
            this.Source?.Dispose();
            this.DisposeResources();
        }
    }
}
