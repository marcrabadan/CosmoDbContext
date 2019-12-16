using System;

namespace CosmosDbFramework.Internal.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class, AllowMultiple = false)]
    public sealed class SuppressCollectionInitializationAttribute : Attribute
    {
    }
}
