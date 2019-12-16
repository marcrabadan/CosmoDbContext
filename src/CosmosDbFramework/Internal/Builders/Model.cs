using System;
using System.Linq.Expressions;

namespace CosmosDbFramework.Internal.Builders
{
    public sealed class Model<TDocument> : Model where TDocument : class
    {
        public Type DocumentType => typeof(TDocument);
        public Expression<Func<TDocument, object>> PartitionKey { get; set; }
    }

    public class Model
    {
        public string DatabaseName { get; set; }
        public string ContainerName { get; set; }
        public int Throughput { get; set; }
    }
}
