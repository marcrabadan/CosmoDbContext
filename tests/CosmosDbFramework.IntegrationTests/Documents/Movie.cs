using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace CosmosDbFramework.IntegrationTests.Documents
{
    public class Movie
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public List<string> Actors { get; set; }
    }
}
