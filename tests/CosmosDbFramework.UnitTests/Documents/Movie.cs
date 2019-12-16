using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDbFramework.UnitTests.Documents
{
    public class Movie
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public List<Guid> Actors { get; set; }
    }
}
