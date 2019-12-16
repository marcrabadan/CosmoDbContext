using CosmosDbFramework.Internal.Extensions;
using System;
using System.Linq.Expressions;

namespace CosmosDbFramework.Internal.Builders
{
    public sealed class DocumentTypeBuilder<T> where T : class
    {
        private Action<DocumentTypeBuilder<T>> _apply;

        public DocumentTypeBuilder(Action<DocumentTypeBuilder<T>> apply)
        {
            _apply = apply;
        }

        internal string DatabaseName { get; set; }
        internal string CollectionName { get; set; }
        internal Expression<Func<T, object>> PartitionKey { get; set; }
        internal int Throughput { get; set; } = 400;

        public DocumentTypeBuilder<T> WithDatabase(string name)
        {
            DatabaseName = name;
            _apply(this);
            return this;
        }

        public DocumentTypeBuilder<T> WithCollection(string name)
        {
            CollectionName = name;
            _apply(this);
            return this;
        }

        public DocumentTypeBuilder<T> WithPartitionKey(Expression<Func<T, object>> partitionKey)
        {
            PartitionKey = partitionKey;
            _apply(this);
            return this;
        }

        public DocumentTypeBuilder<T> WithThroughput(int throughput)
        {
            Throughput = throughput;
            _apply(this);
            return this;
        }
    }
}
