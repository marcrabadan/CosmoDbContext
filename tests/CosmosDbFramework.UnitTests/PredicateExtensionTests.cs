using CosmosDbFramework.Internal.Extensions;
using CosmosDbFramework.UnitTests.Documents;
using System;
using System.Linq.Expressions;
using Xunit;

namespace CosmosDbFramework.UnitTests
{
    public class PredicateExtensionTests
    {
        [Fact]
        public void ShouldReturnSelectAllItemsTest()
        {
            Expression<Func<Actor, bool>> predicate = default;
            var query = predicate.GetCosmosDbQuery<Actor>("c");
            Assert.Equal("SELECT * FROM c", query);
        }

        [Fact]
        public void ShouldReturnSelectItemsByFirstNameTest()
        {
            Expression<Func<Actor, bool>> predicate = c => c.FirstName == "FirstName1";
            var query = predicate.GetCosmosDbQuery<Actor>("c");
            Assert.Equal(@"SELECT * FROM c WHERE c.firstName = ""FirstName1""", query);
        }

        [Fact]
        public void ShouldReturnSelectItemsByFirstNameAndLastNameTest()
        {
            Expression<Func<Actor, bool>> predicate = c => c.FirstName == "FirstName1" && c.LastName == "LastName1";
            var query = predicate.GetCosmosDbQuery<Actor>("c");
            Assert.Equal(@"SELECT * FROM c WHERE c.firstName = ""FirstName1"" AND c.lastName = ""LastName1""", query);
        }

        [Fact]
        public void ShouldReturnSelectItemsByFirstNameAndLastNameAndCountryTest()
        {
            Expression<Func<Actor, bool>> predicate = c => c.FirstName == "FirstName1" && c.LastName == "LastName1" && c.Country == "ES";
            var query = predicate.GetCosmosDbQuery<Actor>("c");
            Assert.Equal(@"SELECT * FROM c WHERE c.firstName = ""FirstName1"" AND c.lastName = ""LastName1"" AND c.country = ""ES""", query);
        }

        [Fact]
        public void ShouldReturnSelectItemsByFirstNameAndLastNameORCountryTest()
        {
            Expression<Func<Actor, bool>> predicate = c => c.FirstName == "FirstName1" && c.LastName == "LastName1" || c.Country == "ES";
            var query = predicate.GetCosmosDbQuery<Actor>("c");
            Assert.Equal(@"SELECT * FROM c WHERE c.firstName = ""FirstName1"" AND c.lastName = ""LastName1"" OR c.country = ""ES""", query);
        }

        [Fact]
        public void ShouldReturnSelectItemsByFirstNameOrFirstNameTest()
        {
            Expression<Func<Actor, bool>> predicate = c => c.FirstName == "FirstName1" || c.FirstName == "FirstName2";
            var query = predicate.GetCosmosDbQuery<Actor>("c");
            Assert.Equal(@"SELECT * FROM c WHERE c.firstName = ""FirstName1"" OR c.firstName = ""FirstName2""", query);
        }
    }
}
