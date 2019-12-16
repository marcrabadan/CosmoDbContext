using CosmosDbFramework.UnitTests.Documents;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Xunit;
using CosmosDbFramework.Internal.Extensions;

namespace CosmosDbFramework.UnitTests
{
    public class PartitionKeyPathUnitTest
    {
        [Fact]
        public void GetPartitionKeyPathTests()
        {
            Expression<Func<Movie, object>> x = (Movie c) => c.Name;
            var paths = x.ToPartitionKeyPath();
            Assert.Contains("/name", paths);
        }        
    }
}
